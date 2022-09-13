using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyOtp
{
    public class LocalSettings
    {
        public ICollection<LocalOtpDefinition> Definitions { get; set; } = new List<LocalOtpDefinition>();

        public static string BasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KeyOtp");
        public static string SettingsPath => Path.Combine(BasePath, "settings.json");

        public LocalSettings()
        {
        }

        public static void Save(LocalSettings settings)
        {
            Directory.CreateDirectory(BasePath);
            File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(settings));
        }

        public static LocalSettings Load()
        {
            Directory.CreateDirectory(BasePath);
            if (File.Exists(SettingsPath))
                return JsonConvert.DeserializeObject<LocalSettings>(File.ReadAllText(SettingsPath));
            else
            {
                var settings = new LocalSettings();
                Save(settings);
                return settings;
            }
        }
    }
}
