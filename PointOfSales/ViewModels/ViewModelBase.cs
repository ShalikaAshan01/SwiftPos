using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using PointOfSales.Views;

namespace PointOfSales.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected T GetEngine<T>() where T :class
        {
            return App.ServiceProvider.GetRequiredService<T>();
        }
        
        // Async command interface
        public interface IAsyncCommand : ICommand
        {
            Task ExecuteAsync(object? parameter);
            bool CanExecute(object? parameter);
        }

        // Async command implementation
        public class AsyncRelayCommand : IAsyncCommand
        {
            private readonly Func<Task> _execute;
            private readonly Func<bool>? _canExecute;
            private bool _isExecuting;

            public AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object? parameter)
                => !_isExecuting && (_canExecute?.Invoke() ?? true);

            public async void Execute(object? parameter)
                => await ExecuteAsync(parameter);

            public async Task ExecuteAsync(object? parameter)
            {
                if (!CanExecute(parameter))
                    return;

                try
                {
                    _isExecuting = true;
                    RaiseCanExecuteChanged();
                    await _execute();
                }
                finally
                {
                    _isExecuting = false;
                    RaiseCanExecuteChanged();
                }
            }

            public event EventHandler? CanExecuteChanged;

            public void RaiseCanExecuteChanged()
                => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public abstract class ObservableObjectBase : ObservableObject
    {
        protected T GetEngine<T>() where T :class
        {
            return App.ServiceProvider.GetRequiredService<T>();
        }

    }
}
