namespace PointOfSales.Core.Exceptions;

using System;

public class SwiftException : Exception
{
    public SwiftException(string messageTemplate, params object[] args)
        : base(string.Format(messageTemplate, args))
    {
    }

    public SwiftException(string messageTemplate, Exception innerException, params object[] args)
        : base(string.Format(messageTemplate, args), innerException)
    {
    }
}
