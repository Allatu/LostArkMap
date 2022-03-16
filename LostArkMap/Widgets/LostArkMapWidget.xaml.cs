using Microsoft.Gaming.XboxGameBar;
using System;
using System.Diagnostics;
using System.IO;
using Windows.UI.Core;
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

        private const string url = "https://papunika.com/world/";

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!(e.Parameter is XboxGameBarWidget wid))
                return;

            SettingsInteractor.Instance.OpacityChanged += Instance_OpacityChanged;
               
            NavigateHome();

            SetOpacityFromSettings();

            widget = wid;

            widget.SettingsClicked += Widget_SettingsClicked;

            widget.CloseRequested += Widget_CloseRequested;

            wid.SettingsSupported = true;

        }

        private void Widget_CloseRequested(XboxGameBarWidget sender, XboxGameBarWidgetCloseRequestedEventArgs args)
        {
            widget.SettingsClicked -= Widget_SettingsClicked;
            widget.CloseRequested -= Widget_CloseRequested;
            SettingsInteractor.Instance.OpacityChanged -= Instance_OpacityChanged;
        }

        private void Instance_OpacityChanged(object sender, EventArgs e)
        {
            SetOpacityFromSettings();                      
        }

        private async void SetOpacityFromSettings()
        {
            if (!SettingsInteractor.Instance.OverrideOpacity)
                return;

            var op = SettingsInteractor.Instance.Opacity;

            var rounded = Math.Round(op, 2, MidpointRounding.AwayFromZero);

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                webView.Opacity = rounded;
            });
        }

        private void NavigateHome()
        {
            var uri = new Uri(url);

            webView.Navigate(uri);
        }

        private async void Widget_SettingsClicked(XboxGameBarWidget sender, object args)
        {
            await widget.ActivateSettingsAsync();
        }

        #endregion
    }
}
