using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUELampenOpdracht2.HUELampen.Domain.Models
{
    public class HUELight
    {
        public int? HUELightID { get; set; }

        public bool IsOn { get; set; }
        public int Brightness { get; set; }
        public int Opacity { get; set; }
        public int Hue { get; set; }

        public HUELight(int id, bool ison, int brightness, int opacity, int hue)
        {
            HUELightID = id;
            IsOn = ison;
            Brightness = brightness;
            Opacity = opacity;
            Hue = hue;
        }
    }
}
