using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LostArkMap.Widgets
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LostArkMapSettings : Page
    {

        public LostArkMapSettings()
        {
            this.InitializeComponent();
        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);


            overrideOpacityCheckBox.IsChecked = SettingsInteractor.Instance.OverrideOpacity;

            var opacity = 1d;

            if (!SettingsInteractor.Instance.OverrideOpacity)
                opacitySlider.Value = opacity;
            else            
                opacitySlider.Value = SettingsInteractor.Instance.Opacity > 1d ? 1d : SettingsInteractor.Instance.Opacity;           
        }

        private void opacitySlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {

            if (!(sender is Slider slider))
                return;

            SettingsInteractor.Instance.Opacity = slider.Value;

        }

        private void overrideOpacityCheckBox_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SettingsInteractor.Instance.OverrideOpacity = true;

        }

        private void overrideOpacityCheckBox_Unchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            
            opacitySlider.Value = 1;

            SettingsInteractor.Instance.OverrideOpacity = false;
            

        }
    }
}
