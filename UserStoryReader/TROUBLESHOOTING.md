# UserStoryReader - Quick Troubleshooting Guide

## Error: "Found 0 user story issues"

### Symptoms
```
Successfully connected to repository: username/repo-name
Retrieved X total issues
Found 0 user story issues
No user stories found.
```

### Root Cause
Your repository doesn't have any issues that match BOTH requirements:
1. Has label: "user-story" OR "enhancement"
2. Title contains: "[USER STORY]"

### Quick Fix
**Option 1: Add the label to existing issues**
1. Go to your GitHub repository → Issues
2. Open an existing issue
3. Click "Labels" on the right sidebar
4. If "user-story" label doesn't exist, create it:
   - Go to Issues → Labels → New label
   - Name: `user-story`
   - Click "Create label"
5. Add "user-story" label to the issue
6. Make sure the issue title contains "[USER STORY]"

**Option 2: Create a new user story issue**
1. Go to your repository → Issues → New issue
2. Title: `[USER STORY] Your story title here`
3. Add label: "user-story"
4. Add body content (see template below)
5. Submit the issue

### User Story Issue Template
```markdown
## User Story
**As a** [role]
**I want** [feature]
**So that** [benefit]

## Acceptance Criteria
- [ ] Criterion 1
- [ ] Criterion 2

## Story Points
**Estimate:** 5

## Priority
**Priority:** Medium
```

---

## Error: "GitHub token not found"

### Symptoms
```
GitHub token not found. Please set it in appsettings.json or as environment variable GITHUB_TOKEN
```

### Quick Fix
1. Create `appsettings.json` from example:
   ```bash
   copy appsettings.example.json appsettings.json
   ```

2. Add your GitHub token:
   ```json
   {
     "GitHub": {
       "Token": "ghp_YOUR_TOKEN_HERE",
       "Owner": "your-username",
       "Repository": "your-repo"
     }
   }
   ```

3. Get a token: https://github.com/settings/tokens/new
   - Scope needed: `repo`

---

## Error: "Failed to connect to repository"

### Symptoms
```
Failed to connect to repository: Not Found
```

### Possible Causes & Fixes

**1. Wrong repository name**
- Check `appsettings.json` → Repository name is correct
- Check `appsettings.json` → Owner name is correct

**2. Repository is private and token lacks permissions**
- Go to https://github.com/settings/tokens
- Find your token → Edit
- Make sure `repo` scope is checked
- Regenerate token if needed

**3. Token expired or revoked**
- Create a new token: https://github.com/settings/tokens/new
- Update `appsettings.json` with new token

---

## Error: "Unauthorized" or "Bad credentials"

### Symptoms
```
Failed to connect to repository: Unauthorized
```

### Quick Fix
1. Your token is invalid or expired
2. Create a new token: https://github.com/settings/tokens/new
3. Required scope: `repo` (Full control of private repositories)
4. Update `appsettings.json` with the new token

---

## Error: "Rate limit exceeded"

### Symptoms
```
API rate limit exceeded for user
```

### Quick Fix
1. Wait 1 hour (GitHub API resets hourly)
2. Or use authenticated requests (make sure token is set)
3. Authenticated limit: 5,000 requests/hour
4. Unauthenticated limit: 60 requests/hour

---

## No Issues Showing Up (But They Exist)

### Checklist
✅ Issue has "user-story" OR "enhancement" label
✅ Issue title contains "[USER STORY]" (case-insensitive)
✅ Issue is in the correct repository
✅ Your token has access to the repository

### Debug Steps
1. Check what labels your issues have:
   - The app will show: "Available labels in repository: ..."
2. Add missing labels to your issues
3. Update issue titles to include "[USER STORY]"

---

## Working for You But Not for Others

### Common Scenario
- App works on your machine
- Other users get "Found 0 user story issues"

### Root Cause
Other users are pointing to a different repository that doesn't have properly labeled issues.

### Fix for Other Users
1. They need their own `appsettings.json` with:
   - Their own GitHub token
   - Correct Owner/Repository names
2. The target repository must have issues with "user-story" label
3. Share the SETUP_GUIDE.md with them

---

## Quick Verification Test

Run this checklist to verify everything is set up correctly:

```
□ appsettings.json exists (not appsettings.example.json)
□ GitHub token is valid (not expired)
□ Token has 'repo' scope
□ Owner name matches your GitHub username
□ Repository name is spelled correctly
□ Repository has at least 1 issue
□ Issue has "user-story" label
□ Issue title contains "[USER STORY]"
```

---

## Still Having Issues?

1. **Check the console output carefully** - it shows exactly what's happening
2. **Verify on GitHub web interface** - can you see the issues there?
3. **Test with a simple repository** - create a test repo with one labeled issue
4. **Check token permissions** - regenerate with full `repo` scope

## Need More Help?

See the full [SETUP_GUIDE.md](SETUP_GUIDE.md) for detailed instructions.
