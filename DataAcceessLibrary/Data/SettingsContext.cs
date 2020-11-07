using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcceessLibrary.Data
{
   public static class SettingsContext
    {

        private static Settings _settings { get; set; }

        public static async void GetSettingsInformation()
        {
            //StorageFile settingsFile = await ApplicationData.Current.LocalFolder.GetFileAsync("settings.json");
            //var json= await FileIO.ReadTextAsync(settingsFile);
            //var settings = JsonConvert.DeserializeObject<Settings>(json);

            var settingsFile = "{\"status\":   [\"new\", \"active\", \"closed\"],   \"maxItemsCount\": 4}";
           
            _settings = JsonConvert.DeserializeObject<Settings>(settingsFile);
        }


        public static async Task<IEnumerable<string>> GetStatus()
        { // hämta ten fil 
          // packa upp json code i file
          // var json = "{\"status\":   [\"new\", \"active\", \"closed\"],   \"maxItemsCount\": 4}";
          // var set  = JsonConvert.DeserializeObject<Settings>(settingsFile);

            var list = new List<string>();
            foreach (var status in _settings.status)
            {
                list.Add(status);
            }           
            //lis.Add("active");
            //list.Add("closed");          

            return list;
        }

        public static int GetMaxItemsCount()
        {
            return _settings.maxItemsCount;
        }

    }

    public class Settings
    {// "status ":   [ "new ",  "active ",  "closed "]//paste json as class
        public string[] status { get; set; } //array
        public int maxItemsCount { get; set; }
    }
}
