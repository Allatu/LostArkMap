using System;
using Windows.Storage;

namespace LostArkMap
{
    public class SettingsInteractor
    {
        private readonly ApplicationDataContainer dataContainer;
        private bool overrideOpacity;

        private SettingsInteractor()
        {
            dataContainer = ApplicationData.Current.LocalSettings;
        }

        public event EventHandler OpacityChanged;


        public static SettingsInteractor Instance { get; } = new SettingsInteractor();

        private const string OverrideOpacitySettinsKey = "overrideOpacity";
        private const string OpacitySettinsKey = "Opacity";


        public bool OverrideOpacity
        {
            get => GetSetting<bool>(OverrideOpacitySettinsKey); 
            set => SetSetting(OverrideOpacitySettinsKey, value); 
        }

        public double Opacity
        {
            get => GetSetting<double>(OpacitySettinsKey);
            set
            {
                SetSetting(OpacitySettinsKey, value);
                OnOpacityChanged();
            }
        }

        private T GetSetting<T>(string settingsKey)
        {
            var result = dataContainer.Values[settingsKey];

            if(!(result is T boolResult))
                return default;

            return boolResult; 
        }

        private void SetSetting<T>(string settingsKey, T value)
        {
            dataContainer.Values[settingsKey] = value;
        }

        private void OnOpacityChanged()
        {
            OpacityChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
