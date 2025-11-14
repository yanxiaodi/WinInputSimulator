using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinInputSimulator;

namespace WinInputSimulator.Demo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly InputSimulator _simulator;

    public MainWindow()
    {
        InitializeComponent();
        _simulator = new InputSimulator();
    }

    private async void SimulateButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Disable button during operation
            SimulateButton.IsEnabled = false;
            UpdateStatus("Starting Notepad...", "#FFA500");

            // Get the text to input
            string textToInput = InputTextBox.Text;

            if (string.IsNullOrWhiteSpace(textToInput))
            {
                UpdateStatus("Please enter some text first!", "#FF0000");
                SimulateButton.IsEnabled = true;
                return;
            }

            // Start Notepad
            Process? notepadProcess = null;
            try
            {
                notepadProcess = Process.Start("notepad.exe");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Failed to start Notepad: {ex.Message}", "#FF0000");
                SimulateButton.IsEnabled = true;
                return;
            }

            if (notepadProcess == null)
            {
                UpdateStatus("Failed to start Notepad!", "#FF0000");
                SimulateButton.IsEnabled = true;
                return;
            }

            // Wait for Notepad to be ready
            UpdateStatus("Waiting for Notepad to load...", "#FFA500");
            await Task.Delay(2000); // Increased delay for better reliability

            // Find and activate Notepad window
            UpdateStatus("Activating Notepad window...", "#FFA500");
            bool activated = _simulator.ActivateWindowByProcessName("notepad");

            if (!activated)
            {
                // Try to find by window title
                var hWnd = _simulator.FindWindowByTitle("Notepad");
                if (hWnd != IntPtr.Zero)
                {
                    _simulator.ActivateWindow(hWnd);
                }
                else
                {
                    // Try Untitled - Notepad for newer Windows versions
                    hWnd = _simulator.FindWindowByTitle("Untitled");
                    if (hWnd != IntPtr.Zero)
                    {
                        _simulator.ActivateWindow(hWnd);
                    }
                    else
                    {
                        UpdateStatus("Could not activate Notepad window!", "#FF0000");
                        SimulateButton.IsEnabled = true;
                        return;
                    }
                }
            }

            // Wait a bit more to ensure window is focused
            await Task.Delay(800);

            // Type the text into Notepad
            UpdateStatus($"Typing {textToInput.Length} characters into Notepad...", "#007ACC");
            _simulator.InputText(textToInput, delayMs: 50); // Increased from 30 to 50

            // Success
            UpdateStatus("✓ Text successfully typed into Notepad!", "#008000");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error: {ex.Message}", "#FF0000");
        }
        finally
        {
            // Re-enable button
            await Task.Delay(2000);
            SimulateButton.IsEnabled = true;
            UpdateStatus("Ready", "#008000");
        }
    }

    private void UpdateStatus(string message, string colorHex)
    {
        StatusTextBlock.Text = message;
        StatusTextBlock.Foreground = new System.Windows.Media.SolidColorBrush(
            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorHex));
    }
}