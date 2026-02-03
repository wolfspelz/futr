---
name: deploy
description: Rebase deployment branch onto main and push to trigger deployment
---

# Deploy

Rebase the `deployment` branch onto `main` and push to trigger the deployment pipeline.

## Steps

1. Fetch latest from origin
2. Checkout `deployment` branch
3. Rebase onto `main`
4. Force push with lease
5. Switch back to `main`

## Command

```bash
git fetch origin && git checkout deployment && git rebase main && git push --force-with-lease && git checkout main
```
