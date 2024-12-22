using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUELampenOpdracht2.HUELampen.Domain.Models
{
    public class HUELight
    {
        
        public int Brightness { get; set; }
        public int? HUELightID { get; set; }
        public int Saturation { get; set; }
        public int Hue { get; set; }
        public bool IsOn { get; set; }

        public HUELight(int id, bool ison, int brightness, int saturation, int hue)
        {
            Brightness = brightness;
            HUELightID = id;
            Saturation = saturation;
            Hue = hue;
            IsOn = ison;    
        }
    }
}
