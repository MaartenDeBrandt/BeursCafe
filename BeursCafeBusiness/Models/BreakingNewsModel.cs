using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BeursCafeBusiness.Models
{
    public class BreakingNewsModel
    {
        [JsonIgnore]
        public List<Drink> Drinks { get; set; }
        public string BreakingNews { get; set; }
        public string DrinkNames { get; set; }

        /// <summary>
        /// Price update plus of min
        /// </summary>
        public double PriceUpdate { get; set; }
        public bool AlreadyRun { get; set; }

        public override string ToString()
        {
            return BreakingNews;
        }
    }
}
