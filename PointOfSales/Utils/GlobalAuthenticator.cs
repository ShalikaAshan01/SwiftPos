using System;

namespace PointOfSales.Utils;

public class GlobalAuthenticator
{
    private static bool _isAuthenticated;
    public static bool IsAuthenticated
    {
        get => _isAuthenticated;
        set
        {
            if (_isAuthenticated == value) return;
            _isAuthenticated = value;
            AuthChanged?.Invoke(_isAuthenticated);
        }
    }
    
    public static event Action<bool>? AuthChanged;
}