# Fix: GitHub Push Protection Blocked Your Push

## What Happened
GitHub detected a Personal Access Token in your `appsettings.json` file and blocked the push to protect your security.

## Quick Fix Steps

### Step 1: Remove appsettings.json from Git History

Open PowerShell/Terminal in the project directory and run:

```powershell
# Remove the file from Git tracking (but keep it locally)
git rm --cached appsettings.json

# Commit the removal
git commit -m "Remove appsettings.json from version control"
```

### Step 2: Verify .gitignore is Updated

The `.gitignore` file has been updated to include:
```
# Configuration files with secrets
appsettings.json
appsettings.*.json
!appsettings.example.json
!appsettings.shared.json
```

### Step 3: Push Again

```powershell
git push origin dev/OM/feature/1.0.0
```

### Step 4: Revoke the Exposed Token (IMPORTANT!)

Since the token was in a commit, you should revoke it and create a new one:

1. Go to: https://github.com/settings/tokens
2. Find the token that was exposed
3. Click "Delete" or "Revoke"
4. Create a new token: https://github.com/settings/tokens/new
   - Name: `UserStoryReader`
   - Scope: `repo`
   - Generate token
5. Update your local `appsettings.json` with the new token

## Alternative: Allow the Secret (Not Recommended)

GitHub provided a link to allow the secret:
```
https://github.com/krupanidhi/UserStoryReader/security/secret-scanning/unblock-secret/33W3sCwybglB916QmWgAzUwdZa8
```

**⚠️ This is NOT recommended!** It's better to remove the secret from version control.

## Prevention for Future

### What Should Be Committed:
✅ `appsettings.example.json` - Template without real secrets
✅ `appsettings.shared.json` - Template for team sharing
✅ `.gitignore` - With appsettings.json excluded

### What Should NOT Be Committed:
❌ `appsettings.json` - Contains real tokens
❌ Any file with actual secrets/tokens
❌ Personal access tokens in code

### Best Practices:

1. **Always use .gitignore** for sensitive files
2. **Use environment variables** for CI/CD:
   ```powershell
   $env:GITHUB_TOKEN = "your-token"
   ```
3. **Share tokens securely** (not via email/chat)
4. **Rotate tokens regularly**
5. **Use read-only tokens** when possible

## Complete Command Sequence

```powershell
# Navigate to project
cd C:\Users\KPeterson\CascadeProjects\UserStoryReader\UserStoryReader

# Remove from Git
git rm --cached appsettings.json

# Commit the change
git commit -m "Remove appsettings.json from version control"

# Push
git push origin dev/OM/feature/1.0.0

# Revoke old token and create new one at:
# https://github.com/settings/tokens
```

## If You Get "File Not Found" Error

If `git rm --cached appsettings.json` says file not found, it means it wasn't tracked yet. In that case:

```powershell
# Just commit the .gitignore change
git add .gitignore
git commit -m "Add appsettings.json to .gitignore"
git push origin dev/OM/feature/1.0.0
```

## Verification

After fixing, verify appsettings.json is ignored:

```powershell
# This should NOT show appsettings.json
git status

# This should show appsettings.json
git check-ignore -v appsettings.json
```

## Summary

1. ✅ `.gitignore` updated to exclude `appsettings.json`
2. ⏳ Remove `appsettings.json` from Git: `git rm --cached appsettings.json`
3. ⏳ Commit and push
4. ⏳ Revoke the exposed token
5. ⏳ Create a new token
6. ⏳ Update local `appsettings.json` with new token

This ensures your secrets stay secure! 🔒
