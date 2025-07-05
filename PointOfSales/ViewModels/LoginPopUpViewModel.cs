using System;
using System.Threading.Tasks;
using PointOfSales.Core.IEngines;
using PointOfSales.KeyBehaviors;
using PointOfSales.Utils;

namespace PointOfSales.ViewModels
{
    public class LoginPopUpViewModel : ViewModelBase, IDisposable
    {

        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        public LoginPopUpViewModel()
        {
            VirtualKeyboardHelper.SubmitTriggered += OnKeyboardSubmit;
            LoginCommand = new AsyncRelayCommand(Login);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        public IAsyncCommand LoginCommand { get; }

        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = Common.Resources.ApplicationErrors.UsernameRequired;
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = Common.Resources.ApplicationErrors.PasswordRequired;;
                return;
            }
            
            try
            {
                var engine = GetEngine<IAuthenticationEngine>();
                var user = await engine.AuthenticateUserAsync(_username, _password);
                GlobalAuthenticator.Authenticate(user);
            }
            catch (Exception e)
            {
                Engine.Utils.Common.Logger.LogError(e, e.ToString());
            }
            VirtualKeyboardHelper.CloseKeyboard();
        }

        private void OnKeyboardSubmit(object? sender, EventArgs e)
        {
            if (LoginCommand.CanExecute(null))
                LoginCommand.Execute(null);
        }

        public void Dispose()
        {
            VirtualKeyboardHelper.SubmitTriggered -= OnKeyboardSubmit;
        }
    }
}
