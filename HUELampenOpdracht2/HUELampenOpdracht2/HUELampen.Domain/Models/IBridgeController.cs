using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUELampenOpdracht2.HUELampen.Domain.Models
{
    public interface IBridgeController
    {
        Task<string> SendApiLinkAsync();
        void SetConnectionType(ConnectionType connection);
        Task<string> GetAllLightIDsAsync();
        Task<string> SetLightColorAsync(string id,int hue, int saturation,int brightness,bool isOn);
    }
}
