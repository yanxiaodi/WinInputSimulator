namespace WinInputSimulator.Tests;

/// <summary>
/// Tests for VirtualKey constants to ensure all keys are properly defined
/// </summary>
public class VirtualKeyTests
{
    [Theory]
    [InlineData(nameof(InputSimulator.VirtualKey.Shift), 0x10)]
    [InlineData(nameof(InputSimulator.VirtualKey.Control), 0x11)]
    [InlineData(nameof(InputSimulator.VirtualKey.Alt), 0x12)]
    [InlineData(nameof(InputSimulator.VirtualKey.Enter), 0x0D)]
    [InlineData(nameof(InputSimulator.VirtualKey.Tab), 0x09)]
    [InlineData(nameof(InputSimulator.VirtualKey.Escape), 0x1B)]
    [InlineData(nameof(InputSimulator.VirtualKey.Space), 0x20)]
    [InlineData(nameof(InputSimulator.VirtualKey.Backspace), 0x08)]
    [InlineData(nameof(InputSimulator.VirtualKey.Delete), 0x2E)]
    public void ModifierAndSpecialKeys_ShouldHaveCorrectValues(string keyName, int expectedValue)
    {
        // Act
        var actualValue = keyName switch
        {
            nameof(InputSimulator.VirtualKey.Shift) => InputSimulator.VirtualKey.Shift,
            nameof(InputSimulator.VirtualKey.Control) => InputSimulator.VirtualKey.Control,
            nameof(InputSimulator.VirtualKey.Alt) => InputSimulator.VirtualKey.Alt,
            nameof(InputSimulator.VirtualKey.Enter) => InputSimulator.VirtualKey.Enter,
            nameof(InputSimulator.VirtualKey.Tab) => InputSimulator.VirtualKey.Tab,
            nameof(InputSimulator.VirtualKey.Escape) => InputSimulator.VirtualKey.Escape,
            nameof(InputSimulator.VirtualKey.Space) => InputSimulator.VirtualKey.Space,
            nameof(InputSimulator.VirtualKey.Backspace) => InputSimulator.VirtualKey.Backspace,
            nameof(InputSimulator.VirtualKey.Delete) => InputSimulator.VirtualKey.Delete,
            _ => throw new ArgumentException($"Unknown key: {keyName}")
        };

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData('A', 0x41)]
    [InlineData('B', 0x42)]
    [InlineData('C', 0x43)]
    [InlineData('Z', 0x5A)]
    public void LetterKeys_ShouldHaveCorrectValues(char letter, int expectedValue)
    {
        // Act
        var actualValue = letter switch
        {
            'A' => InputSimulator.VirtualKey.A,
            'B' => InputSimulator.VirtualKey.B,
            'C' => InputSimulator.VirtualKey.C,
            'Z' => InputSimulator.VirtualKey.Z,
            _ => throw new ArgumentException($"Unknown letter: {letter}")
        };

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(0, 0x30)]
    [InlineData(1, 0x31)]
    [InlineData(5, 0x35)]
    [InlineData(9, 0x39)]
    public void NumberKeys_ShouldHaveCorrectValues(int number, int expectedValue)
    {
        // Act
        var actualValue = number switch
        {
            0 => InputSimulator.VirtualKey.D0,
            1 => InputSimulator.VirtualKey.D1,
            2 => InputSimulator.VirtualKey.D2,
            3 => InputSimulator.VirtualKey.D3,
            4 => InputSimulator.VirtualKey.D4,
            5 => InputSimulator.VirtualKey.D5,
            6 => InputSimulator.VirtualKey.D6,
            7 => InputSimulator.VirtualKey.D7,
            8 => InputSimulator.VirtualKey.D8,
            9 => InputSimulator.VirtualKey.D9,
            _ => throw new ArgumentException($"Unknown number: {number}")
        };

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(1, 0x70)]
    [InlineData(6, 0x75)]
    [InlineData(12, 0x7B)]
    public void FunctionKeys_ShouldHaveCorrectValues(int functionNumber, int expectedValue)
    {
        // Act
        var actualValue = functionNumber switch
        {
            1 => InputSimulator.VirtualKey.F1,
            2 => InputSimulator.VirtualKey.F2,
            3 => InputSimulator.VirtualKey.F3,
            4 => InputSimulator.VirtualKey.F4,
            5 => InputSimulator.VirtualKey.F5,
            6 => InputSimulator.VirtualKey.F6,
            7 => InputSimulator.VirtualKey.F7,
            8 => InputSimulator.VirtualKey.F8,
            9 => InputSimulator.VirtualKey.F9,
            10 => InputSimulator.VirtualKey.F10,
            11 => InputSimulator.VirtualKey.F11,
            12 => InputSimulator.VirtualKey.F12,
            _ => throw new ArgumentException($"Unknown function key: F{functionNumber}")
        };

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(nameof(InputSimulator.VirtualKey.Left), 0x25)]
    [InlineData(nameof(InputSimulator.VirtualKey.Up), 0x26)]
    [InlineData(nameof(InputSimulator.VirtualKey.Right), 0x27)]
    [InlineData(nameof(InputSimulator.VirtualKey.Down), 0x28)]
    public void ArrowKeys_ShouldHaveCorrectValues(string keyName, int expectedValue)
    {
        // Act
        var actualValue = keyName switch
        {
            nameof(InputSimulator.VirtualKey.Left) => InputSimulator.VirtualKey.Left,
            nameof(InputSimulator.VirtualKey.Up) => InputSimulator.VirtualKey.Up,
            nameof(InputSimulator.VirtualKey.Right) => InputSimulator.VirtualKey.Right,
            nameof(InputSimulator.VirtualKey.Down) => InputSimulator.VirtualKey.Down,
            _ => throw new ArgumentException($"Unknown key: {keyName}")
        };

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(nameof(InputSimulator.VirtualKey.Home), 0x24)]
    [InlineData(nameof(InputSimulator.VirtualKey.End), 0x23)]
    [InlineData(nameof(InputSimulator.VirtualKey.PageUp), 0x21)]
    [InlineData(nameof(InputSimulator.VirtualKey.PageDown), 0x22)]
    [InlineData(nameof(InputSimulator.VirtualKey.Insert), 0x2D)]
    public void NavigationKeys_ShouldHaveCorrectValues(string keyName, int expectedValue)
    {
        // Act
        var actualValue = keyName switch
        {
            nameof(InputSimulator.VirtualKey.Home) => InputSimulator.VirtualKey.Home,
            nameof(InputSimulator.VirtualKey.End) => InputSimulator.VirtualKey.End,
            nameof(InputSimulator.VirtualKey.PageUp) => InputSimulator.VirtualKey.PageUp,
            nameof(InputSimulator.VirtualKey.PageDown) => InputSimulator.VirtualKey.PageDown,
            nameof(InputSimulator.VirtualKey.Insert) => InputSimulator.VirtualKey.Insert,
            _ => throw new ArgumentException($"Unknown key: {keyName}")
        };

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void AllLetterKeys_ShouldBeSequential()
    {
        // Assert - verify A-Z are sequential
        Assert.Equal(InputSimulator.VirtualKey.A + 1, InputSimulator.VirtualKey.B);
        Assert.Equal(InputSimulator.VirtualKey.Y + 1, InputSimulator.VirtualKey.Z);
        Assert.Equal(25, InputSimulator.VirtualKey.Z - InputSimulator.VirtualKey.A);
    }

    [Fact]
    public void AllNumberKeys_ShouldBeSequential()
    {
        // Assert - verify 0-9 are sequential
        Assert.Equal(InputSimulator.VirtualKey.D0 + 1, InputSimulator.VirtualKey.D1);
        Assert.Equal(InputSimulator.VirtualKey.D8 + 1, InputSimulator.VirtualKey.D9);
        Assert.Equal(9, InputSimulator.VirtualKey.D9 - InputSimulator.VirtualKey.D0);
    }

    [Fact]
    public void AllFunctionKeys_ShouldBeSequential()
    {
        // Assert - verify F1-F12 are sequential
        Assert.Equal(InputSimulator.VirtualKey.F1 + 1, InputSimulator.VirtualKey.F2);
        Assert.Equal(InputSimulator.VirtualKey.F11 + 1, InputSimulator.VirtualKey.F12);
        Assert.Equal(11, InputSimulator.VirtualKey.F12 - InputSimulator.VirtualKey.F1);
    }
}
