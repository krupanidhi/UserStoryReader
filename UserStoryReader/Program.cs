using Microsoft.Extensions.Configuration;
using UserStoryReader.Services;
using UserStoryReader.Models;

namespace UserStoryReader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== User Story Reader ===\n");

            // Load configuration
            var config = LoadConfiguration();
            
            // Get GitHub settings
            var token = config["GitHub:Token"] ?? Environment.GetEnvironmentVariable("GITHUB_TOKEN");
            var owner = config["GitHub:Owner"] ?? "your-username";
            var repository = config["GitHub:Repository"] ?? "user-stories-test";

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("GitHub token not found. Please set it in appsettings.json or as environment variable GITHUB_TOKEN");
                Console.WriteLine("To create a token: GitHub Settings > Developer settings > Personal access tokens");
                return;
            }

            try
            {
                // Initialize GitHub service
                var gitHubService = new GitHubService(token, owner, repository);

                // Test connection
                Console.WriteLine("Testing GitHub connection...");
                if (!await gitHubService.TestConnectionAsync())
                {
                    return;
                }

                Console.WriteLine();

                // Ask user where to read user stories from
                Console.WriteLine("Where would you like to read user stories from?");
                Console.WriteLine("1. GitHub Issues (with 'user-story' label)");
                Console.WriteLine("2. JSON files in 'user-stories' directory");
                Console.Write("\nSelect option (1 or 2): ");
                var choice = Console.ReadLine();

                List<UserStory> userStories;

                if (choice == "1")
                {
                    // Read from GitHub Issues
                    Console.WriteLine("\nReading user stories from GitHub Issues...");
                    userStories = await gitHubService.GetUserStoriesFromIssuesAsync();
                }
                else
                {
                    // Read from JSON files
                    Console.WriteLine("\nReading user stories from JSON files...");
                    userStories = await gitHubService.GetUserStoriesAsync();
                }

                if (userStories.Count == 0)
                {
                    Console.WriteLine("No user stories found.");
                    if (choice == "1")
                    {
                        Console.WriteLine("Make sure you have issues with the 'user-story' label in your repository.");
                    }
                    else
                    {
                        Console.WriteLine("Make sure you have JSON files in the 'user-stories' directory.");
                    }
                    return;
                }

                // Display user stories
                DisplayUserStories(userStories);

                // Interactive menu
                await InteractiveMenu(userStories);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        static void DisplayUserStories(List<UserStory> userStories)
        {
            Console.WriteLine($"\nFound {userStories.Count} user stories:\n");

            // Group by epic
            var groupedStories = userStories.GroupBy(us => us.Epic).OrderBy(g => g.Key);

            foreach (var group in groupedStories)
            {
                Console.WriteLine($"📁 Epic: {group.Key}");
                Console.WriteLine(new string('-', 50));

                foreach (var story in group.OrderBy(s => s.Id))
                {
                    Console.WriteLine($"  {story}");
                    if (!string.IsNullOrEmpty(story.Assignee))
                    {
                        Console.WriteLine($"    👤 Assignee: {story.Assignee}");
                    }
                    if (story.EstimatedHours > 0)
                    {
                        Console.WriteLine($"    ⏱️  Estimated: {story.EstimatedHours} hours");
                    }
                    if (story.Tags.Any())
                    {
                        Console.WriteLine($"    🏷️  Tags: {string.Join(", ", story.Tags)}");
                    }
                    Console.WriteLine();
                }
            }
        }

        static async Task InteractiveMenu(List<UserStory> userStories)
        {
            while (true)
            {
                Console.WriteLine("\n=== Options ===");
                Console.WriteLine("1. View story details");
                Console.WriteLine("2. Filter by status");
                Console.WriteLine("3. Filter by priority");
                Console.WriteLine("4. Filter by assignee");
                Console.WriteLine("5. Search by keyword");
                Console.WriteLine("6. Export to CSV");
                Console.WriteLine("0. Exit");
                Console.Write("\nSelect an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ViewStoryDetails(userStories);
                        break;
                    case "2":
                        FilterByStatus(userStories);
                        break;
                    case "3":
                        FilterByPriority(userStories);
                        break;
                    case "4":
                        FilterByAssignee(userStories);
                        break;
                    case "5":
                        SearchByKeyword(userStories);
                        break;
                    case "6":
                        ExportToCsv(userStories);
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static Task ViewStoryDetails(List<UserStory> userStories)
        {
            Console.Write("Enter story ID: ");
            var id = Console.ReadLine();

            var story = userStories.FirstOrDefault(s => s.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (story == null)
            {
                Console.WriteLine("Story not found.");
                return Task.CompletedTask;
            }

            Console.WriteLine($"\n=== {story.Id}: {story.Title} ===");
            Console.WriteLine($"Description: {story.Description}");
            Console.WriteLine($"Status: {story.Status}");
            Console.WriteLine($"Priority: {story.Priority}");
            Console.WriteLine($"Assignee: {story.Assignee}");
            Console.WriteLine($"Estimated Hours: {story.EstimatedHours}");
            Console.WriteLine($"Epic: {story.Epic}");
            Console.WriteLine($"Tags: {string.Join(", ", story.Tags)}");
            Console.WriteLine($"Created: {story.CreatedDate:yyyy-MM-dd}");
            Console.WriteLine($"Last Modified: {story.LastModified:yyyy-MM-dd}");
            
            Console.WriteLine("\nAcceptance Criteria:");
            foreach (var criteria in story.AcceptanceCriteria)
            {
                Console.WriteLine($"  ✓ {criteria}");
            }
            
            return Task.CompletedTask;
        }

        static void FilterByStatus(List<UserStory> userStories)
        {
            var statuses = userStories.Select(s => s.Status).Distinct().OrderBy(s => s).ToList();
            
            Console.WriteLine("\nAvailable statuses:");
            for (int i = 0; i < statuses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {statuses[i]}");
            }

            Console.Write("Select status number: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= statuses.Count)
            {
                var selectedStatus = statuses[choice - 1];
                var filtered = userStories.Where(s => s.Status == selectedStatus).ToList();
                Console.WriteLine($"\nStories with status '{selectedStatus}':");
                DisplayUserStories(filtered);
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        static void FilterByPriority(List<UserStory> userStories)
        {
            var priorities = userStories.Select(s => s.Priority).Distinct().OrderBy(s => s).ToList();
            
            Console.WriteLine("\nAvailable priorities:");
            for (int i = 0; i < priorities.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {priorities[i]}");
            }

            Console.Write("Select priority number: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= priorities.Count)
            {
                var selectedPriority = priorities[choice - 1];
                var filtered = userStories.Where(s => s.Priority == selectedPriority).ToList();
                Console.WriteLine($"\nStories with priority '{selectedPriority}':");
                DisplayUserStories(filtered);
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        static void FilterByAssignee(List<UserStory> userStories)
        {
            var assignees = userStories.Select(s => s.Assignee).Where(a => !string.IsNullOrEmpty(a)).Distinct().OrderBy(s => s).ToList();
            
            Console.WriteLine("\nAvailable assignees:");
            for (int i = 0; i < assignees.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {assignees[i]}");
            }

            Console.Write("Select assignee number: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= assignees.Count)
            {
                var selectedAssignee = assignees[choice - 1];
                var filtered = userStories.Where(s => s.Assignee == selectedAssignee).ToList();
                Console.WriteLine($"\nStories assigned to '{selectedAssignee}':");
                DisplayUserStories(filtered);
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        static void SearchByKeyword(List<UserStory> userStories)
        {
            Console.Write("Enter search keyword: ");
            var keyword = Console.ReadLine();

            if (string.IsNullOrEmpty(keyword))
            {
                Console.WriteLine("No keyword provided.");
                return;
            }

            var filtered = userStories.Where(s => 
                s.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                s.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                s.AcceptanceCriteria.Any(ac => ac.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                s.Tags.Any(tag => tag.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            Console.WriteLine($"\nStories containing '{keyword}':");
            DisplayUserStories(filtered);
        }

        static void ExportToCsv(List<UserStory> userStories)
        {
            var fileName = $"user_stories_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            using (var writer = new StreamWriter(filePath))
            {
                // Write header
                writer.WriteLine("ID,Title,Description,Status,Priority,Assignee,EstimatedHours,Epic,Tags,AcceptanceCriteria");

                // Write data
                foreach (var story in userStories)
                {
                    var line = $"\"{story.Id}\",\"{story.Title}\",\"{story.Description}\",\"{story.Status}\",\"{story.Priority}\",\"{story.Assignee}\",{story.EstimatedHours},\"{story.Epic}\",\"{string.Join("; ", story.Tags)}\",\"{string.Join("; ", story.AcceptanceCriteria)}\"";
                    writer.WriteLine(line);
                }
            }

            Console.WriteLine($"User stories exported to: {filePath}");
        }
    }
}
