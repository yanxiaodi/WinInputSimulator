using System.Runtime.InteropServices;

namespace WinInputSimulator;

/// <summary>
/// Keyboard and Mouse Input Simulator - Using Win32 API
/// </summary>
public class InputSimulator
{
    #region Win32 API Declarations

    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    private static extern uint MapVirtualKey(uint uCode, uint uMapType);

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int count);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT
    {
        public uint type;
        public InputUnion U;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct InputUnion
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;
        [FieldOffset(0)]
        public KEYBDINPUT ki;
        [FieldOffset(0)]
        public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct HARDWAREINPUT
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    // ShowWindow constants
    private const int SW_RESTORE = 9;

    // Input type constants
    private const uint INPUT_MOUSE = 0;
    private const uint INPUT_KEYBOARD = 1;
    private const uint INPUT_HARDWARE = 2;

    #endregion

    #region Constant Definitions

    // keybd_event flags
    private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
    private const uint KEYEVENTF_KEYUP = 0x0002;
    private const uint KEYEVENTF_UNICODE = 0x0004;
    private const uint KEYEVENTF_SCANCODE = 0x0008;

    // mouse_event flags
    private const uint MOUSEEVENTF_MOVE = 0x0001;
    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;
    private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
    private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
    private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
    private const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
    private const uint MOUSEEVENTF_WHEEL = 0x0800;
    private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

    // Virtual key codes
    private const byte VK_SHIFT = 0x10;
    private const byte VK_CONTROL = 0x11;
    private const byte VK_MENU = 0x12; // Alt
    private const byte VK_RETURN = 0x0D;
    private const byte VK_TAB = 0x09;
    private const byte VK_ESCAPE = 0x1B;
    private const byte VK_SPACE = 0x20;
    private const byte VK_BACK = 0x08;
    private const byte VK_DELETE = 0x2E;

    #endregion

    #region Window Management Methods

    /// <summary>
    /// Find window by title (partial match)
    /// </summary>
    public IntPtr FindWindowByTitle(string titleContains)
    {
        if (string.IsNullOrEmpty(titleContains))
            return IntPtr.Zero;
            
        IntPtr hWnd = IntPtr.Zero;
        foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcesses())
        {
            if (proc.MainWindowTitle.Contains(titleContains, StringComparison.OrdinalIgnoreCase))
            {
                hWnd = proc.MainWindowHandle;
                if (hWnd != IntPtr.Zero)
                    break;
            }
        }
        return hWnd;
    }

    /// <summary>
    /// Find window by process name
    /// </summary>
    public IntPtr FindWindowByProcessName(string processName)
    {
        System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(processName);
        if (processes.Length > 0)
        {
            return processes[0].MainWindowHandle;
        }
        return IntPtr.Zero;
    }

    /// <summary>
    /// Activate (bring to front) a window
    /// </summary>
    public bool ActivateWindow(IntPtr hWnd)
    {
        if (hWnd == IntPtr.Zero)
            return false;

        ShowWindow(hWnd, SW_RESTORE);
        return SetForegroundWindow(hWnd);
    }

    /// <summary>
    /// Find and activate window by process name
    /// </summary>
    public bool ActivateWindowByProcessName(string processName)
    {
        IntPtr hWnd = FindWindowByProcessName(processName);
        return ActivateWindow(hWnd);
    }

    /// <summary>
    /// Get the handle of the currently active window
    /// </summary>
    public IntPtr GetActiveWindow()
    {
        return GetForegroundWindow();
    }

    #endregion

    #region Keyboard Simulation Methods

    /// <summary>
    /// Press down a key
    /// </summary>
    private void KeyDown(byte virtualKey)
    {
        keybd_event(virtualKey, 0, 0, UIntPtr.Zero);
    }

    /// <summary>
    /// Release a key
    /// </summary>
    private void KeyUp(byte virtualKey)
    {
        keybd_event(virtualKey, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
    }

    /// <summary>
    /// Press and release a key
    /// </summary>
    public void KeyPress(byte virtualKey)
    {
        KeyDown(virtualKey);
        Thread.Sleep(10);
        KeyUp(virtualKey);
        Thread.Sleep(10);
    }

    /// <summary>
    /// Input a single character using SendInput (more reliable)
    /// </summary>
    public void InputCharacter(char character)
    {
        // Handle special characters that need virtual key codes
        switch (character)
        {
            case '\r': // Carriage return
            case '\n': // Line feed
                PressEnter();
                return;
            case '\t': // Tab
                PressTab();
                return;
            case '\b': // Backspace
                PressBackspace();
                return;
            default:
                // Use SendInput with Unicode for better reliability
                INPUT[] inputs = new INPUT[2];

                // Key down
                inputs[0] = new INPUT
                {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = 0,
                            wScan = (ushort)character,
                            dwFlags = KEYEVENTF_UNICODE,
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };

                // Key up
                inputs[1] = new INPUT
                {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = 0,
                            wScan = (ushort)character,
                            dwFlags = KEYEVENTF_UNICODE | KEYEVENTF_KEYUP,
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };

                SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
                break;
        }
    }

    /// <summary>
    /// Input a text string with full Unicode support including surrogate pairs
    /// </summary>
    public void InputText(string text, int delayMs = 50)
    {
        if (string.IsNullOrEmpty(text))
            return;

        // Use StringInfo to properly handle Unicode characters including surrogate pairs
        var textElements = System.Globalization.StringInfo.GetTextElementEnumerator(text);
        
        while (textElements.MoveNext())
        {
            string element = textElements.GetTextElement();
            
            // Handle special control characters
            if (element.Length == 1)
            {
                char c = element[0];
                switch (c)
                {
                    case '\r':
                    case '\n':
                        PressEnter();
                        Thread.Sleep(delayMs);
                        continue;
                    case '\t':
                        PressTab();
                        Thread.Sleep(delayMs);
                        continue;
                    case '\b':
                        PressBackspace();
                        Thread.Sleep(delayMs);
                        continue;
                }
            }

            // Send each character (including surrogate pairs)
            foreach (char c in element)
            {
                INPUT[] inputs = new INPUT[2];

                // Key down
                inputs[0] = new INPUT
                {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = 0,
                            wScan = (ushort)c,
                            dwFlags = KEYEVENTF_UNICODE,
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };

                // Key up
                inputs[1] = new INPUT
                {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = 0,
                            wScan = (ushort)c,
                            dwFlags = KEYEVENTF_UNICODE | KEYEVENTF_KEYUP,
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };

                SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
                
                // Small delay between surrogate pair code units
                if (element.Length > 1)
                {
                    Thread.Sleep(5);
                }
            }
            
            Thread.Sleep(delayMs);
        }
    }

    /// <summary>
    /// Simulate key combination (e.g., Ctrl+C)
    /// </summary>
    public void InputCombination(params byte[] keys)
    {
        // Press all modifier keys
        foreach (byte key in keys)
        {
            KeyDown(key);
            Thread.Sleep(10);
        }

        // Release all modifier keys (in reverse order)
        for (int i = keys.Length - 1; i >= 0; i--)
        {
            KeyUp(keys[i]);
            Thread.Sleep(10);
        }
    }

    /// <summary>
    /// Ctrl + Key
    /// </summary>
    public void CtrlKey(byte key)
    {
        InputCombination(VK_CONTROL, key);
    }

    /// <summary>
    /// Alt + Key
    /// </summary>
    public void AltKey(byte key)
    {
        InputCombination(VK_MENU, key);
    }

    /// <summary>
    /// Shift + Key
    /// </summary>
    public void ShiftKey(byte key)
    {
        InputCombination(VK_SHIFT, key);
    }

    /// <summary>
    /// Press Enter key
    /// </summary>
    public void PressEnter()
    {
        KeyPress(VK_RETURN);
    }

    /// <summary>
    /// Press Tab key
    /// </summary>
    public void PressTab()
    {
        KeyPress(VK_TAB);
    }

    /// <summary>
    /// Press Backspace key
    /// </summary>
    public void PressBackspace()
    {
        KeyPress(VK_BACK);
    }

    /// <summary>
    /// Press Delete key
    /// </summary>
    public void PressDelete()
    {
        KeyPress(VK_DELETE);
    }

    #endregion

    #region Mouse Simulation Methods

    /// <summary>
    /// Get current mouse position
    /// </summary>
    public (int X, int Y) GetMousePosition()
    {
        GetCursorPos(out POINT point);
        return (point.X, point.Y);
    }

    /// <summary>
    /// Move mouse to specified position
    /// </summary>
    public void MoveMouse(int x, int y)
    {
        SetCursorPos(x, y);
    }

    /// <summary>
    /// Move mouse relatively
    /// </summary>
    public void MoveMouseRelative(int dx, int dy)
    {
        mouse_event(MOUSEEVENTF_MOVE, dx, dy, 0, UIntPtr.Zero);
    }

    /// <summary>
    /// Press left mouse button down
    /// </summary>
    public void LeftMouseDown()
    {
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
    }

    /// <summary>
    /// Release left mouse button
    /// </summary>
    public void LeftMouseUp()
    {
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
    }

    /// <summary>
    /// Left mouse button click
    /// </summary>
    public void LeftClick()
    {
        LeftMouseDown();
        Thread.Sleep(50);
        LeftMouseUp();
        Thread.Sleep(50);
    }

    /// <summary>
    /// Left mouse button double click
    /// </summary>
    public void LeftDoubleClick()
    {
        LeftClick();
        Thread.Sleep(50);
        LeftClick();
    }

    /// <summary>
    /// Press right mouse button down
    /// </summary>
    public void RightMouseDown()
    {
        mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
    }

    /// <summary>
    /// Release right mouse button
    /// </summary>
    public void RightMouseUp()
    {
        mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
    }

    /// <summary>
    /// Right mouse button click
    /// </summary>
    public void RightClick()
    {
        RightMouseDown();
        Thread.Sleep(50);
        RightMouseUp();
        Thread.Sleep(50);
    }

    /// <summary>
    /// Press middle mouse button down
    /// </summary>
    public void MiddleMouseDown()
    {
        mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, UIntPtr.Zero);
    }

    /// <summary>
    /// Release middle mouse button
    /// </summary>
    public void MiddleMouseUp()
    {
        mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, UIntPtr.Zero);
    }

    /// <summary>
    /// Middle mouse button click
    /// </summary>
    public void MiddleClick()
    {
        MiddleMouseDown();
        Thread.Sleep(50);
        MiddleMouseUp();
        Thread.Sleep(50);
    }

    /// <summary>
    /// Mouse wheel scroll
    /// </summary>
    /// <param name="delta">Positive value scrolls up, negative value scrolls down</param>
    public void MouseWheel(int delta)
    {
        mouse_event(MOUSEEVENTF_WHEEL, 0, 0, (uint)delta, UIntPtr.Zero);
    }

    /// <summary>
    /// Click at specified position
    /// </summary>
    public void ClickAt(int x, int y)
    {
        MoveMouse(x, y);
        Thread.Sleep(100);
        LeftClick();
    }

    #endregion

    #region Virtual Key Code Constants (for convenience)

    public static class VirtualKey
    {
        public const byte Shift = VK_SHIFT;
        public const byte Control = VK_CONTROL;
        public const byte Alt = VK_MENU;
        public const byte Enter = VK_RETURN;
        public const byte Tab = VK_TAB;
        public const byte Escape = VK_ESCAPE;
        public const byte Space = VK_SPACE;
        public const byte Backspace = VK_BACK;
        public const byte Delete = VK_DELETE;

        // Letter keys
        public const byte A = 0x41;
        public const byte B = 0x42;
        public const byte C = 0x43;
        public const byte D = 0x44;
        public const byte E = 0x45;
        public const byte F = 0x46;
        public const byte G = 0x47;
        public const byte H = 0x48;
        public const byte I = 0x49;
        public const byte J = 0x4A;
        public const byte K = 0x4B;
        public const byte L = 0x4C;
        public const byte M = 0x4D;
        public const byte N = 0x4E;
        public const byte O = 0x4F;
        public const byte P = 0x50;
        public const byte Q = 0x51;
        public const byte R = 0x52;
        public const byte S = 0x53;
        public const byte T = 0x54;
        public const byte U = 0x55;
        public const byte V = 0x56;
        public const byte W = 0x57;
        public const byte X = 0x58;
        public const byte Y = 0x59;
        public const byte Z = 0x5A;

        // Number keys
        public const byte D0 = 0x30;
        public const byte D1 = 0x31;
        public const byte D2 = 0x32;
        public const byte D3 = 0x33;
        public const byte D4 = 0x34;
        public const byte D5 = 0x35;
        public const byte D6 = 0x36;
        public const byte D7 = 0x37;
        public const byte D8 = 0x38;
        public const byte D9 = 0x39;

        // Function keys
        public const byte F1 = 0x70;
        public const byte F2 = 0x71;
        public const byte F3 = 0x72;
        public const byte F4 = 0x73;
        public const byte F5 = 0x74;
        public const byte F6 = 0x75;
        public const byte F7 = 0x76;
        public const byte F8 = 0x77;
        public const byte F9 = 0x78;
        public const byte F10 = 0x79;
        public const byte F11 = 0x7A;
        public const byte F12 = 0x7B;

        // Arrow keys
        public const byte Left = 0x25;
        public const byte Up = 0x26;
        public const byte Right = 0x27;
        public const byte Down = 0x28;

        // Other common keys
        public const byte Home = 0x24;
        public const byte End = 0x23;
        public const byte PageUp = 0x21;
        public const byte PageDown = 0x22;
        public const byte Insert = 0x2D;
    }

    #endregion
}
