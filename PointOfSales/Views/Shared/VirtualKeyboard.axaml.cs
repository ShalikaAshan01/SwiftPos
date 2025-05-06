using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace PointOfSales.Views.Shared
{
    public partial class VirtualKeyboard : Window
    {
        public event EventHandler<string>? KeyPressed;


        public VirtualKeyboard()
        {
            InitializeComponent();
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