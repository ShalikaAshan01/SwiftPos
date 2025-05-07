using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using PointOfSales.Views.Shared;

namespace PointOfSales.KeyBehaviors;

public static class VirtualKeyboardHelper
{
    public static readonly AttachedProperty<bool> IsEnabledProperty =
        AvaloniaProperty.RegisterAttached<Control, bool>(
            "IsEnabled",
            typeof(VirtualKeyboardHelper),
            defaultValue: false);

    private static readonly AttachedProperty<VirtualKeyboard?> KeyboardWindowProperty =
        AvaloniaProperty.RegisterAttached<Control, VirtualKeyboard?>(
            "KeyboardWindow",
            typeof(VirtualKeyboardHelper));

    public static void SetIsEnabled(AvaloniaObject element, bool value) =>
        element.SetValue(IsEnabledProperty, value);

    public static bool GetIsEnabled(AvaloniaObject element) =>
        element.GetValue(IsEnabledProperty);

    static VirtualKeyboardHelper()
    {
        IsEnabledProperty.Changed.Subscribe(new PropertyChangedObserver());
    }

    private class PropertyChangedObserver : IObserver<AvaloniaPropertyChangedEventArgs<bool>>
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is not TextBox textBox) return;
            textBox.GotFocus -= OnGotFocus;
            textBox.LostFocus -= OnLostFocus;

            if (!e.NewValue.Value) return;
            textBox.GotFocus += OnGotFocus;
            textBox.LostFocus += OnLostFocus;
        }

        private void OnGotFocus(object? sender, RoutedEventArgs e)
        {
            if (sender is not TextBox textBox) return;

            var existingKeyboard = textBox.GetValue(KeyboardWindowProperty);
            if (existingKeyboard != null && existingKeyboard.IsVisible) return;
            var keyboard = new VirtualKeyboard();

            void OnKeyboardOnKeyPressed(object? _, string key)
            {
                textBox.Text ??= "";
                var caretIndex = textBox.CaretIndex;
                textBox.Text = textBox.Text.Insert(caretIndex, key);
                textBox.CaretIndex = caretIndex + key.Length;
                if (key == "Q") // Check if the key is 'q'
                {
                    MoveFocusToNextTextBox(textBox); // Move focus after pressing 'q'
                }
            }

            keyboard.KeyPressed += OnKeyboardOnKeyPressed;
            keyboard.Closed += (_, _) =>
            {
                keyboard.KeyPressed -= OnKeyboardOnKeyPressed;
                textBox.SetValue(KeyboardWindowProperty, null);
            };
            textBox.SetValue(KeyboardWindowProperty, keyboard);
            keyboard.Show();
        }

        private void OnLostFocus(object? sender, RoutedEventArgs e)
        {
            if (sender is not TextBox textBox) return;

            Task.Run(async () =>
            {
                await Task.Delay(300);
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var keyboard = textBox.GetValue(KeyboardWindowProperty);
                    if (keyboard != null && !keyboard.IsActive)
                    {
                        keyboard.Close();
                        textBox.SetValue(KeyboardWindowProperty, null);
                    }
                });
            });
        }

        // Move focus to the next TextBox in the visual tree
        private void MoveFocusToNextTextBox(TextBox currentTextBox)
        {
            var root = currentTextBox.GetVisualRoot();
            if (root is not Visual visualRoot) return;

            // Find all focusable TextBoxes in the visual tree
            var textBoxes = visualRoot
                .GetVisualDescendants()
                .OfType<TextBox>()
                .Where(x => x.Focusable && x.IsEffectivelyEnabled && x.IsVisible)
                .ToList();

            var currentIndex = textBoxes.IndexOf(currentTextBox);
            if (currentIndex >= 0 && currentIndex < textBoxes.Count - 1)
            {
                // Focus on the next TextBox
                textBoxes[currentIndex + 1].Focus();
            }
        }
    }
}
