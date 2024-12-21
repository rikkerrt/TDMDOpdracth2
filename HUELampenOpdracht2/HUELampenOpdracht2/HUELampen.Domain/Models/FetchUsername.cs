using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HUELampenOpdracht2.HUELampen.Domain.Models
{
    public class FetchUsername
    {
        private IPreferences preferences;

        public FetchUsername(IPreferences preferences)
        {
            this.preferences = preferences;
        }

        public void setUsername (string jsonAsString)
        {
            JsonDocument document = JsonDocument.Parse (jsonAsString);
            var rootArray = document.RootElement;
            var rootObject = rootArray[0];
            var username = rootObject.GetProperty("success").GetProperty("username").GetString();

            Debug.WriteLine (username);
            preferences.Set("username", username);
        }
    }
}
