# Publishing Guide

This guide explains how to build and publish the WinInputSimulator NuGet package.

## Prerequisites

1. **NuGet Account**
   - Create an account at https://www.nuget.org/
   - Generate an API key from your account settings
   - Store the API key securely (never commit it to Git!)

2. **.NET SDK**
   - Install .NET 8.0, 9.0, or 10.0 SDK
   - Verify: `dotnet --list-sdks`

## Local Package Build

### Option 1: Using PowerShell Script (Recommended)

```powershell
# Build the package
.\build-package.ps1

# This will create the package in .\nupkg\ directory
```

### Option 2: Manual Build

```bash
# Clean
dotnet clean -c Release

# Restore
dotnet restore

# Build
dotnet build -c Release

# Pack
dotnet pack -c Release --no-build -o ./nupkg
```

## Test Package Locally

Before publishing, test your package in a local test project:

```powershell
# Add local source
dotnet nuget add source ./nupkg --name LocalWinInputSimulator

# Create test project
mkdir TestProject
cd TestProject
dotnet new console

# Add package from local source
dotnet add package WinInputSimulator --source LocalWinInputSimulator

# Test the package
# Write some code using InputSimulator
dotnet run
```

## Publishing to NuGet.org

### Manual Publishing

```bash
# Publish to NuGet.org
dotnet nuget push ./nupkg/WinInputSimulator.*.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

### Automated Publishing with GitHub Actions

The repository is configured with GitHub Actions for automated publishing.

#### Setup

1. **Add NuGet API Key to GitHub Secrets**
   - Go to repository Settings ¡ú Secrets and variables ¡ú Actions
   - Click "New repository secret"
   - Name: `NUGET_API_KEY`
   - Value: Your NuGet API key
   - Click "Add secret"

2. **Create a Version Tag**
   ```bash
   # Make sure all changes are committed
   git add .
   git commit -m "Release v1.0.0"
   
   # Create and push tag
   git tag v1.0.0
   git push origin v1.0.0
   ```

3. **Monitor the Workflow**
   - Go to Actions tab in GitHub
   - Watch the "Publish NuGet Package" workflow
   - Package will be automatically published to NuGet.org

#### Manual Workflow Trigger

You can also manually trigger the workflow:

1. Go to Actions tab
2. Select "Publish NuGet Package"
3. Click "Run workflow"
4. Select branch and click "Run workflow"

## Version Management

Follow [Semantic Versioning](https://semver.org/):

- **MAJOR.MINOR.PATCH** (e.g., 1.0.0)
  - **MAJOR**: Breaking changes
  - **MINOR**: New features (backward compatible)
  - **PATCH**: Bug fixes (backward compatible)

### Updating Version

1. **Update `.csproj` file**
   ```xml
   <Version>1.0.1</Version>
   ```

2. **Update CHANGELOG.md**
   ```markdown
   ## [1.0.1] - 2025-01-XX
   ### Fixed
   - Bug fix description
   ```

3. **Commit and tag**
   ```bash
   git add .
   git commit -m "Release v1.0.1"
   git tag v1.0.1
   git push origin main
   git push origin v1.0.1
   ```

## Package Validation

Before publishing, validate your package:

```bash
# Install validation tool
dotnet tool install -g dotnet-validate

# Validate package
dotnet validate package ./nupkg/WinInputSimulator.*.nupkg
```

## Post-Publication Checklist

After publishing:

- [ ] Verify package appears on NuGet.org
- [ ] Test installation in a fresh project
- [ ] Check that README displays correctly
- [ ] Verify all target frameworks work
- [ ] Update GitHub release notes
- [ ] Announce on relevant channels

## Troubleshooting

### Package not appearing on NuGet.org

- Wait 10-15 minutes for indexing
- Check for validation errors in email
- Verify API key permissions

### Build errors

```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore with verbose output
dotnet restore -v detailed
```

### Multi-targeting issues

```bash
# List installed SDKs
dotnet --list-sdks

# Install missing SDK versions
# Download from: https://dotnet.microsoft.com/download
```

## Best Practices

1. **Test thoroughly before publishing**
   - Test on multiple .NET versions
   - Test on different Windows versions
   - Verify all public APIs work

2. **Version carefully**
   - Never delete published versions
   - Follow semantic versioning strictly
   - Document breaking changes

3. **Document well**
   - Keep README up to date
   - Maintain CHANGELOG
   - Include XML documentation

4. **Security**
   - Never commit API keys
   - Use GitHub Secrets for CI/CD
   - Review dependencies regularly

## Support

- **Issues**: https://github.com/yanxiaodi/WinInputSimulator/issues
- **NuGet Package**: https://www.nuget.org/packages/WinInputSimulator
- **NuGet Docs**: https://docs.microsoft.com/nuget/
