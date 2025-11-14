using WinInputSimulator;

namespace WinInputSimulator.Tests;

/// <summary>
/// Unit tests for InputSimulator class
/// Note: Many tests are integration-style as they interact with Win32 API
/// Run these tests on a Windows machine with UI access
/// </summary>
public class InputSimulatorTests
{
    private readonly InputSimulator _simulator;

    public InputSimulatorTests()
    {
        _simulator = new InputSimulator();
    }

    [Fact]
    public void Constructor_ShouldCreateInstance()
    {
        // Arrange & Act
        var simulator = new InputSimulator();

        // Assert
        Assert.NotNull(simulator);
    }

    [Fact]
    public void GetMousePosition_ShouldReturnValidCoordinates()
    {
        // Act
        var (x, y) = _simulator.GetMousePosition();

        // Assert
        Assert.True(x >= 0, "X coordinate should be non-negative");
        Assert.True(y >= 0, "Y coordinate should be non-negative");
    }

    [Fact]
    public void MoveMouse_ShouldUpdateMousePosition()
    {
        // Arrange
        var targetX = 500;
        var targetY = 500;

        // Act
        _simulator.MoveMouse(targetX, targetY);
        Thread.Sleep(100); // Allow time for cursor to move
        var (actualX, actualY) = _simulator.GetMousePosition();

        // Assert - Allow for DPI scaling differences (up to 20% variance)
        var tolerance = 100; // 20% of 500
        Assert.True(Math.Abs(actualX - targetX) <= tolerance, 
            $"X position should be close to {targetX}, but was {actualX}. This may be due to DPI scaling.");
        Assert.True(Math.Abs(actualY - targetY) <= tolerance, 
            $"Y position should be close to {targetY}, but was {actualY}. This may be due to DPI scaling.");
    }

    [Fact]
    public void GetActiveWindow_ShouldReturnValidHandle()
    {
        // Act
        var handle = _simulator.GetActiveWindow();

        // Assert
        Assert.NotEqual(IntPtr.Zero, handle);
    }

    [Fact]
    public void FindWindowByProcessName_WithInvalidProcess_ShouldReturnZero()
    {
        // Arrange
        var invalidProcessName = "NonExistentProcess_" + Guid.NewGuid();

        // Act
        var handle = _simulator.FindWindowByProcessName(invalidProcessName);

        // Assert
        Assert.Equal(IntPtr.Zero, handle);
    }

    [Fact]
    public void FindWindowByTitle_WithInvalidTitle_ShouldReturnZero()
    {
        // Arrange
        var invalidTitle = "NonExistentWindowTitle_" + Guid.NewGuid();

        // Act
        var handle = _simulator.FindWindowByTitle(invalidTitle);

        // Assert
        Assert.Equal(IntPtr.Zero, handle);
    }

