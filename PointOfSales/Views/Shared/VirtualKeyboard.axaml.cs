using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Threading;
using PointOfSales.Core.Constants;
using PointOfSales.Core.Keyboard;
using PointOfSales.KeyBehaviors;
using PointOfSales.Utils.Keyboard;

namespace PointOfSales.Views.Shared
{
    public partial class VirtualKeyboard : Window
    {
        public event EventHandler<VirtualKey>? KeyPressed;

        private readonly IKeyboardLayout _keyboardLayout;

        public VirtualKeyboard(IKeyboardLayout? keyboardLayout = null)
        {
            _keyboardLayout = keyboardLayout ?? new SwiftBoard(); // Your default layout implementation

            InitializeComponent();

            Topmost = true;
            Title = $"{LocalConfigurations.ApplicationName}:Virtual Keyboard";
            SystemDecorations = SystemDecorations.Full;
            CanResize = false;

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var screen = GetPrimaryScreen();
                if (screen != null)
                {
                    var screenBounds = screen.WorkingArea;
                    var windowHeight = this.Height;
                    Width = screenBounds.Width;
                    Position = new PixelPoint(0, (int)(screenBounds.Bottom - windowHeight));
                }
                BuildKeyboard();
            });
        }

        private Screen? GetPrimaryScreen() => Screens.Primary;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void BuildKeyboard()
        {
            var keyboardGrid = this.FindControl<Grid>("KeyboardGrid");
            if (keyboardGrid == null)
                throw new InvalidOperationException("KeyboardGrid not found in XAML.");

            keyboardGrid.RowDefinitions.Clear();
            keyboardGrid.Children.Clear();

            var bgColor = _keyboardLayout.GetBackgroundColor();
            keyboardGrid.Background = new SolidColorBrush(Color.FromRgb(bgColor.Item1, bgColor.Item2, bgColor.Item3));

            var keyBgColor = _keyboardLayout.GetKeyBackgroundColor();
            var keyBorderColor = _keyboardLayout.GetKeyBorderColor();

            var layout = _keyboardLayout.GetLayout();

            for (int rowIndex = 0; rowIndex < layout.Count; rowIndex++)
            {
                var row = layout[rowIndex];

                keyboardGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));

                var rowGrid = new Grid
                {
                    Margin = new Thickness(1, 2, 1, 2),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };

                // Define columns for the row grid according to key sizes (using star sizing)
                foreach (var key in row)
                {
                    rowGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(key.Size, GridUnitType.Star)));
                }

                for (int colIndex = 0; colIndex < row.Count; colIndex++)
                {
                    var key = row[colIndex];

                    if (key.KeyType == KeyTypes.None)
                    {
                        // Add empty space (transparent rectangle)
                        var rect = new Rectangle
                        {
                            Fill = Brushes.Transparent
                        };
                        Grid.SetColumn(rect, colIndex);
                        rowGrid.Children.Add(rect);
                        continue;
                    }

                    var button = new Button
                    {
                        Margin = new Thickness(2),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Background = new SolidColorBrush(Color.FromRgb(keyBgColor.Item1, keyBgColor.Item2, keyBgColor.Item3)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(keyBorderColor.Item1, keyBorderColor.Item2, keyBorderColor.Item3)),
                        BorderThickness = new Thickness(1),
                        Tag = key
                    };

                    // Show text depends on caps lock state
                    string displayText = VirtualKeyboardHelper.CapsLock ? key.Caps.Display : key.Normal.Display;

                    // Use a Viewbox to scale text nicely inside button
                    button.Content = new Viewbox
                    {
                        Stretch = Stretch.Uniform,
                        Child = new TextBlock
                        {
                            Text = displayText,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            TextAlignment = TextAlignment.Center,
                            FontWeight = FontWeight.SemiBold
                        }
                    };

                    button.Click += OnButtonClick;

                    Grid.SetColumn(button, colIndex);
                    rowGrid.Children.Add(button);
                }

                Grid.SetRow(rowGrid, rowIndex);
                keyboardGrid.Children.Add(rowGrid);
            }
        }

        private void OnButtonClick(object? sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            if (btn.Tag is not VirtualKey key)
                return;

            switch (key.KeyType)
            {
                case KeyTypes.Shift:
                    VirtualKeyboardHelper.CapsLock = !VirtualKeyboardHelper.CapsLock;
                    BuildKeyboard(); // rebuild to update key labels
                    break;
                default:
                    OnKeyPressed(key);
                    break;
            }
        }

        protected virtual void OnKeyPressed(VirtualKey key)
        {
            KeyPressed?.Invoke(this, key);
        }
    }
}
