using HUELampenOpdracht2.HUELampen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HUELampenOpdracht2.HUELampen.Infrastructure
{
    public class HueBridgeController : IBridgeController
    {
        private static HttpClient _httpClient = null;
        private IPreferences preferences;
        private FetchUsername fetchUsername;

        public HueBridgeController(IPreferences preferences, FetchUsername fetchUsername, HttpClient httpClient)
        {
            this.preferences = preferences;
            this.fetchUsername = fetchUsername;
            _httpClient = httpClient;
        }

        public async Task<string> GetAllLightIDsAsync()
        {
            Debug.WriteLine("Fetching HUE Lights");
            try
            {
                var response = await _httpClient.GetAsync($"{preferences.Get("username", string.Empty)}/lights");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
                return "Httpclient request failed";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                return "Unmarked exception occured";
            } 
        }
        public async Task<string> SetLightColorAsync(string id, int hue, int saturation, int brightness, bool isOn)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{preferences.Get("username", string.Empty)}/lights/{id}/state", new
                {
                    on = isOn,
                    sat = saturation,
                    bri = brightness,
                    hue = hue,
                });
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                return json;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
                return "SetLight Method http request failed";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return "SetLight Method error occured";
            }
        }

        public async Task<string> SendApiLinkAsync()
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("", new
                {
                    devicetype = "my_hue_app#iphone collin zonder iphone"
                });
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(json);

                    fetchUsername.setUsername(json);
                    return json;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(errorContent);
                    return errorContent;
                }
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine(httpEx.Message);
                return " error";
            }
            catch (TimeoutException timeoutEx)
            {
                Debug.WriteLine($"Time-out fout: {timeoutEx.Message}");
                return " error";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return " error";
            }
        }

        public void SetConnectionType(ConnectionType connection)
        {
            if (_httpClient.BaseAddress == null)
            {
                if (connection == ConnectionType.Emulator)
                {
                    _httpClient.BaseAddress = new Uri("http://localhost/api/");
                }
                else if (connection == ConnectionType.HUELight)
                {
                    _httpClient.BaseAddress = new Uri("http://192.168.1.179/api/");
                }
            }
            else
            {
                Console.WriteLine("BaseAdrress is already set to: " + _httpClient.BaseAddress);
            }
        }

        
    }
}
