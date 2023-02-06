using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeursCafeWPF.ViewModels
{
    public class SettingsViewModel
    {
        public int PriceUpdateIntervalInMinutes { get; set; } = 1;
        public int PriceUpdateIntervalInMs { get { return PriceUpdateIntervalInMinutes * 60 * 1000; } }
        public int TimesToUpdateExpectedInInterval { get; set; } = 10;
        public double MaxPriceChangeTocompensateHighMarket { get; set; } = 0.3;
    }
}
