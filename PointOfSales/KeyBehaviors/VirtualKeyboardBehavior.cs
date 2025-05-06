using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using PointOfSales.Views.Shared;

namespace PointOfSales.KeyBehaviors;

public class VirtualKeyboardBehavior : Behavior<TextBox>
{
    private VirtualKeyboard? _keyboardWindow;
    private TextBox? _textBox;

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject != null)
        {
            AssociatedObject.GotFocus += OnGotFocus;
            AssociatedObject.LostFocus += OnLostFocus;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.GotFocus -= OnGotFocus;
            AssociatedObject.LostFocus -= OnLostFocus;
        }

        base.OnDetaching();
    }

    private void OnGotFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            _textBox = textBox;

            // Open keyboard only if not already open
            if (_keyboardWindow == null || !_keyboardWindow.IsVisible)
            {
                OpenVirtualKeyboard();
            }
            else
            {
                _keyboardWindow.Close();
            }
        }
    }

    private void OpenVirtualKeyboard()
    {
        _keyboardWindow = null;
        _keyboardWindow = new VirtualKeyboard();
        _keyboardWindow.KeyPressed += OnKeyPressed;
        _keyboardWindow.Closed += (s, args) => _keyboardWindow = null;
        _keyboardWindow.Show();
    }
    
    private void OnKeyPressed(object? sender, string key)
    {
        if (_textBox == null) return;
        _textBox.Text += key;
        // var caretIndex = _textBox.CaretIndex;
        // _textBox.Text = _textBox.Text?.Insert(caretIndex, key);
        // _textBox.CaretIndex = caretIndex + key.Length;
    }
    
    private void OnLostFocus(object? sender, RoutedEventArgs e)
    {
        Task.Run(async () =>
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (_keyboardWindow != null && !_keyboardWindow.IsActive)
                {
                    _keyboardWindow.Close();
                    _keyboardWindow = null;
                }
            });
        });
    }
}