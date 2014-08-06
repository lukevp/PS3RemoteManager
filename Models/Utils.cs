using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS3RemoteManager
{
    public static class Utils
    {

        public static void SaveSettings(App app)
        {

            app.Log.Write("Serializing settings to JSON file...");
            try
            {
                using (StreamWriter sw = new StreamWriter("settings.json"))
                {
                    sw.Write(JsonConvert.SerializeObject(app.SettingsVM));
                }
                app.Log.Write("Settings saved to settings.json!");
            }
            catch
            {
                app.Log.Write("Unable to serialize settings!");
            }
        }
    }
}
