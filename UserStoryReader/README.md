# UserStoryReader

> **⚠️ Team Member?** Get the shared `appsettings.json` from your team lead and see [SHARED_SETUP.md](SHARED_SETUP.md)
> 
> **⚠️ New User?** If you're getting "No user stories found" error, see [SETUP_GUIDE.md](SETUP_GUIDE.md) for detailed setup instructions.

A C# console application that reads and manages user stories from GitHub repositories. Supports reading from both GitHub Issues and JSON files.

## Features

- ✅ Read user stories from **GitHub Issues** (with 'user-story' label)
- ✅ Read user stories from **JSON files** in repository
- ✅ Filter by status, priority, and assignee
- ✅ Search by keyword
- ✅ Export to CSV
- ✅ Group stories by Epic
- ✅ Interactive console menu

## Setup

### 1. Prerequisites

- .NET 8.0 SDK
- GitHub Personal Access Token

### 2. Create GitHub Personal Access Token

1. Go to: https://github.com/settings/tokens/new
2. Name: `UserStoryReader`
3. Scopes: `repo` (full control of repositories)
4. Click **Generate token**
5. Copy the token

### 3. Configure Application

1. Copy `appsettings.example.json` to `appsettings.json`
2. Update the configuration:

```json
{
  "GitHub": {
    "Token": "your-github-token-here",
    "Owner": "krupanidhi",
    "Repository": "UserStoryReader"
  }
}
```

**Or** set environment variable:
```powershell
$env:GITHUB_TOKEN = "your-token-here"
```

### 4. Build and Run

```powershell
dotnet build
dotnet run
```

## Usage

### Reading from GitHub Issues

1. Create issues in your repository with the **'user-story'** label
2. Use the user story template (`.github/ISSUE_TEMPLATE/user-story.md`)
3. Run the application and select option **1** (GitHub Issues)

### Reading from JSON Files

1. Create JSON files in the `user-stories` directory of your repository
2. Run the application and select option **2** (JSON files)

### Example User Story Issue Format

```markdown
## User Story
**As a** developer
**I want** to read user stories from GitHub Issues
**So that** I can manage stories directly in GitHub

## Description
Enhance the UserStoryReader to parse GitHub Issues...

## Acceptance Criteria
- [ ] Read issues with 'user-story' label
- [ ] Parse issue body into UserStory model
- [ ] Display stories in console

## Story Points
**Estimate:** 8

## Priority
**Priority:** High

## Additional Notes
Epic: Core Features
Tags: enhancement, github-api
```

## Interactive Menu Options

1. **View story details** - See full details of a specific story
2. **Filter by status** - Show stories by status (Open/Closed)
3. **Filter by priority** - Show stories by priority level
4. **Filter by assignee** - Show stories assigned to specific users
5. **Search by keyword** - Search across title, description, and criteria
6. **Export to CSV** - Export all stories to CSV file

## Dependencies

- **Octokit** (9.0.0) - GitHub API client
- **Newtonsoft.Json** (13.0.3) - JSON parsing
- **Microsoft.Extensions.Configuration** (8.0.0) - Configuration management

## Project Structure

```
UserStoryReader/
├── Models/
│   └── UserStory.cs          # User story data model
├── Services/
│   └── GitHubService.cs      # GitHub API integration
├── Program.cs                # Main application logic
├── appsettings.json          # Configuration (gitignored)
└── appsettings.example.json  # Configuration template
```

## Security Notes

⚠️ **Never commit `appsettings.json` with your GitHub token!**

- The `.gitignore` file excludes `appsettings.json`
- Use environment variables for CI/CD pipelines
- Revoke tokens immediately if accidentally exposed

## Contributing

1. Create user stories as GitHub Issues with 'user-story' label
2. Follow the user story template format
3. Test changes locally before committing

## License

MIT License
