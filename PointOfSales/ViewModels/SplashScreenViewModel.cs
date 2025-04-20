using PointOfSales.Globalization.Resources;

namespace PointOfSales.ViewModels
{
    public class SplashScreenViewModel: ViewModelBase
    {
        private string _loadingText = Translations.LoadingLabel;

        public string LoadingText
        {
            get { return _loadingText; }
            set { SetProperty(ref _loadingText, value); }
        }
    }
}
