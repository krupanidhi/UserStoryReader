# UserStoryReader - Setup Guide for New Users

## Problem: "No user stories found" Error

If you see this error when running the app:
```
Found 0 user story issues
No user stories found.
Make sure you have issues with the 'user-story' label in your repository.
```

This means your GitHub repository doesn't have any issues with the required label and format.

## Solution: Setting Up Your Repository

### Step 1: Configure Your GitHub Token

1. **Create a GitHub Personal Access Token:**
   - Go to GitHub Settings → Developer settings → Personal access tokens → Tokens (classic)
   - Click "Generate new token (classic)"
   - Give it a name like "UserStoryReader"
   - Select scopes: `repo` (Full control of private repositories)
   - Click "Generate token"
   - **Copy the token immediately** (you won't see it again!)

2. **Add the token to appsettings.json:**
   ```bash
   cd C:\Users\[YourUsername]\CascadeProjects\UserStoryReader\UserStoryReader
   copy appsettings.example.json appsettings.json
   ```

3. **Edit appsettings.json:**
   ```json
   {
     "GitHub": {
       "Token": "ghp_YOUR_ACTUAL_TOKEN_HERE",
       "Owner": "your-github-username",
       "Repository": "your-repository-name"
     }
   }
   ```

### Step 2: Create User Story Issues in GitHub

The app requires issues to have:
1. ✅ A label called **"user-story"** (or "enhancement")
2. ✅ Title containing **"[USER STORY]"**

#### Option A: Create the "user-story" Label

1. Go to your GitHub repository
2. Click on "Issues" tab
3. Click "Labels"
4. Click "New label"
5. Name: `user-story`
6. Color: Choose any color (e.g., #0075ca)
7. Click "Create label"

#### Option B: Create a User Story Issue

1. Go to your repository on GitHub
2. Click "Issues" → "New issue"
3. **Title:** `[USER STORY] User can login to the system`
4. **Add label:** Select "user-story" (or "enhancement")
5. **Body:** Use this template:

```markdown
## User Story
**As a** registered user
**I want** to login with my credentials
**So that** I can access my personalized dashboard

## Description
The system should provide a secure login mechanism for registered users.

## Acceptance Criteria
- [ ] User can enter username and password
- [ ] System validates credentials
- [ ] Successful login redirects to dashboard
- [ ] Failed login shows error message
- [ ] Password is masked during entry

## Story Points
**Estimate:** 5

## Priority
**Priority:** High

## Additional Notes
Epic: Authentication
Tags: security, login, authentication
```

6. Click "Submit new issue"

### Step 3: Run the Application

```bash
cd C:\Users\[YourUsername]\CascadeProjects\UserStoryReader\UserStoryReader
dotnet run
```

Select option **1** (GitHub Issues) when prompted.

## Troubleshooting

### Issue: "GitHub token not found"
**Solution:** Make sure `appsettings.json` exists and contains your token.

### Issue: "Failed to connect to repository"
**Solutions:**
- Check that Owner and Repository names are correct in `appsettings.json`
- Verify your GitHub token has `repo` scope
- Make sure the repository exists and you have access to it

### Issue: "Retrieved X total issues" but "Found 0 user story issues"
**Solutions:**
- Create the "user-story" label in your repository
- Add the "user-story" label to existing issues
- Make sure issue titles contain "[USER STORY]"

### Issue: Token permissions error
**Solution:** Regenerate your token with the `repo` scope enabled.

## Quick Test

To verify your setup is working:

1. Create one test issue with:
   - Title: `[USER STORY] Test user story`
   - Label: `user-story`
   - Body: `As a tester, I want to verify the app works, so that I can use it.`

2. Run the app:
   ```bash
   dotnet run
   ```

3. Select option 1 (GitHub Issues)

4. You should see:
   ```
   Retrieved 1 total issues
   Found 1 user story issues
   Successfully parsed issue #1
   ```

## Alternative: Use JSON Files Instead

If you don't want to use GitHub Issues, you can use JSON files:

1. Create a `user-stories` directory in your repository
2. Add JSON files with this format:
   ```json
   {
     "id": "US-001",
     "title": "User Login",
     "description": "As a user, I want to login...",
     "acceptanceCriteria": [
       "User can enter credentials",
       "System validates login"
     ],
     "priority": "High",
     "estimatedHours": 8,
     "status": "Open",
     "epic": "Authentication",
     "tags": ["security", "login"]
   }
   ```
3. Run the app and select option 2 (JSON files)

## Need Help?

If you're still having issues:
1. Check that you're using the correct repository name
2. Verify your GitHub token hasn't expired
3. Make sure you have at least one issue with the "user-story" label
4. Check the console output for specific error messages

## Example Repository Structure

```
your-repository/
├── .github/
│   └── ISSUE_TEMPLATE/
│       └── user_story.md (optional template)
├── user-stories/ (optional, for JSON files)
│   ├── authentication/
│   │   └── login.json
│   └── dashboard/
│       └── view-stats.json
└── README.md
```

## Summary

✅ **Required for GitHub Issues mode:**
1. GitHub Personal Access Token with `repo` scope
2. `appsettings.json` with correct token, owner, and repository
3. At least one issue with "user-story" label
4. Issue title containing "[USER STORY]"

✅ **Required for JSON files mode:**
1. GitHub Personal Access Token with `repo` scope
2. `appsettings.json` with correct token, owner, and repository
3. `user-stories/` directory with JSON files in your repository
