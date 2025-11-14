# CI/CD Guide

## Overview

This project uses GitHub Actions for continuous integration and deployment with comprehensive test automation.

## Workflows

### 1. Main CI/CD Pipeline (`ci-cd.yml`)
**Triggers:** Push to main/develop, Pull Requests, Releases

**Features:**
- Builds and tests on .NET 8 and .NET 10
- Code quality analysis (SonarCloud)
- Security scanning (CodeQL)
- NuGet package creation
- Automatic publishing on releases

### 2. Pull Request Validation (`pr-validation.yml`)
**Triggers:** Pull Requests

**Features:**
- Quick validation
- Code formatting checks
- Test execution
- Automated comments

### 3. Scheduled Tests (`scheduled-tests.yml`)
**Triggers:** Daily at 2 AM UTC, Manual

**Features:**
- Comprehensive testing
- Multi-version compatibility
- Automated issue creation on failure

## Local Development

### Prerequisites
```bash
# Install .NET 8 (LTS) and .NET 10
dotnet --list-sdks
# Should show: 8.0.xxx and 10.0.xxx
```

### Run Tests
```bash
# PowerShell (recommended)
.\test.ps1

# With coverage
.\test.ps1 -Coverage

# Watch mode
.\test.ps1 -Watch

# Batch file
.\test.cmd

# .NET CLI
dotnet test
```

## GitHub Secrets

Configure these in: `Settings ¡ú Secrets and variables ¡ú Actions`

| Secret | Required For | How to Get |
|--------|-------------|------------|
| `NUGET_API_KEY` | Publishing releases | https://www.nuget.org/account/apikeys |
| `CODECOV_TOKEN` | Coverage reports (optional) | https://codecov.io |
| `SONAR_TOKEN` | Code quality (optional) | https://sonarcloud.io |

## Publishing Releases

1. **Create a release on GitHub**
   - Tag: `v1.0.0` (semantic versioning)
   - The CI/CD pipeline automatically:
     - Builds the library
     - Runs all tests
     - Creates NuGet package
     - Publishes to NuGet.org

2. **Manual publish** (if needed):
   ```bash
   dotnet pack src/WinInputSimulator/WinInputSimulator/WinInputSimulator.csproj -c Release
   dotnet nuget push ./artifacts/*.nupkg --api-key YOUR_KEY --source https://api.nuget.org/v3/index.json
   ```

## Status Badges

Add to your README:
```markdown
[![CI/CD](https://github.com/yanxiaodi/WinInputSimulator/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/yanxiaodi/WinInputSimulator/actions/workflows/ci-cd.yml)
[![Tests](https://github.com/yanxiaodi/WinInputSimulator/actions/workflows/pr-validation.yml/badge.svg)](https://github.com/yanxiaodi/WinInputSimulator/actions/workflows/pr-validation.yml)
```

## Troubleshooting

### Tests Failing Due to DPI Scaling
Mouse position tests include tolerance for DPI scaling. If issues persist, run tests at 100% display scaling.

### .NET SDK Not Found
Ensure both .NET 8 and .NET 10 SDKs are installed:
```bash
dotnet --list-sdks
```

### Workflow Not Triggering
1. Check branch name matches workflow triggers
2. Validate YAML syntax
3. Check if workflow is disabled in Actions tab

### NuGet Push Fails
1. Verify `NUGET_API_KEY` secret is set
2. Check API key has "Push" permission
3. Ensure package ID doesn't conflict

## Project Structure

| Project | Target Framework | Purpose |
|---------|-----------------|---------|
| WinInputSimulator | .NET 8 (LTS) | Main library |
| WinInputSimulator.Tests | .NET 8 (LTS) | Unit tests (83 tests) |
| WinInputSimulator.Demo | .NET 10 | WPF Demo app |

## Workflow Strategy

```
Feature Branch ¡ú Pull Request ¡ú PR Validation (pr-validation.yml)
                       ¡ý
                    Merge
                       ¡ý
                Main/Develop ¡ú Full CI/CD (ci-cd.yml)
                       ¡ý
                   Release ¡ú Build + Publish (ci-cd.yml)
               
         Nightly ¡ú Comprehensive Tests (scheduled-tests.yml)
```

## Additional Resources

- [GitHub Actions Docs](https://docs.github.com/actions)
- [.NET CLI Reference](https://docs.microsoft.com/dotnet/core/tools/)
- [xUnit Documentation](https://xunit.net/)
- [Test Documentation](tests/WinInputSimulator.Tests/README.md)