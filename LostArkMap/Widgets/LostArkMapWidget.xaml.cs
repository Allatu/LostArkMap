using Microsoft.Gaming.XboxGameBar;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LostArkMap.Widgets
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LostArkMapWidget : Page
    {
        #region Fields

        private XboxGameBarWidget widget = null;

        #endregion

        #region Constructors

        public LostArkMapWidget()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Constants

        private const string url = "https://papunika.com/map/";

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!(e.Parameter is XboxGameBarWidget wid))
                return;
               
            NavigateHome();

            widget = wid;

            wid.SettingsSupported = false;
        }

        private void NavigateHome()
        {
            var uri = new Uri(url);

            webView.Navigate(uri);
        }

        private void HomeButton_Clicked(object sender, RoutedEventArgs e)
        {
            NavigateHome();
        }

        #endregion

    }
}
