using BeursCafeBusiness.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BeursCafeBusiness.Services
{
    public class BreakingNewsService
    {
        public BreakingNewsService()
        {
        }

        public string BeursCrash()
        {
            return "Beurscrash: Alles aan goedkope prijzen!";
        }

        public bool CheckIfBreakingNewsStillRelevant(IEnumerable<Drink> activeDrinks, BreakingNewsModel breakingNews)
        {
            if (breakingNews != null && breakingNews.Drinks != null)
            {
                foreach (var drinkName in breakingNews.Drinks)
                {
                    var drink = activeDrinks.FirstOrDefault(drinkName);
                    if (drink != null)
                    {
                        if (breakingNews.PriceUpdate < 0 && drink.Price == drink.MinimumPrice && breakingNews.Drinks.Count > 1)
                        {
                            return false;
                        }
                        if (breakingNews.PriceUpdate > 0 && drink.Price == drink.MaximumPrice && breakingNews.Drinks.Count > 1)
                        {
                            return false;
                        }

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
