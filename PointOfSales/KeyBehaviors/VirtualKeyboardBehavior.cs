// using System.Threading.Tasks;
// using Avalonia.Controls;
// using Avalonia.Input;
// using Avalonia.Interactivity;
// using Avalonia.Threading;
// using Avalonia.Xaml.Interactivity;
// using PointOfSales.Views.Shared;
//
// namespace PointOfSales.KeyBehaviors;
//
// public class VirtualKeyboardBehavior : Behavior<TextBox>
// {
//     protected override void OnAttached()
//     {
//         base.OnAttached();
//
//         if (AssociatedObject != null)
//         {
//             AssociatedObject.GotFocus += OnGotFocus;
//             AssociatedObject.LostFocus += OnLostFocus;
//         }
//     }
//
//     protected override void OnDetaching()
//     {
//         if (AssociatedObject != null)
//         {
//             AssociatedObject.GotFocus -= OnGotFocus;
//             AssociatedObject.LostFocus -= OnLostFocus;
//         }
//
//         base.OnDetaching();
//     }
//
//     private void OnGotFocus(object? sender, RoutedEventArgs e)
//     {
//         if (sender is TextBox textBox)
//         {
//             VirtualKeyboardHelper.CurrentTextBox = textBox;
//
//             // Open keyboard only if not already open
//             if (VirtualKeyboardHelper.KeyboardWindow == null || !VirtualKeyboardHelper.KeyboardWindow.IsVisible)
//             {
//                 OpenVirtualKeyboard();
//             }
//             else
//             {
//                 VirtualKeyboardHelper.KeyboardWindow.Close();
//             }
//         }
//     }
//
//     private void OpenVirtualKeyboard()
//     {
//         if (VirtualKeyboardHelper.KeyboardWindow != null)
//         {
//             return;
//         }
//         VirtualKeyboardHelper.KeyboardWindow = null;
//         VirtualKeyboardHelper.KeyboardWindow = new VirtualKeyboard();
//         VirtualKeyboardHelper.KeyboardWindow.KeyPressed += OnKeyPressed;
//         VirtualKeyboardHelper.KeyboardWindow.Closed += (s, args) => VirtualKeyboardHelper.KeyboardWindow = null;
//         VirtualKeyboardHelper.KeyboardWindow.Show();
//     }
//     
//     private void OnKeyPressed(object? sender, string key)
//     {
//         if (VirtualKeyboardHelper.CurrentTextBox == null) return;
//         VirtualKeyboardHelper.CurrentTextBox.Text += key;
//         // var caretIndex = _textBox.CaretIndex;
//         // _textBox.Text = _textBox.Text?.Insert(caretIndex, key);
//         // _textBox.CaretIndex = caretIndex + key.Length;
//     }
//     
//     private void OnLostFocus(object? sender, RoutedEventArgs e)
//     {
//         Task.Run(async () =>
//         {
//             await Dispatcher.UIThread.InvokeAsync(() =>
//             {
//                 if (VirtualKeyboardHelper.KeyboardWindow != null && !VirtualKeyboardHelper.KeyboardWindow.IsActive)
//                 {
//                     VirtualKeyboardHelper.KeyboardWindow.Close();
//                     VirtualKeyboardHelper.KeyboardWindow = null;
//                 }
//             });
//         });
//     }
// }