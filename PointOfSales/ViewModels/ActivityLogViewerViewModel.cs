using System;
using PointOfSales.Core.Entities.Security;

namespace PointOfSales.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class ActivityLogViewerViewModel : ObservableObject
{
    public ObservableCollection<ActivityLog> Logs { get; } = new();

    public ActivityLogViewerViewModel()
    {
        // Hardcoded sample
        Logs.Add(new ActivityLog
        {
            ActivityLogId = Guid.NewGuid(),
            UserId = 1,
            AccessedAt = DateTime.Now,
            IsSuccess = true,
            Message = "User logged in successfully.",
            PermissionId = 101,
            LocationId = 1,
            DeviceId = 1
        });

        Logs.Add(new ActivityLog
        {
            ActivityLogId = Guid.NewGuid(),
            UserId = 2,
            AccessedAt = DateTime.Now.AddMinutes(-5),
            IsSuccess = false,
            Message = "Unauthorized attempt to access admin panel.",
            PermissionId = 102,
            LocationId = 1,
            DeviceId = 1
        });
    }
}
