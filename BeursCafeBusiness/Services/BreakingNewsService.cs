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
                    if(activeDrinks.Contains(drinkName))
                        return true;
                }
            }

            return false;
        }
    }
}
