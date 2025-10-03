# Setting Up GitHub Repository for User Stories

## Step 1: Create GitHub Repository

1. Go to [GitHub](https://github.com) and sign in
2. Click the "+" icon and select "New repository"
3. Name your repository (e.g., "user-stories-test")
4. Make it public or private (your choice)
5. Initialize with a README
6. Click "Create repository"

## Step 2: Upload User Story Files

### Option A: Using GitHub Web Interface

1. In your new repository, click "Add file" > "Create new file"
2. Create the directory structure by typing: `user-stories/epic-1-authentication/US001-user-login.json`
3. Copy the content from the sample files in this project
4. Commit the file
5. Repeat for all user story files

### Option B: Using Git Command Line

```bash
# Clone your repository
git clone https://github.com/YOUR-USERNAME/user-stories-test.git
cd user-stories-test

# Copy the sample user stories
cp -r /path/to/UserStoryReader/sample-user-stories/* ./user-stories/

# Add and commit files
git add .
git commit -m "Add initial user stories"
git push origin main
```

## Step 3: Create GitHub Personal Access Token

1. Go to GitHub Settings > Developer settings > Personal access tokens > Tokens (classic)
2. Click "Generate new token (classic)"
3. Give it a descriptive name like "UserStoryReader"
4. Select scopes:
   - `repo` (Full control of private repositories)
   - `public_repo` (Access public repositories)
5. Click "Generate token"
6. **Important**: Copy the token immediately - you won't see it again!

## Step 4: Configure the Application

1. Open `appsettings.json` in the UserStoryReader project
2. Update the configuration:

```json
{
  "GitHub": {
    "Token": "your_github_token_here",
    "Owner": "your-github-username",
    "Repository": "user-stories-test"
  }
}
```

**Security Note**: For production use, store the token as an environment variable instead of in the config file.

## Step 5: Directory Structure in GitHub

Your repository should look like this:

```
user-stories-test/
├── README.md
└── user-stories/
    ├── epic-1-authentication/
    │   ├── US001-user-login.json
    │   └── US002-password-reset.json
    ├── epic-2-dashboard/
    │   ├── US003-dashboard-view.json
    │   └── US004-user-profile.json
    └── backlog/
        └── US005-advanced-search.json
```

## Step 6: Test the Setup

1. Open terminal in the UserStoryReader directory
2. Run the application:

```bash
dotnet run
```

The application should connect to your GitHub repository and display the user stories.

## Tips

- **Organization**: Group related user stories in epic directories
- **Naming**: Use consistent naming like `US###-short-description.json`
- **Validation**: Ensure JSON files are valid before committing
- **Security**: Never commit tokens to your repository
- **Backup**: Consider using GitHub releases for major versions of your user stories
