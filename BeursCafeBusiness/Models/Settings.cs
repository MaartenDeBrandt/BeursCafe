using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BeursCafeBusiness.Models
{
    public class Settings
    {
        public int PriceUpdateIntervalInMinutes { get; set; } = 1;

        [JsonIgnore]
        public int PriceUpdateIntervalInMs { get { return PriceUpdateIntervalInMinutes * 60 * 1000; } }
        public int TimesToUpdateExpectedInInterval { get; set; } = 10;
        public double MaxPriceChangeTocompensateHighMarket { get; set; } = 0.3;
        public string FileLocation { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\BeursCafe";

        internal void LoadSettings(Settings settingsFile)
        {
            PriceUpdateIntervalInMinutes = settingsFile.PriceUpdateIntervalInMinutes;
            TimesToUpdateExpectedInInterval = settingsFile.TimesToUpdateExpectedInInterval;
            MaxPriceChangeTocompensateHighMarket = settingsFile.MaxPriceChangeTocompensateHighMarket;
            FileLocation = settingsFile.FileLocation;
        }
    }
}
