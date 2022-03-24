using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace LostArkMap
{
    public class SettingsInteractor
    {
        #region Constructor

        private SettingsInteractor()
        {
            dataContainer = ApplicationData.Current.LocalSettings;
        }

        #endregion

        #region Fields

        private readonly ApplicationDataContainer dataContainer;

        private bool overrideOpacity;

        #endregion

        #region Events

        public event EventHandler OpacityChanged;

        public event EventHandler MapChanged;

        public event EventHandler Reload;

        #endregion

        #region Constants

        private const string OverrideOpacitySettinsKey = "overrideOpacity";

        private const string OpacitySettinsKey = "Opacity";

        private const string MapKey = "usedMap";

        private const string pupunikaOldKey = "pupunikaOld";
        private const string pupunikaKey = "pupunika";
        private const string lostArkMapKey = "lostarkmap";

        #endregion

        #region Properties

        public Dictionary<string,Tuple<string ,string>> Maps = new Dictionary<string,Tuple<string, string>>()
        {
            { lostArkMapKey,new Tuple<string,string>("Lost Ark Map", "https://lostarkmap.com/index.html")},
            { pupunikaKey,new Tuple<string,string>("Pupunika", "https://papunika.com/world")},
            { pupunikaOldKey,new Tuple<string,string>("Pupunika Old (obsolete)", "https://papunika.com/map/?z=overworld&l=us")},
        };

        public static SettingsInteractor Instance { get; } = new SettingsInteractor();
               
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

        public string Map
        {
            get => GetSetting<string>(MapKey) ?? pupunikaOldKey;               
            set
            {
                SetSetting(MapKey, value);
                OnMapChanged();
            }
        }

        #endregion

        #region Methods
        
        public static Uri GetMapUri()
        { 
            var key = Instance.Map;

            var mapUrl = Instance.Maps[key];

            if(mapUrl == null)
                return null;

            return new Uri(mapUrl.Item2);
        }

        private T GetSetting<T>(string settingsKey)
        {
            var result = dataContainer.Values[settingsKey];

            if (!(result is T boolResult))
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

        private void OnMapChanged()
        {
            MapChanged?.Invoke(this, EventArgs.Empty);
        }


        public void OnReload()
        {
            Reload?.Invoke(this, EventArgs.Empty);
        }

        #endregion

    }
}
