# WinInputSimulator.Tests

Unit tests for the WinInputSimulator library.

## Overview

This test project contains comprehensive unit tests for the WinInputSimulator library, covering:

- **InputSimulator Tests**: Core functionality including keyboard input, mouse operations, and text input
- **VirtualKey Tests**: Validation of all virtual key constants
- **Window Management Tests**: Window finding and activation functionality

## CI/CD Integration

This project is integrated with GitHub Actions for continuous integration and deployment:

### .NET Version Support

The workspace includes projects targeting:
- **.NET 8 (LTS)**: Main library and test project
- **.NET 10 (stable)**: Demo application only

**CI/CD workflows automatically install both .NET 8 and .NET 10 SDKs** to support all projects.

### Workflows

1. **CI/CD Pipeline** (`.github/workflows/ci-cd.yml`)
   - Runs on push to main/develop branches and releases
   - Installs .NET 8 (LTS) and .NET 10 (stable) SDKs
   - Builds, tests, creates packages, and publishes to NuGet
   - Includes code quality analysis and security scanning

2. **Pull Request Validation** (`.github/workflows/pr-validation.yml`)
   - Validates all pull requests
   - Supports both .NET 8 and .NET 10
   - Runs tests, checks code formatting
   - Posts results as PR comments

3. **Scheduled Tests** (`.github/workflows/scheduled-tests.yml`)
   - Runs nightly at 2 AM UTC
   - Tests on both .NET 8 and .NET 10
   - Creates issues if tests fail
   - Compatibility matrix testing

### Badges

Add these to your main README.md:

```markdown
[![CI/CD](https://github.com/yanxiaodi/WinInputSimulator/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/yanxiaodi/WinInputSimulator/actions/workflows/ci-cd.yml)
[![Tests](https://github.com/yanxiaodi/WinInputSimulator/actions/workflows/pr-validation.yml/badge.svg)](https://github.com/yanxiaodi/WinInputSimulator/actions/workflows/pr-validation.yml)
[![NuGet](https://img.shields.io/nuget/v/WinInputSimulator.svg)](https://www.nuget.org/packages/WinInputSimulator/)
```

## Running the Tests

### Using .NET CLI

Run all tests:
```bash
dotnet test
```

Run tests with detailed output:
```bash
dotnet test --logger "console;verbosity=detailed"
```

Run with code coverage:
```bash
dotnet test --collect:"XPlat Code Coverage" --settings tests/test.runsettings
```

Run specific test class:
```bash
dotnet test --filter "FullyQualifiedName~InputSimulatorTests"
```

### Using Visual Studio

1. Open the solution in Visual Studio
2. Open Test Explorer (Test ¡ú Test Explorer)
3. Click "Run All" or select specific tests to run

### Using Visual Studio Code

1. Install the .NET Core Test Explorer extension
2. Tests will appear in the Testing sidebar
3. Click the play button to run tests

## Test Categories

### InputSimulatorTests
Tests for core input simulation functionality:
- Constructor and instance creation
- Mouse position retrieval and movement
- Mouse clicks (left, right, middle)
- Mouse wheel scrolling
- Keyboard key presses
- Key combinations (Ctrl, Alt, Shift)
- Text input with Unicode support
- Special characters and control keys

### VirtualKeyTests
Tests to verify all virtual key constants:
- Modifier keys (Shift, Control, Alt)
- Special keys (Enter, Tab, Escape, etc.)
- Letter keys (A-Z)
- Number keys (0-9)
- Function keys (F1-F12)
- Arrow keys
- Navigation keys (Home, End, PageUp, PageDown)

### WindowManagementTests
Tests for window management operations:
- Getting active window handle
- Finding windows by process name
- Finding windows by title (with partial matching)
- Window activation
- Case-insensitive title matching

## Important Notes

### Windows-Only Tests
These tests require a Windows environment as they interact with Win32 APIs. They will not run on Linux or macOS.

### DPI Scaling
Some tests involving mouse coordinates account for DPI scaling, which can cause coordinate differences on high-DPI displays. Tests include tolerance ranges to handle this.

### Interactive Tests
Many tests perform actual input simulation (moving mouse, pressing keys). When running tests:
- Avoid moving the mouse or pressing keys during test execution
- Some tests may briefly move your mouse cursor
- Tests include appropriate delays to ensure operations complete

### CI/CD Considerations
When running tests in CI/CD pipelines:
- Ensure the build agent has UI access (not headless)
- Tests may fail in environments without proper Windows UI support
- Consider using specialized Windows runners for automated testing

**GitHub Actions Configuration:**
- Uses `windows-latest` runners for UI access
- Includes test result uploads and coverage reporting
- Configured for both PR validation and release pipelines

## Test Coverage

The test suite includes:
- **82 unit tests** covering all public methods
- Input validation tests
- Error handling tests
- Integration tests with Win32 APIs

Test results are uploaded as artifacts in GitHub Actions for review.

## Test Configuration

The project uses `tests/test.runsettings` for:
- Code coverage configuration
- Test timeout settings
- xUnit runner configuration
- Logger settings

## Contributing

When adding new features to WinInputSimulator:
1. Add corresponding unit tests
2. Ensure all existing tests pass
3. Update this README if adding new test categories
4. All tests will run automatically in CI/CD pipelines

## Dependencies

- xUnit 2.9.2
- Microsoft.NET.Test.Sdk 17.11.1
- coverlet.collector 6.0.2 (for code coverage)

## License

MIT License - Same as the main WinInputSimulator library
