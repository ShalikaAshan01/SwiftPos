using System;
using Avalonia.Controls;

namespace PointOfSales.Utils;

public abstract class AuthorizedUserControl : UserControl
{
    public abstract string PermissionCode { get; }
    
    public AuthorizedUserControl Reload()
    {
        var type = GetType();

        // Make sure it has a parameterless constructor
        if (Activator.CreateInstance(type) is AuthorizedUserControl instance)
            return instance;

        throw new InvalidOperationException($"{type.Name} does not have a parameterless constructor.");
    }
}