using System;
using System.Collections.Generic;
using PointOfSales.Core.Entities.Infrastructure;
using PointOfSales.Core.Entities.Security;

namespace PointOfSales.Utils
{
    public static class GlobalAuthenticator
    {
        private static readonly object Lock = new();

        private static bool _isAuthenticated;
        private static User? _user;
        private static Company? _company;
        private static Location? _location;
        private static Device? _device;
        private static Dictionary<short, bool> _userPermissions = new();

        public static bool IsAuthenticated
        {
            get
            {
                lock (Lock) return _isAuthenticated;
            }
            set
            {
                lock (Lock)
                {
                    if (_isAuthenticated == value) return;
                    _isAuthenticated = value;
                    AuthChanged?.Invoke(_isAuthenticated);
                }
            }
        }

        public static User? CurrentUser
        {
            get
            {
                lock (Lock) return _user;
            }
            set
            {
                lock (Lock)
                {
                    _user = value;
                    if (_user != null)
                        AuthChangedUser?.Invoke(_user);
                }
            }
        }

        public static Company? CurrentCompany
        {
            get
            {
                lock (Lock) return _company;
            }
            set
            {
                lock (Lock)
                {
                    _company = value;
                    if (_company != null)
                        OnChangedCompany?.Invoke(_company);
                }
            }
        }

        public static Location? CurrentLocation
        {
            get
            {
                lock (Lock) return _location;
            }
            set
            {
                lock (Lock)
                {
                    _location = value;
                    if (_location != null)
                        OnChangedLocation?.Invoke(_location);
                }
            }
        }

        public static Device? CurrentDevice
        {
            get
            {
                lock (Lock) return _device;
            }
            set
            {
                lock (Lock)
                {
                    _device = value;
                    if (_device != null)
                        OnChangedDevice?.Invoke(_device);
                }
            }
        }

        public static Dictionary<short, bool> UserPermissions
        {
            get
            {
                lock (Lock) return _userPermissions;
            }
            set
            {
                _userPermissions = value;
                OnPermissionChanged?.Invoke(_userPermissions);
            }
        }

        public static event Action<bool>? AuthChanged;
        public static event Action<User>? AuthChangedUser;
        public static event Action<Company>? OnChangedCompany;
        public static event Action<Location>? OnChangedLocation;
        public static event Action<Device>? OnChangedDevice;
        public static event Action<Dictionary<short ,bool>>? OnPermissionChanged;

        public static void Authenticate(User user, Dictionary<short, bool> userPermissions,Company company, Location location, Device device)
        {
            lock (Lock)
            {
                CurrentUser = user;
                CurrentCompany = company;
                CurrentLocation = location;
                CurrentDevice = device;
                UserPermissions = userPermissions;
                IsAuthenticated = true;
            }
        }

        public static void Logout()
        {
            lock (Lock)
            {
                _user = null;
                _company = null;
                _location = null;
                _device = null;
                IsAuthenticated = false;
            }
        }
    }
}