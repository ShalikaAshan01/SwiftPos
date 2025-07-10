using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using PointOfSales.Core.Constants;
using PointOfSales.Utils;
using PointOfSales.ViewModels;
using PointOfSales.Views.UserControls.BackOffice.Logs;

namespace PointOfSales.Views.UserControls.BackOffice;

public partial class BackOffice : AuthorizedUserControl
{
    private readonly ContentControl? _mainContent;
    private readonly TextBlock? _titleBlock;

    public Dictionary<string, List<SideBarItemModel>> Sections { get; } = new()
    {
        ["GENERAL"] =
        [
            new SideBarItemModel("fa-solid fa-chart-line", "Dashboard", new UserControl()),
            new SideBarItemModel("fa-solid fa-bell", "Notifications",new UserControl(), false, 4)
        ],
        ["SISYPHUS VENTURES"] =
        [
            new SideBarItemModel("fa-solid fa-users-cog", "User management", new UserControl(),true),
        ],
        ["System"] = new List<SideBarItemModel>() // Footer
        {
            new("fa-solid fa-cog", "Settings",new UserControl()),
            new("fa-solid fa-clipboard-list", "Activity Logs",new ActivityLogViewer()),
            new("fa-solid fa-bug", "System Logs",new LogViewer()),
        }
    };


    public BackOffice()
    {
        InitializeComponent();
        DataContext = this;
        _mainContent = this.FindControl<ContentControl>("MainContent");
        _titleBlock = this.FindControl<TextBlock>("Title");
        SetUi(Sections.Values
            .SelectMany(section => section)
            .First(item => item.IsActive));
    }


    public override string PermissionCode => PermissionCodes.AccessBackOffice;

    private void SideBar_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Border { Tag: SideBarItemModel item }) return;
        ResetAllSideBarItemStates();
        item.IsActive = true;
        SetUi(item);
    }

    private void SetUi(SideBarItemModel item)
    {
        if (_mainContent != null)
        {
            _mainContent.Content = item.Content;
        }

        if (_titleBlock != null)
        {
            _titleBlock.Text = item.Title;
        }
    }

    private void ResetAllSideBarItemStates()
    {
        foreach (var item in Sections.Values.SelectMany(section => section))
        {
            if (item.IsActive)
                item.IsActive = false;
        }
    }
}