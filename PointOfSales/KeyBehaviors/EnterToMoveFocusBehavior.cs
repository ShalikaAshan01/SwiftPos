using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using System;
using System.Linq;

public static class EnterToMoveFocusBehavior
{
    public static readonly AttachedProperty<bool> IsEnabledProperty =
        AvaloniaProperty.RegisterAttached<Control, bool>(
            "IsEnabled",
            typeof(EnterToMoveFocusBehavior),
            defaultValue: false);

    public static void SetIsEnabled(AvaloniaObject element, bool value) =>
        element.SetValue(IsEnabledProperty, value);

    public static bool GetIsEnabled(AvaloniaObject element) =>
        element.GetValue(IsEnabledProperty);

    static EnterToMoveFocusBehavior()
    {
        // Explicitly create the observer
        IsEnabledProperty.Changed.Subscribe(new PropertyChangedObserver());
    }

    private class PropertyChangedObserver : IObserver<AvaloniaPropertyChangedEventArgs<bool>>
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is Control control)
            {
                control.KeyDown -= OnKeyDown;

                if (e.NewValue.Value)
                {
                    control.KeyDown += OnKeyDown;
                }
            }
        }
    }

    private static void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && sender is Control control)
        {
            var root = control.GetVisualRoot();
            if (root is not Visual visualRoot) return;

            var focusables = visualRoot
                .GetVisualDescendants()
                .OfType<Control>()
                .Where(x => x.Focusable && x.IsEffectivelyEnabled && x.IsVisible)
                .ToList();

            var currentIndex = focusables.IndexOf(control);
            if (currentIndex >= 0 && currentIndex < focusables.Count - 1)
            {
                focusables[currentIndex + 1].Focus();
                e.Handled = true;
            }
        }
    }
}
