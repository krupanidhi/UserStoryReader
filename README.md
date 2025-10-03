# User Story Reader

A C# utility to read user stories from a GitHub repository.

## Repository Structure

This project demonstrates how to:
1. Structure user stories in a GitHub repository
2. Read user stories programmatically using C# and GitHub API
3. Parse and display user story data

## Setup Instructions

### 1. Create GitHub Repository

1. Create a new repository on GitHub
2. Add user stories in the `user-stories/` directory
3. Use JSON or Markdown format for user stories

### 2. GitHub Personal Access Token

1. Go to GitHub Settings > Developer settings > Personal access tokens
2. Generate a new token with `repo` scope
3. Copy the token for use in the application

### 3. Run the Application

```bash
dotnet run --project UserStoryReader
```

## User Story Format

User stories are stored as JSON files with the following structure:

```json
{
  "id": "US001",
  "title": "User Login",
  "description": "As a user, I want to log in to the system so that I can access my account",
  "acceptanceCriteria": [
    "User can enter username and password",
    "System validates credentials",
    "User is redirected to dashboard on success"
  ],
  "priority": "High",
  "status": "In Progress",
  "assignee": "John Doe",
  "estimatedHours": 8,
  "tags": ["authentication", "security"]
}
```

## Directory Structure

```
user-stories/
├── epic-1-authentication/
│   ├── US001-user-login.json
│   └── US002-password-reset.json
├── epic-2-dashboard/
│   ├── US003-dashboard-view.json
│   └── US004-user-profile.json
└── backlog/
    └── US005-advanced-search.json
```
