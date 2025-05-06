using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
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
            if (e.Sender is TextBox textBox)
            {
                textBox.GotFocus -= OnGotFocus;
                textBox.LostFocus -= OnLostFocus;

                if (e.NewValue.Value)
                {
                    textBox.GotFocus += OnGotFocus;
                    textBox.LostFocus += OnLostFocus;
                }
            }
        }

        private void OnGotFocus(object? sender, RoutedEventArgs e)
        {
            if (sender is not TextBox textBox) return;

            var existingKeyboard = textBox.GetValue(KeyboardWindowProperty);
            if (existingKeyboard == null || !existingKeyboard.IsVisible)
            {
                var keyboard = new VirtualKeyboard();
                keyboard.KeyPressed += (_, key) =>
                {
                    textBox.Text += key;
                };
                keyboard.Closed += (_, _) =>
                {
                    textBox.SetValue(KeyboardWindowProperty, null);
                };
                textBox.SetValue(KeyboardWindowProperty, keyboard);
                keyboard.Show();
            }
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
    }
}
