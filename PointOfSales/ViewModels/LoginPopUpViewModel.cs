using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PointOfSales.Common.Resources;
using PointOfSales.Core.Constants;
using PointOfSales.Core.Data;
using PointOfSales.Core.Entities.Security;
using PointOfSales.Core.Exceptions;
using PointOfSales.Core.IEngines;
using PointOfSales.Core.Utils;
using PointOfSales.Engine.Utils;
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
            string machineUniqueCode = GetEngine<ISystemInformation>().GetMachineUniqueCode();
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = Common.Resources.ApplicationErrors.UsernameRequired;
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = Common.Resources.ApplicationErrors.PasswordRequired;
                ;
                return;
            }

            try
            {
                var permission = PermissionCodes.GetPermissionId(PermissionCodes.LoginToSystem);
                var engine = GetEngine<IAuthenticationEngine>();
                var user = await engine.AuthenticateUserAsync(_username, _password);
                bool rewriteIni = false;
                //validate machine
                if (string.IsNullOrEmpty(Configurations.StoreCode))
                {
                    // this is a first time setup
                }

                var deviceEngine = GetEngine<IDeviceEngine>();
                if (Configurations.MachineCode == short.MinValue)
                {
                    //register the machine
                    await deviceEngine.RegisterDeviceAsync(machineUniqueCode);
                    rewriteIni = true;
                }
                else
                {
                    //validate the machine ids
                    var device = await deviceEngine.GetDeviceByUniqueCode(machineUniqueCode);
                    if (device == null || device.IsDeleted)
                    {
                        throw new SwiftException(ApplicationErrors.DeviceNotFound, machineUniqueCode);
                    }

                    if (!device.IsActive)
                    {
                        throw new SwiftException(ApplicationErrors.DeviceNotActive, machineUniqueCode);
                    }

                    if (device.DeviceId != Configurations.MachineCode)
                    {
                        throw new SwiftException(ApplicationErrors.DeviceMismatch, machineUniqueCode);
                    }
                }

                //save in ini file
                if (rewriteIni)
                {
                    await GetEngine<IUnitOfWork>().SaveChangesAsync();
                    var device = await deviceEngine.GetDeviceByUniqueCode(machineUniqueCode);
                    var deviceId = device?.DeviceId ?? short.MinValue;
                    var iniEngine = GetEngine<IIniEngine>();
                    await iniEngine.UpdateAsync(nameof(Configurations.MachineCode), deviceId);
                    await iniEngine.InitAsync();
                    Configurations.IniEngine = iniEngine;
                }

                if (Configurations.MachineCode != short.MinValue && Configurations.StoreCode != string.Empty)
                {
                    var activityLog = new ActivityLog
                    {
                        PermissionId = permission,
                        // LocationId = Configurations.LocationId,
                        DeviceId = Configurations.MachineCode,
                        Message = $"User {Username} attempt logged  in",
                        AccessedAt = DateTime.UtcNow
                    };
                    await GetEngine<IUnitOfWork>().AuditLogRepository.WriteToLogAsync(activityLog);
                }

                await GetEngine<IUnitOfWork>().SaveChangesAsync();


                // GlobalAuthenticator.Authenticate(user);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
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