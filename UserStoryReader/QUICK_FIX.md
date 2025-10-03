# Quick Fix: Enable GitHub Issues Mode

## Problem
- ✅ Option 2 (JSON files) works
- ❌ Option 1 (GitHub Issues) shows "Found 0 user story issues"

## Root Cause
The issues in your repository don't have the "user-story" label AND don't have "[USER STORY]" in the title.

## Solution (Choose One)

### Option A: Add "[USER STORY]" to Issue Titles (Easiest)

1. Go to your repository issues: https://github.com/krupanidhi/user-stories-test/issues
2. Edit each issue you want to appear as a user story
3. Add `[USER STORY]` to the beginning of the title
   - Before: `Implement login feature`
   - After: `[USER STORY] Implement login feature`
4. Save the issue

**That's it!** The app will now pick up these issues.

### Option B: Add "user-story" Label to Issues

1. Go to your repository: https://github.com/krupanidhi/user-stories-test
2. Click "Issues" → "Labels"
3. If "user-story" label doesn't exist:
   - Click "New label"
   - Name: `user-story`
   - Click "Create label"
4. Go back to Issues
5. Open each issue you want to appear
6. Add the "user-story" label
7. Save

### Option C: Do Both (Best Practice)

For best organization:
1. Create the "user-story" label
2. Add it to relevant issues
3. Also add "[USER STORY]" to titles for clarity

## After the Fix

Run the app again:
```bash
dotnet run
```

Select option **1** (GitHub Issues)

You should now see:
```
Retrieved X total issues
Found Y user story issues  ← This should be > 0 now!
```

## Why This Happened

The previous code required BOTH:
- Label: "user-story" OR "enhancement" **AND**
- Title: "[USER STORY]"

The new code requires EITHER:
- Label: "user-story" OR "enhancement" **OR**
- Title: "[USER STORY]"

This is more flexible and easier to use!

## Quick Test

1. Edit one issue in GitHub
2. Add `[USER STORY]` to the title
3. Run `dotnet run` and select option 1
4. You should see that issue appear

## Still Not Working?

Check:
- Are you pointing to the right repository in `appsettings.json`?
- Do the issues exist in that repository?
- Try option 2 (JSON files) to verify your connection works
