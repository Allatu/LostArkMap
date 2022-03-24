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

        private const string lostArkmap = "https://lostarkmap.com/index.html";
        private const string pupunikaOld = "https://papunika.com/map/?z=overworld&l=us";
        private const string pupunikaNew = "https://papunika.com/world";

        //private const string url = "https://papunika.com/world/";
        //private const string url = "https://lostarkmap.com/index.html";

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!(e.Parameter is XboxGameBarWidget wid))
                return;

            SettingsInteractor.Instance.OpacityChanged += Instance_OpacityChanged;

            SettingsInteractor.Instance.Reload += Instance_Reload;

            SettingsInteractor.Instance.MapChanged += Instance_Reload;
               
            NavigateHome();

            SetOpacityFromSettings();

            widget = wid;

            widget.SettingsClicked += Widget_SettingsClicked;

            widget.CloseRequested += Widget_CloseRequested;

            wid.SettingsSupported = true;

        }

        private void Instance_Reload(object sender, EventArgs e)
        {
            NavigateHome();
        }

        private void Widget_CloseRequested(XboxGameBarWidget sender, XboxGameBarWidgetCloseRequestedEventArgs args)
        {
            widget.SettingsClicked -= Widget_SettingsClicked;
            widget.CloseRequested -= Widget_CloseRequested;
            SettingsInteractor.Instance.OpacityChanged -= Instance_OpacityChanged;
            SettingsInteractor.Instance.Reload -= Instance_Reload;
            SettingsInteractor.Instance.MapChanged -= Instance_Reload;
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

        private async void NavigateHome()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                webView.Source = SettingsInteractor.GetMapUri();
            });                       
        }

        private async void Widget_SettingsClicked(XboxGameBarWidget sender, object args)
        {
            await widget.ActivateSettingsAsync();
        }

        #endregion
    }
}
