# 🚀 CI/CD Pipeline Overview

This repository uses a GitHub Actions workflow to automate the build, test, and Docker image deployment process for the .NET 8 API.

## 🛎️ Workflow Triggers

- **On push** to the `main` branch (for changes in `CleanArchiStarterTemplate/**`)
- **Manual trigger** via the GitHub Actions UI (`workflow_dispatch`)

---

## 🏗️ Pipeline Jobs

### 1. 🏢 Build

- Checks out the repository code
- Sets up .NET 8 SDK
- Caches NuGet packages for faster builds
- Restores dependencies
- Builds the solution in Release mode
- Uploads build artifacts for use in later jobs

### 2. 🧪 Test

- Downloads build artifacts from the build job
- Sets up .NET 8 SDK
- Caches NuGet packages
- Restores dependencies
- Runs tests in Release mode without rebuilding

### 3. 🐳 Docker

- Checks out the repository code
- Sets a short SHA variable (first 7 characters of the commit hash)
- Dynamically sets Docker tags:
  - Always tags as `<branch>.<short-sha>`
  - Adds `latest` tag if on the `main` branch
- Logs in to Docker Hub using repository secrets
- Builds and pushes the Docker image with the computed tags

---

## 🏷️ Docker Tagging Strategy

- **On `main` branch:**
  - `starter-app:main.<short-sha>`
  - `starter-app:latest`
- **On other branches:**
  - `starter-app:<branch>.<short-sha>`

---

## 🔑 Required Secrets

- `DOCKER_USERNAME` — Docker Hub username
- `DOCKER_PASSWORD` — Docker Hub access token

---

## 🎁 Benefits

- **Artifact sharing** between jobs for efficiency
- **Multi-tag Docker push** for traceability and latest release
- **NuGet caching** for faster builds
- **Branch-aware tagging** for easy image management

---
