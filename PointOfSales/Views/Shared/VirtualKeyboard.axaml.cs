using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Threading;
using PointOfSales.Core.Constants;

namespace PointOfSales.Views.Shared
{
    public partial class VirtualKeyboard : Window
    {
        public event EventHandler<string>? KeyPressed;


        public VirtualKeyboard()
        {
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
                    var screenBounds = screen.WorkingArea; // Get working area (screen area excluding taskbar)
                    var windowHeight = this.Height;
                    Width = screenBounds.Width;
                    Position = new PixelPoint(0, (int)(screenBounds.Bottom - windowHeight));
                }
            });
        }
        
        private Screen? GetPrimaryScreen()
        {
            return Screens.Primary;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void OnKeyClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;
            var key = button.Content?.ToString();
            if(!string.IsNullOrEmpty(key)) 
                OnKeyPressed(key);
        }
        
        protected virtual void OnKeyPressed(string key)
        {
            if(KeyPressed == null) return;
            KeyPressed.Invoke(this, key);
        }

    }
}