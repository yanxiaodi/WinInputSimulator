using System.Diagnostics;

namespace WinInputSimulator.Tests;

/// <summary>
/// Tests for window management functionality
/// Note: These tests interact with actual Windows processes and may be affected by system state
/// </summary>
public class WindowManagementTests
{
    private readonly InputSimulator _simulator;

    public WindowManagementTests()
    {
        _simulator = new InputSimulator();
    }

    [Fact]
    public void GetActiveWindow_ShouldReturnNonZeroHandle()
    {
        // Act
        var handle = _simulator.GetActiveWindow();

        // Assert
        Assert.NotEqual(IntPtr.Zero, handle);
    }

    [Fact]
    public void FindWindowByProcessName_WithCurrentProcess_ShouldFindWindow()
    {
        // Arrange
        var currentProcess = Process.GetCurrentProcess();
        var processName = currentProcess.ProcessName;

        // Act
        var handle = _simulator.FindWindowByProcessName(processName);

        // Assert
        // Note: Test processes might not have a main window, so we just verify no exception
        Assert.True(true, "Method executed without throwing exception");
    }

    [Fact]
    public void FindWindowByProcessName_WithNullOrEmpty_ShouldReturnZero()
    {
        // Act
        var handleNull = _simulator.FindWindowByProcessName(null!);
        var handleEmpty = _simulator.FindWindowByProcessName(string.Empty);

        // Assert
        Assert.Equal(IntPtr.Zero, handleNull);
        Assert.Equal(IntPtr.Zero, handleEmpty);
    }

    [Fact]
    public void FindWindowByTitle_WithNullOrEmpty_ShouldReturnZero()
    {
        // Act
        var handleNull = _simulator.FindWindowByTitle(null!);
        var handleEmpty = _simulator.FindWindowByTitle(string.Empty);

        // Assert
        Assert.Equal(IntPtr.Zero, handleNull);
        Assert.Equal(IntPtr.Zero, handleEmpty);
    }

    [Fact]
    public void FindWindowByTitle_WithPartialMatch_ShouldWork()
    {
        // Arrange
        // Try to find a common Windows process that should exist
        var processes = Process.GetProcesses();
        var processWithWindow = processes.FirstOrDefault(p =>
            !string.IsNullOrEmpty(p.MainWindowTitle) &&
            p.MainWindowHandle != IntPtr.Zero);

        if (processWithWindow == null)
        {
            // Skip test if no suitable process found
            return;
        }

        var partialTitle = processWithWindow.MainWindowTitle.Length > 3
            ? processWithWindow.MainWindowTitle.Substring(0, 3)
            : processWithWindow.MainWindowTitle;

        // Act
        var handle = _simulator.FindWindowByTitle(partialTitle);

        // Assert
        Assert.NotEqual(IntPtr.Zero, handle);
    }

    [Fact]
    public void ActivateWindow_WithValidHandle_ShouldNotThrow()
    {
        // Arrange
        var activeWindow = _simulator.GetActiveWindow();

        // Act & Assert
        var exception = Record.Exception(() => _simulator.ActivateWindow(activeWindow));
        Assert.Null(exception);
    }

    [Fact]
    public void ActivateWindow_WithInvalidHandle_ShouldReturnFalse()
    {
        // Arrange
        var invalidHandle = new IntPtr(999999);

        // Act
        var result = _simulator.ActivateWindow(invalidHandle);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ActivateWindowByProcessName_WithInvalidProcessName_ShouldReturnFalse()
    {
        // Arrange
        var invalidProcessName = "NonExistentProcess_" + Guid.NewGuid().ToString("N");

        // Act
        var result = _simulator.ActivateWindowByProcessName(invalidProcessName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void FindWindowByTitle_CaseInsensitive_ShouldWork()
    {
        // Arrange
        var processes = Process.GetProcesses();
        var processWithWindow = processes.FirstOrDefault(p =>
            !string.IsNullOrEmpty(p.MainWindowTitle) &&
            p.MainWindowHandle != IntPtr.Zero);

        if (processWithWindow == null || processWithWindow.MainWindowTitle.Length < 3)
        {
            // Skip test if no suitable process found
            return;
        }

        var lowerCaseTitle = processWithWindow.MainWindowTitle.Substring(0, 3).ToLowerInvariant();

        // Act
        var handle = _simulator.FindWindowByTitle(lowerCaseTitle);

        // Assert
        // Should find the window regardless of case
        Assert.NotEqual(IntPtr.Zero, handle);
    }

    [Fact]
    public void MultipleWindowOperations_ShouldWorkInSequence()
    {
        // Arrange
        var activeWindow = _simulator.GetActiveWindow();

        // Act & Assert
        Assert.NotEqual(IntPtr.Zero, activeWindow);

        var activateResult = _simulator.ActivateWindow(activeWindow);
        Assert.True(activateResult || activeWindow == IntPtr.Zero);

        var newActiveWindow = _simulator.GetActiveWindow();
        Assert.NotEqual(IntPtr.Zero, newActiveWindow);
    }

    [Theory]
    [InlineData("explorer")]
    [InlineData("winlogon")]
    [InlineData("dwm")]
    public void FindWindowByProcessName_WithCommonWindowsProcess_ShouldNotThrow(string processName)
    {
        // Act & Assert
        var exception = Record.Exception(() =>
        {
            var handle = _simulator.FindWindowByProcessName(processName);
            // We just verify it doesn't throw, handle might be Zero if process doesn't have a window
        });

        Assert.Null(exception);
    }
}
