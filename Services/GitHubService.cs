using Octokit;
using Newtonsoft.Json;
using UserStoryReader.Models;
using System.Text;

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
    }
}