    [Fact]
    public void ActivateWindow_WithZeroHandle_ShouldReturnFalse()
    {
        // Arrange
        var zeroHandle = IntPtr.Zero;

        // Act
        var result = _simulator.ActivateWindow(zeroHandle);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void InputText_WithNullOrEmpty_ShouldNotThrow(string? text)
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.InputText(text!, 10));
        Assert.Null(exception);
    }

    [Fact]
    public void InputText_WithSimpleText_ShouldNotThrow()
    {
        // Arrange
        var text = "Hello World";

        // Act & Assert
        var exception = Record.Exception(() => _simulator.InputText(text, 10));
        Assert.Null(exception);
    }

    [Fact]
    public void InputText_WithUnicodeCharacters_ShouldNotThrow()
    {
        // Arrange
        var text = "Hello ÊÀ½ç ??";

        // Act & Assert
        var exception = Record.Exception(() => _simulator.InputText(text, 10));
        Assert.Null(exception);
    }

    [Fact]
    public void InputText_WithSpecialCharacters_ShouldNotThrow()
    {
        // Arrange
        var text = "Test\nNew Line\tTab\rReturn";

        // Act & Assert
        var exception = Record.Exception(() => _simulator.InputText(text, 10));
        Assert.Null(exception);
    }

    [Fact]
    public void InputCharacter_WithRegularChar_ShouldNotThrow()
    {
        // Arrange
        var character = 'A';

        // Act & Assert
        var exception = Record.Exception(() => _simulator.InputCharacter(character));
        Assert.Null(exception);
    }

    [Theory]
    [InlineData('\n')]
    [InlineData('\r')]
    [InlineData('\t')]
    [InlineData('\b')]
    public void InputCharacter_WithControlCharacters_ShouldNotThrow(char character)
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.InputCharacter(character));
        Assert.Null(exception);
    }

    [Fact]
    public void PressEnter_ShouldNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.PressEnter());
        Assert.Null(exception);
    }

    [Fact]
    public void PressTab_ShouldNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.PressTab());
        Assert.Null(exception);
    }

    [Fact]
    public void PressBackspace_ShouldNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.PressBackspace());
        Assert.Null(exception);
    }

    [Fact]
    public void PressDelete_ShouldNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.PressDelete());
        Assert.Null(exception);
    }

    [Fact]
    public void KeyPress_WithValidVirtualKey_ShouldNotThrow()
    {
        // Arrange
        var virtualKey = InputSimulator.VirtualKey.A;

        // Act & Assert
        var exception = Record.Exception(() => _simulator.KeyPress(virtualKey));
        Assert.Null(exception);
    }

    [Fact]
    public void CtrlKey_WithValidKey_ShouldNotThrow()
    {
        // Arrange
        var key = InputSimulator.VirtualKey.C;

        // Act & Assert
        var exception = Record.Exception(() => _simulator.CtrlKey(key));
        Assert.Null(exception);
    }

    [Fact]
    public void AltKey_WithValidKey_ShouldNotThrow()
    {
        // Arrange
        var key = InputSimulator.VirtualKey.F4;

        // Act & Assert
        var exception = Record.Exception(() => _simulator.AltKey(key));
        Assert.Null(exception);
    }

    [Fact]
    public void ShiftKey_WithValidKey_ShouldNotThrow()
    {
        // Arrange
        var key = InputSimulator.VirtualKey.A;

        // Act & Assert
        var exception = Record.Exception(() => _simulator.ShiftKey(key));
        Assert.Null(exception);
    }

    [Fact]
    public void InputCombination_WithMultipleKeys_ShouldNotThrow()
    {
        // Arrange
        byte[] keys = { InputSimulator.VirtualKey.Control, InputSimulator.VirtualKey.Shift, InputSimulator.VirtualKey.A };

        // Act & Assert
        var exception = Record.Exception(() => _simulator.InputCombination(keys));
        Assert.Null(exception);
    }

    [Fact]
    public void LeftClick_ShouldNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.LeftClick());
        Assert.Null(exception);
    }

    [Fact]
    public void LeftDoubleClick_ShouldNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.LeftDoubleClick());
        Assert.Null(exception);
    }

    [Fact]
    public void RightClick_ShouldNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.RightClick());
        Assert.Null(exception);
    }

    [Fact]
    public void MiddleClick_ShouldNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.MiddleClick());
        Assert.Null(exception);
    }

    [Fact]
    public void LeftMouseDown_ShouldNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.LeftMouseDown());
        Assert.Null(exception);
        
        // Cleanup - release the button
        _simulator.LeftMouseUp();
    }

    [Fact]
    public void RightMouseDown_ShouldNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.RightMouseDown());
        Assert.Null(exception);
        
        // Cleanup - release the button
        _simulator.RightMouseUp();
    }

    [Fact]
    public void MiddleMouseDown_ShouldNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.MiddleMouseDown());
        Assert.Null(exception);
        
        // Cleanup - release the button
        _simulator.MiddleMouseUp();
    }

    [Theory]
    [InlineData(120)]  // Scroll up
    [InlineData(-120)] // Scroll down
    public void MouseWheel_WithValidDelta_ShouldNotThrow(int delta)
    {
        // Act & Assert
        var exception = Record.Exception(() => _simulator.MouseWheel(delta));
        Assert.Null(exception);
    }

    [Fact]
    public void MoveMouseRelative_ShouldNotThrow()
    {
        // Arrange
        var dx = 10;
        var dy = 10;

        // Act & Assert
        var exception = Record.Exception(() => _simulator.MoveMouseRelative(dx, dy));
        Assert.Null(exception);
    }

    [Fact]
    public void ClickAt_ShouldMoveMouseAndClick()
    {
        // Arrange
        var targetX = 600;
        var targetY = 600;

        // Act
        _simulator.ClickAt(targetX, targetY);
        Thread.Sleep(150); // Allow time for the operation to complete
        var (actualX, actualY) = _simulator.GetMousePosition();

        // Assert - Allow for DPI scaling differences (up to 20% variance)
        var tolerance = 120; // 20% of 600
        Assert.True(Math.Abs(actualX - targetX) <= tolerance, 
            $"X position should be close to {targetX}, but was {actualX}. This may be due to DPI scaling.");
        Assert.True(Math.Abs(actualY - targetY) <= tolerance, 
            $"Y position should be close to {targetY}, but was {actualY}. This may be due to DPI scaling.");
    }
}
