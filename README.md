# WinInputSimulator

[![NuGet](https://img.shields.io/nuget/v/WinInputSimulator.svg)](https://www.nuget.org/packages/WinInputSimulator/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A lightweight and easy-to-use .NET library for simulating keyboard and mouse input on Windows using the Win32 API. Perfect for automation, testing, and accessibility applications.

## ? Features

- ?? **Keyboard Simulation**
  - Unicode character input support
  - Virtual key press simulation
  - Key combinations (Ctrl, Alt, Shift)
  - Special keys (Enter, Tab, Backspace, Delete, Arrow keys, Function keys)
  
- ??? **Mouse Operations**
  - Move cursor to absolute or relative positions
  - Left, right, and middle button clicks
  - Double-click support
  - Mouse wheel scrolling
  - Click at specific coordinates

- ?? **Window Management**
  - Find windows by title or process name
  - Activate and bring windows to foreground
  - Get active window information

## ?? Installation

To install the `WinInputSimulator` library, you have a few different options depending on your development setup and preferences.

### Package Manager Console

If you're using Visual Studio, the easiest way to install the library is through the Package Manager Console. You can open the Package Manager Console from the Tools > NuGet Package Manager menu. Then, run the following command:

```powershell
Install-Package WinInputSimulator
```

### .NET CLI

If you prefer using the command line, you can use the .NET CLI to install the package. Open a command prompt or terminal window and navigate to your project directory. Then, run the following command:

```bash
dotnet add package WinInputSimulator
```

### Manual Installation

If you want to install the package manually, you can download the latest release from the [NuGet Gallery](https://www.nuget.org/packages/WinInputSimulator). You can choose to install either the `.nupkg` file or the `.zip` file containing the library DLLs. If you download the `.zip` file, you will need to extract it and reference the appropriate DLLs in your project.

#### PackageReference

For projects using PackageReference in their `.csproj` file, you can add the following line to the `ItemGroup` section:

```xml
<PackageReference Include="WinInputSimulator" Version="1.0.0" />
```

## ?? Quick Start

Getting started with `WinInputSimulator` is easy! Just follow these simple steps to simulate keyboard and mouse input in your .NET application.

### 1. Add Reference to Your Project

First, you need to add a reference to the `WinInputSimulator` library in your project. If you installed the package using the Package Manager Console or .NET CLI, this should be done automatically. Otherwise, you can add the reference manually by downloading the library DLLs and adding them to your project.

### 2. Import the Namespace

In your C# code file, import the `WinInputSimulator` namespace to access the library's functionality:

```csharp
using WinInputSimulator;
```

### 3. Create an Instance of the Simulator

To start simulating input, create an instance of the `InputSimulator` class:

```csharp
var simulator = new InputSimulator();
```

### 4. Simulate Keyboard Input

You can simulate keyboard input using methods like `InputText`, `KeyPress`, and `CtrlKey`. Here's an example that types "Hello, World!" and presses the Enter key:

```csharp
simulator.InputText("Hello, World!");
simulator.KeyPress(InputSimulator.VirtualKey.Enter);
```

For more advanced scenarios, you can simulate key combinations and special keys:

```csharp
// Key combinations
simulator.CtrlKey(InputSimulator.VirtualKey.C);  // Ctrl+C
simulator.CtrlKey(InputSimulator.VirtualKey.V);  // Ctrl+V
simulator.AltKey(InputSimulator.VirtualKey.F4);  // Alt+F4

// Special keys
simulator.PressEnter();
simulator.PressTab();
simulator.PressBackspace();
```

### 5. Simulate Mouse Operations

The library also allows you to simulate mouse movements and clicks. You can move the mouse to a specific position and perform clicks like this:

```csharp
// Move mouse to absolute position
simulator.MoveMouse(500, 300);

// Click at current position
simulator.LeftClick();
```

You can also perform double-clicks, right-clicks, and scroll the mouse wheel:

```csharp
// Double click
simulator.LeftDoubleClick();

// Scroll mouse wheel
simulator.MouseWheel(120);  // Scroll up
simulator.MouseWheel(-120); // Scroll down
```

### 6. Manage Windows

In addition to input simulation, the library provides methods for window management. You can find a window by its title or process name and bring it to the foreground:

```csharp
// Find window by process name
IntPtr hWnd = simulator.FindWindowByProcessName("notepad");

// Activate window
simulator.ActivateWindow(hWnd);
```

You can also get information about the currently active window:

```csharp
// Get currently active window
IntPtr activeWindow = simulator.GetActiveWindow();
```

### 7. Advanced Scenarios

For more complex automation tasks, you can combine keyboard, mouse, and window management features. Here's an example that opens Notepad, types some text, and saves the file:

```csharp
var simulator = new InputSimulator();

// Open Notepad
simulator.ActivateWindowByProcessName("notepad");
Thread.Sleep(500);

// Type some text
simulator.InputText("This is automated text input.");
simulator.PressEnter();
simulator.PressEnter();

// Select all and copy
simulator.CtrlKey(InputSimulator.VirtualKey.A);
Thread.Sleep(100);
simulator.CtrlKey(InputSimulator.VirtualKey.C);

// Save file
simulator.CtrlKey(InputSimulator.VirtualKey.S);
```

### 8. Custom Key Combinations

You can also simulate custom key combinations using the `InputCombination` method. For example, to simulate Ctrl+Shift+T:

```csharp
simulator.InputCombination(
    InputSimulator.VirtualKey.Control,
    InputSimulator.VirtualKey.Shift,
    InputSimulator.VirtualKey.T
);
```

## ?? Virtual Key Codes

The library provides a convenient `VirtualKey` static class with constants for all common keys. Here are some examples:

```csharp
// Modifier keys
InputSimulator.VirtualKey.Control
InputSimulator.VirtualKey.Shift
InputSimulator.VirtualKey.Alt

// Letter keys (A-Z)
InputSimulator.VirtualKey.A ... InputSimulator.VirtualKey.Z

// Number keys (0-9)
InputSimulator.VirtualKey.D0 ... InputSimulator.VirtualKey.D9

// Function keys (F1-F12)
InputSimulator.VirtualKey.F1 ... InputSimulator.VirtualKey.F12

// Arrow keys
InputSimulator.VirtualKey.Left
InputSimulator.VirtualKey.Right
InputSimulator.VirtualKey.Up
InputSimulator.VirtualKey.Down

// Other common keys
InputSimulator.VirtualKey.Enter
InputSimulator.VirtualKey.Tab
InputSimulator.VirtualKey.Escape
InputSimulator.VirtualKey.Space
InputSimulator.VirtualKey.Backspace
InputSimulator.VirtualKey.Delete
InputSimulator.VirtualKey.Home
InputSimulator.VirtualKey.End
InputSimulator.VirtualKey.PageUp
InputSimulator.VirtualKey.PageDown
InputSimulator.VirtualKey.Insert
```

## ?? Requirements

Before you start using `WinInputSimulator`, make sure your system meets the following requirements:

- **Operating System:** Windows 10 or later
- **.NET Version:** .NET 8.0, .NET 9.0, or .NET 10.0
- **Platform:** Windows x64, x86, or ARM64

## ?? Important Notes

1. **Administrator Privileges:** Some applications may require administrator privileges to receive simulated input, especially if they are running with elevated permissions.

2. **Focus:** The target application window should have focus to receive keyboard input. Use the window management methods to activate windows before sending input.

3. **Timing:** Add appropriate delays (`Thread.Sleep()`) between operations to ensure the target application has time to process the input.

4. **Security:** This library simulates real user input. Use it responsibly and only in appropriate contexts (testing, automation, accessibility).

5. **Thread Safety:** The library is not thread-safe. Create separate instances for multi-threaded scenarios or use proper synchronization.

## ?? API Reference

The following tables summarize the available methods in the `WinInputSimulator` library for keyboard, mouse, and window management.

### Keyboard Methods

| Method | Description |
|--------|-------------|
| `KeyPress(byte virtualKey)` | Press and release a key |
| `InputCharacter(char character)` | Input a single character with Unicode support |
| `InputText(string text, int delayMs = 50)` | Type a text string |
| `InputCombination(params byte[] keys)` | Simulate key combination |
| `CtrlKey(byte key)` | Ctrl + Key |
| `AltKey(byte key)` | Alt + Key |
| `ShiftKey(byte key)` | Shift + Key |
| `PressEnter()` | Press Enter key |
| `PressTab()` | Press Tab key |
| `PressBackspace()` | Press Backspace key |
| `PressDelete()` | Press Delete key |

### Mouse Methods

| Method | Description |
|--------|-------------|
| `GetMousePosition()` | Get current mouse position |
| `MoveMouse(int x, int y)` | Move mouse to absolute position |
| `MoveMouseRelative(int dx, int dy)` | Move mouse relatively |
| `LeftClick()` | Left mouse button click |
| `LeftDoubleClick()` | Left mouse button double click |
| `RightClick()` | Right mouse button click |
| `MiddleClick()` | Middle mouse button click |
| `ClickAt(int x, int y)` | Click at specific position |
| `MouseWheel(int delta)` | Scroll mouse wheel |
| `LeftMouseDown()` | Press left button down |
| `LeftMouseUp()` | Release left button |
| `RightMouseDown()` | Press right button down |
| `RightMouseUp()` | Release right button |
| `MiddleMouseDown()` | Press middle button down |
| `MiddleMouseUp()` | Release middle button |

### Window Management Methods

| Method | Description |
|--------|-------------|
| `FindWindowByTitle(string titleContains)` | Find window by title (partial match) |
| `FindWindowByProcessName(string processName)` | Find window by process name |
| `ActivateWindow(IntPtr hWnd)` | Activate window |
| `ActivateWindowByProcessName(string processName)` | Find and activate window |
| `GetActiveWindow()` | Get currently active window handle |

## ?? Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## ?? License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## ?? Acknowledgments

Built with ?? using the Windows Win32 API.

## ?? Support

- **Issues:** [GitHub Issues](https://github.com/yanxiaodi/WinInputSimulator/issues)
- **Discussions:** [GitHub Discussions](https://github.com/yanxiaodi/WinInputSimulator/discussions)

---

**Note:** This library is designed for legitimate automation and testing purposes. Please use it responsibly and in compliance with applicable laws and regulations.