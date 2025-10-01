using Octokit;
using Newtonsoft.Json;
using UserStoryReader.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace UserStoryReader.Services
{
    public class GitHubService
    {
        private readonly GitHubClient _client;
        private readonly string _owner;
        private readonly string _repository;

        public GitHubService(string token, string owner, string repository)
        {
            _client = new GitHubClient(new ProductHeaderValue("UserStoryReader"));
            _client.Credentials = new Credentials(token);
            _owner = owner;
            _repository = repository;
        }

        public async Task<List<UserStory>> GetUserStoriesAsync(string path = "user-stories")
        {
            var userStories = new List<UserStory>();

            try
            {
                // Get all contents from the user-stories directory
                var contents = await GetDirectoryContentsAsync(path);
                
                foreach (var content in contents)
                {
                    if (content.Type == ContentType.File && content.Name.EndsWith(".json"))
                    {
                        var userStory = await GetUserStoryFromFileAsync(content.Path);
                        if (userStory != null)
                        {
                            userStories.Add(userStory);
                        }
                    }
                    else if (content.Type == ContentType.Dir)
                    {
                        // Recursively get user stories from subdirectories
                        var subDirectoryStories = await GetUserStoriesAsync(content.Path);
                        userStories.AddRange(subDirectoryStories);
                    }
                }
            }
            catch (NotFoundException)
            {
                Console.WriteLine($"Directory '{path}' not found in repository {_owner}/{_repository}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading user stories: {ex.Message}");
            }

            return userStories;
        }

        private async Task<IReadOnlyList<RepositoryContent>> GetDirectoryContentsAsync(string path)
        {
            try
            {
                return await _client.Repository.Content.GetAllContents(_owner, _repository, path);
            }
            catch (NotFoundException)
            {
                Console.WriteLine($"Path '{path}' not found in repository");
                return new List<RepositoryContent>();
            }
        }

        private async Task<UserStory?> GetUserStoryFromFileAsync(string filePath)
        {
            try
            {
                var fileContents = await _client.Repository.Content.GetAllContents(_owner, _repository, filePath);
                var fileContent = fileContents[0];
                
                string jsonContent;
                
                Console.WriteLine($"Reading file: {filePath}, Encoding: {fileContent.Encoding}, Size: {fileContent.Size}");
                
                // Handle both base64 encoded and direct content
                if (fileContent.Encoding == "base64")
                {
                    try
                    {
                        // Decode base64 content
                        jsonContent = Encoding.UTF8.GetString(Convert.FromBase64String(fileContent.Content));
                    }
                    catch (FormatException)
                    {
                        // If base64 decoding fails, try using content directly
                        Console.WriteLine($"Base64 decoding failed for {filePath}, using content directly");
                        jsonContent = fileContent.Content;
                    }
                }
                else
                {
                    // Content is already decoded
                    jsonContent = fileContent.Content;
                }
                
                var userStory = JsonConvert.DeserializeObject<UserStory>(jsonContent);
                
                // Extract epic from file path if not set
                if (userStory != null && string.IsNullOrEmpty(userStory.Epic))
                {
                    var pathParts = filePath.Split('/');
                    if (pathParts.Length > 1)
                    {
                        userStory.Epic = pathParts[pathParts.Length - 2];
                    }
                }
                
                return userStory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var repo = await _client.Repository.Get(_owner, _repository);
                Console.WriteLine($"Successfully connected to repository: {repo.FullName}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to repository: {ex.Message}");
                return false;
            }
        }

        public async Task<List<UserStory>> GetUserStoriesFromIssuesAsync()
        {
            var userStories = new List<UserStory>();

            try
            {
                Console.WriteLine($"Reading user stories from GitHub Issues in {_owner}/{_repository}...");
                
                // Get all issues with the "user-story" or "enhancement" label
                var issueRequest = new RepositoryIssueRequest
                {
                    State = ItemStateFilter.All,
                    Filter = IssueFilter.All
                };

                Console.WriteLine("Fetching issues from GitHub...");
                var allIssues = await _client.Issue.GetAllForRepository(_owner, _repository, issueRequest);
                Console.WriteLine($"Retrieved {allIssues.Count} total issues");
                
                // Filter issues that have 'user-story' or 'enhancement' label and title contains [USER STORY]
                var issues = allIssues.Where(i => 
                    (i.Labels.Any(l => l.Name == "user-story" || l.Name == "enhancement")) &&
                    i.Title.Contains("[USER STORY]", StringComparison.OrdinalIgnoreCase)
                ).ToList();
                
                Console.WriteLine($"Found {issues.Count} user story issues");

                foreach (var issue in issues)
                {
                    try
                    {
                        var userStory = ParseIssueToUserStory(issue);
                        if (userStory != null)
                        {
                            userStories.Add(userStory);
                            Console.WriteLine($"Successfully parsed issue #{issue.Number}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing issue #{issue.Number}: {ex.Message}");
                        Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading user stories from issues: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            return userStories;
        }

        private UserStory? ParseIssueToUserStory(Issue issue)
        {
            try
            {
                Console.WriteLine($"Parsing issue #{issue.Number}: {issue.Title}");
                
                var userStory = new UserStory
                {
                    Id = $"#{issue.Number}",
                    Title = issue.Title.Replace("[USER STORY]", "").Trim(),
                    CreatedDate = issue.CreatedAt.DateTime,
                    LastModified = issue.UpdatedAt?.DateTime ?? issue.CreatedAt.DateTime,
                    Status = issue.State.StringValue,
                    Assignee = issue.Assignee?.Login ?? string.Empty,
                    EstimatedHours = 0,
                    Priority = "Medium",
                    Epic = "Unassigned"
                };

                // Parse the issue body
                var body = issue.Body ?? string.Empty;

                // Extract User Story section
                var userStoryMatch = Regex.Match(body, @"##\s*User Story\s*\*\*As a\*\*\s*(.+?)\s*\*\*I want\*\*\s*(.+?)\s*\*\*So that\*\*\s*(.+?)(?=##|$)", RegexOptions.Singleline);
                if (userStoryMatch.Success)
                {
                    userStory.Description = $"As a {userStoryMatch.Groups[1].Value.Trim()}, I want {userStoryMatch.Groups[2].Value.Trim()}, so that {userStoryMatch.Groups[3].Value.Trim()}";
                }

                // Extract Description section
                var descriptionMatch = Regex.Match(body, @"##\s*Description\s*(.+?)(?=##|$)", RegexOptions.Singleline);
                if (descriptionMatch.Success && string.IsNullOrEmpty(userStory.Description))
                {
                    userStory.Description = descriptionMatch.Groups[1].Value.Trim();
                }

                // Extract Acceptance Criteria
                var acceptanceCriteriaMatch = Regex.Match(body, @"##\s*Acceptance Criteria\s*(.+?)(?=##|$)", RegexOptions.Singleline);
                if (acceptanceCriteriaMatch.Success)
                {
                    var criteria = acceptanceCriteriaMatch.Groups[1].Value;
                    var criteriaLines = Regex.Matches(criteria, @"-\s*\[.\]\s*(.+?)(?=\n|$)");
                    foreach (Match match in criteriaLines)
                    {
                        userStory.AcceptanceCriteria.Add(match.Groups[1].Value.Trim());
                    }
                }

                // Extract Story Points
                var storyPointsMatch = Regex.Match(body, @"##\s*Story Points\s*\*\*Estimate:\*\*\s*(\d+)", RegexOptions.Singleline);
                if (storyPointsMatch.Success && !string.IsNullOrWhiteSpace(storyPointsMatch.Groups[1].Value))
                {
                    if (int.TryParse(storyPointsMatch.Groups[1].Value.Trim(), out int points))
                    {
                        userStory.EstimatedHours = points;
                    }
                }
                else
                {
                    userStory.EstimatedHours = 0;
                }

                // Extract Priority
                var priorityMatch = Regex.Match(body, @"##\s*Priority\s*\*\*Priority:\*\*\s*(\w+)", RegexOptions.Singleline);
                if (priorityMatch.Success)
                {
                    userStory.Priority = priorityMatch.Groups[1].Value.Trim();
                }

                // Extract Epic from Additional Notes
                var epicMatch = Regex.Match(body, @"Epic:\s*(.+?)(?=\n|$)", RegexOptions.Singleline);
                if (epicMatch.Success)
                {
                    userStory.Epic = epicMatch.Groups[1].Value.Trim();
                }

                // Extract Tags from Additional Notes
                var tagsMatch = Regex.Match(body, @"Tags:\s*(.+?)(?=\n|$)", RegexOptions.Singleline);
                if (tagsMatch.Success)
                {
                    var tags = tagsMatch.Groups[1].Value.Split(',');
                    userStory.Tags = tags.Select(t => t.Trim()).ToList();
                }

                // Get labels as tags if no tags found
                if (!userStory.Tags.Any())
                {
                    userStory.Tags = issue.Labels.Select(l => l.Name).ToList();
                }

                // Set status based on issue state
                userStory.Status = issue.State.StringValue == "open" ? "Open" : "Closed";

                return userStory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing issue #{issue.Number}: {ex.Message}");
                return null;
            }
        }
    }
}
