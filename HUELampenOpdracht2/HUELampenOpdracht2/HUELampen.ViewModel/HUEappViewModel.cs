using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text.Json;
using static Microsoft.Maui.ApplicationModel.Permissions;
using HUELampenOpdracht2.HUELampen.Domain.Models;
using HUELampenOpdracht2.HUELampen.Infrastructure;


namespace HUELampenOpdracht2.HUELampen.ViewModel
{

    public partial class HUEappViewModel : ObservableObject
    {
        private IBridgeController BridgeConnector;
        private ConnectionType _currentConnectionType = ConnectionType.None;

        public HUEappViewModel(IPreferences preferences, IBridgeController bridgeConnectorHueLights)
        {
            BridgeConnector = bridgeConnectorHueLights;
            lights = new ObservableCollection<HUELight>();
            IsBridgeButtonEnabled = true;
            IsEmulatorButtonEnabled = true;
        }

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
        private int _saturation;
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
                IsEmulatorButtonEnabled = true;
                return;
            }
            else
            {
                return;
            }
        }

        [RelayCommand]
        public async Task GetLights()
        {
            var result = await BridgeConnector.GetAllLightIDsAsync();
            if (result.Contains("error"))
            {
                IsBridgeButtonEnabled = true;
                IsEmulatorButtonEnabled = true;
                return;
            }

            JsonDocument jsondoc = JsonDocument.Parse(result);
            var root = jsondoc.RootElement;
            foreach (var lampElement in root.EnumerateObject())
            {

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
                if (!lights.Contains(hueLight))
                {
                    lights.Add(hueLight);
                }
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
                IsBridgeButtonEnabled = true;
                return;
            }
            else
            {
                return;
            }
        }
        

        [RelayCommand]
        public async Task SetLightColor()
        {
            int hue = Hue >= 0 && Hue <= 65535 ? Hue : 0;
            int saturation = Saturation >= 0 && Saturation <= 255 ? Saturation : 0;
            int brightness = Brightness >= 0 && Brightness <= 255 ? Brightness : 0;

            if (SelectedLight == null)
                return;

            var result = await BridgeConnector.SetLightColorAsync(SelectedLight.HUELightID.ToString(), SelectedLight.Hue, SelectedLight.Saturation, SelectedLight.Brightness, SelectedLight.IsOn);
            if (!result.Contains("error") && !result.Contains("Device is set to off"))
            {
                IsBridgeButtonEnabled = true;
                IsEmulatorButtonEnabled = true;
                return;
            }
        }
    }
}
