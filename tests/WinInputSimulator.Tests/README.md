# WinInputSimulator.Tests

Unit tests for the WinInputSimulator library.

## Overview

This test project contains comprehensive unit tests for the WinInputSimulator library, covering:

- **InputSimulator Tests**: Core functionality including keyboard input, mouse operations, and text input
- **VirtualKey Tests**: Validation of all virtual key constants
- **Window Management Tests**: Window finding and activation functionality

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

## Test Coverage

The test suite includes:
- **83 unit tests** covering all public methods
- Input validation tests
- Error handling tests
- Integration tests with Win32 APIs

## Contributing

When adding new features to WinInputSimulator:
1. Add corresponding unit tests
2. Ensure all existing tests pass
3. Update this README if adding new test categories

## Dependencies

- xUnit 2.9.2
- Microsoft.NET.Test.Sdk 17.11.1
- coverlet.collector 6.0.2 (for code coverage)

## License

MIT License - Same as the main WinInputSimulator library
