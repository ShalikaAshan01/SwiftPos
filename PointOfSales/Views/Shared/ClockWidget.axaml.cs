using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Threading;

namespace PointOfSales.Views.Shared
{
    public partial class ClockWidget : UserControl
    {
        private readonly DispatcherTimer _timer;

        public ClockWidget()
        {
            InitializeComponent();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timer.Tick += (_, _) =>
            {
                var now = DateTime.Now;
                TimeText.Text = now.ToString("hh:mm:ss", CultureInfo.CurrentCulture);
                DateText.Text = now.ToString("dddd, MMMM d, yyyy", CultureInfo.CurrentCulture);
            };

            _timer.Start();
        }
    }
}