# Shared Setup for Team Members

## Quick Setup (Using Shared Configuration)

If you're part of the team and want to use the same repository as everyone else, follow these steps:

### Step 1: Get the Shared Configuration

Ask your team lead for the `appsettings.json` file with the shared GitHub token, or create it with these values:

```json
{
  "GitHub": {
    "Token": "ASK_YOUR_TEAM_LEAD_FOR_TOKEN",
    "Owner": "krupanidhi",
    "Repository": "user-stories-test"
  }
}
```

### Step 2: Copy the Configuration

1. Navigate to the project directory:
   ```bash
   cd C:\Users\[YourUsername]\CascadeProjects\UserStoryReader\UserStoryReader
   ```

2. Create `appsettings.json` with the shared configuration above

### Step 3: Verify the Repository Has User Stories

Before running the app, make sure the repository has issues with the "user-story" label:

1. Go to: https://github.com/krupanidhi/user-stories-test/issues
2. Check if there are issues with the "user-story" label
3. If not, see "Adding User Stories" section below

### Step 4: Run the Application

```bash
dotnet run
```

Select option **1** (GitHub Issues) when prompted.

---

## Adding User Stories to the Shared Repository

If the repository doesn't have any user story issues yet, someone needs to create them:

### Creating the "user-story" Label (One-time setup)

1. Go to: https://github.com/krupanidhi/user-stories-test/labels
2. Click "New label"
3. Name: `user-story`
4. Color: `#0075ca` (or any color you prefer)
5. Click "Create label"

### Creating a User Story Issue

1. Go to: https://github.com/krupanidhi/user-stories-test/issues/new
2. **Title:** `[USER STORY] Your story title here`
3. **Labels:** Add "user-story" label
4. **Body:** Use this template:

```markdown
## User Story
**As a** [role]
**I want** [feature]
**So that** [benefit]

## Description
Detailed description of the feature...

## Acceptance Criteria
- [ ] Criterion 1
- [ ] Criterion 2
- [ ] Criterion 3

## Story Points
**Estimate:** 5

## Priority
**Priority:** Medium

## Additional Notes
Epic: Feature Name
Tags: tag1, tag2
```

5. Click "Submit new issue"

---

## Why This Approach is Better

### ✅ Advantages of Shared Configuration
- Everyone sees the same user stories
- Single source of truth
- No need for individual tokens
- Easier team collaboration
- Consistent data across team

### ⚠️ Important Notes
- The shared token should have **read-only** access if possible
- Don't commit `appsettings.json` to git (it's in `.gitignore`)
- Share the token securely (not via email/chat)
- Consider using environment variables in production

---

## Troubleshooting

### "Found 0 user story issues" Error

**This means the repository doesn't have properly labeled issues yet.**

**Solution:**
1. Go to https://github.com/krupanidhi/user-stories-test/issues
2. Check if any issues exist
3. If issues exist, add the "user-story" label to them
4. If no issues exist, create new ones following the template above
5. Make sure issue titles contain "[USER STORY]"

### "Failed to connect to repository" Error

**This means the token is invalid or the repository name is wrong.**

**Solution:**
1. Verify the token is correct in `appsettings.json`
2. Check that Owner is "krupanidhi"
3. Check that Repository is "user-stories-test"
4. Ask team lead for a fresh token if needed

---

## Alternative: Use Your Own Token

If you prefer to use your own GitHub token instead of a shared one:

1. Create your own token: https://github.com/settings/tokens/new
   - Name: `UserStoryReader`
   - Scope: `repo` (read access)
   - Generate token

2. Update `appsettings.json`:
   ```json
   {
     "GitHub": {
       "Token": "YOUR_OWN_TOKEN_HERE",
       "Owner": "krupanidhi",
       "Repository": "user-stories-test"
     }
   }
   ```

**Note:** You still need access to the `krupanidhi/user-stories-test` repository.

---

## Summary

**For team members:**
1. Get the shared `appsettings.json` from team lead
2. Copy it to your local project directory
3. Run `dotnet run`
4. If you get "Found 0 user story issues", the repository needs issues with the "user-story" label

**For team lead:**
1. Create user story issues in the repository with proper labels
2. Share the `appsettings.json` with team members securely
3. Make sure everyone has access to the repository

---

## Quick Test

To verify everything is working:

```bash
cd C:\Users\[YourUsername]\CascadeProjects\UserStoryReader\UserStoryReader
dotnet run
```

You should see:
```
Successfully connected to repository: krupanidhi/user-stories-test
Retrieved X total issues
Found Y user story issues
```

If Y is 0, the repository needs issues with the "user-story" label!
