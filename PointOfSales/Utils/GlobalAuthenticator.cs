using System;
using PointOfSales.Core.Entities.Security;

namespace PointOfSales.Utils;

public static class GlobalAuthenticator
{
    private static readonly object Lock = new();
    private static bool _isAuthenticated;
    private static User? _user;

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
            lock (Lock) { _user = value; }
        }
    }

    public static event Action<bool>? AuthChanged;

    public static void Authenticate(User user)
    {
        lock (Lock)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
            IsAuthenticated = true;
        }
    }

    public static void Logout()
    {
        lock (Lock)
        {
            _user = null;
            IsAuthenticated = false;
        }
    }
}