using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text.Json;
using TdmdHueApp.Domain.Model;
using HUELampen.Infrastructure;
using static Microsoft.Maui.ApplicationModel.Permissions;
using HUELampenOpdracht2.HUELampen.Domain.Models;


namespace TdmdHueApp.Domain.Services
{
    public partial class ViewModel : ObservableObject
    {
        private IBridgeConnector BridgeConnector;
        private ConnectionType _currentConnectionType = ConnectionType.None;

        public ViewModel(IPreferences preferences, IBridgeConnector bridgeConnectorHueLights)
        {
            BridgeConnector = bridgeConnectorHueLights;
            lights = new ObservableCollection<HUELight>();
            IsBridgeButtonEnabled = true;
            IsEmulatorButtonEnabled = true;
        }
        [ObservableProperty]
        private string _statusApp;
        [ObservableProperty]
        private bool _isEmulatorButtonEnabled;
        [ObservableProperty]
        private bool _isBridgeButtonEnabled;
        [ObservableProperty]
        private HUELight _selectedLight;
        [ObservableProperty]
        private string _lightId;
        [ObservableProperty]
        private bool _isLightOn;
        [ObservableProperty]
        private int _hue;
        [ObservableProperty]
        private int _opacity;
        [ObservableProperty]
        private int _brightness;
        [ObservableProperty]
        public ObservableCollection<HUELight> lights;

        [RelayCommand]
        public void SetSelectedLight(HUELight light)
        {
            SelectedLight = light;
        }

        [RelayCommand]
        public async Task SendApiLink()
        {
            _currentConnectionType = ConnectionType.Emulator;
            BridgeConnector.SetConnectionType(_currentConnectionType);

            IsEmulatorButtonEnabled = false;
            IsBridgeButtonEnabled = false;

            var result = await BridgeConnector.SendApiLinkAsync();
            if (result.Contains("error"))
            {
                StatusApp = "unable to make Connection " + result;
                IsEmulatorButtonEnabled = true;
                return;
            }
            else
            {
                StatusApp = "Made a connection " + result;
                return;
            }

        }
        [RelayCommand]
        public async Task SendApiBridge()
        {
            _currentConnectionType = ConnectionType.HUELight;
            BridgeConnector.SetConnectionType(_currentConnectionType);

            IsEmulatorButtonEnabled = false;
            IsBridgeButtonEnabled = false;

            var result = await BridgeConnector.SendApiLinkAsync();
            if (result.Contains("error"))
            {
                StatusApp = "unable to make Connection " + result;
                IsBridgeButtonEnabled = true;
                return;
            }
            else
            {
                StatusApp = "Made a connection " + result;
                return;
            }

        }
        [RelayCommand]
        public async Task GetLights()
        {
            var result = await BridgeConnector.GetAllLightIDsAsync();
            if (result.Contains("error"))
            {
                StatusApp = "retry Connecting " + result;
                IsBridgeButtonEnabled = true;
                IsEmulatorButtonEnabled = true;
                return;
            }
            Debug.WriteLine("in lghts");
            JsonDocument jsondoc = JsonDocument.Parse(result);
            var root = jsondoc.RootElement;
            Debug.WriteLine("voor array: " + root);
            Debug.WriteLine("geparsed");

            foreach (var lampElement in root.EnumerateObject())
            {
                Debug.WriteLine("in ");
                var name = lampElement.Name;
                Debug.WriteLine($"{name}");
                var info = lampElement.Value;
                var baseProperty = info.GetProperty("state");
                var LampId = int.Parse(name);
                var Saturation = baseProperty.GetProperty("sat").GetInt32();
                var IsOn = baseProperty.GetProperty("on").GetBoolean();
                var Brightness = baseProperty.GetProperty("bri").GetInt32();
                var Hue = baseProperty.GetProperty("hue").GetInt32();
                var hueLight = new HUELight(LampId, IsOn, Brightness, Saturation, Hue);
                Debug.WriteLine("Lampieess");
                if (!lights.Contains(hueLight))
                {
                    lights.Add(hueLight);
                }
            }

        }

        [RelayCommand]
        public async Task SetLightColor()
        {
            int hue = Hue >= 0 && Hue <= 65535 ? Hue : 0;
            int opacity = Opacity >= 0 && Opacity <= 255 ? Opacity : 0;
            int brightness = Brightness >= 0 && Brightness <= 255 ? Brightness : 0;

            if (SelectedLight == null)
                return;


            var result = await BridgeConnector.SetLightColorAsync(SelectedLight.HUELightID.ToString(), SelectedLight.Hue, SelectedLight.Opacity, SelectedLight.Brightness, SelectedLight.IsOn);
            if (result.Contains("error") && result.Contains("Device is set to off"))
            {
                StatusApp = "light is turned off";
                return;
            }
            else if (result.Contains("error"))
            {
                StatusApp = "Retry Connecting " + result;
                IsBridgeButtonEnabled = true;
                IsEmulatorButtonEnabled = true;
                return;
            }

        }



    }
}
