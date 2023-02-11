using BeursCafeBusiness.Models;
using BeursCafeWPF.Models;
using BeursCafeWPF.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeursCafeWPF.Services
{
    public class DrinksPriceService
    {
        private readonly SettingsViewModel _settingsViewModel;

        string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public DrinksPriceService(SettingsViewModel settingsViewModel)
        {
            _settingsViewModel = settingsViewModel;
        }

        internal void UpdateExpectedDrinksUpdate(IEnumerable<Drink> allDrinks)
        {
            foreach (var drinks in allDrinks.GroupBy(el => el.DrinksType))
            {
                //Update all prices
                foreach (var drink in drinks)
                {
                    drink.UpdateExpectedPriceUpdate(drinks);
                }
            }

            //TODO deal with expected breaking news updates
        }

        internal void UpdateDrinksPrice(IEnumerable<Drink> allDrinks)
        {
            foreach (var drinks in allDrinks.GroupBy(el => el.DrinksType))
            {
                //Update all prices
                foreach (var drink in drinks)
                {
                    drink.UpdatePriceBasedOnSoldCount(drinks);
                }
                //Set all sold counts to 0
                foreach (var drink in drinks)
                {
                    drink.SoldCount = 0;
                    drink.PriceWillFall = false;
                    drink.PriceWillRise = false;
                }

                //TODO add logic to deal with BreakingNews updates. This could make certain beers more expensive or cheaper.
                CompensateForMarketImbalance(drinks);
            }
        }

        //Try to get the total sum of prices the same as the normal prices. Dont drop to fast.
        private void CompensateForMarketImbalance(IEnumerable<Drink> drinks)
        {
            double totalDefaultPrices = drinks.Sum(d => d.DefaultPrice);
            double totalCurrentPrices = drinks.Sum(d => d.Price);
            double difference = totalDefaultPrices - totalCurrentPrices;

            var numberOfRandomDrinksToUpdate = Math.Min(Math.Abs(difference) / 0.1, _settingsViewModel.MaxPriceChangeTocompensateHighMarket / 0.1);

            if (difference < -0.1)
            {
                UpdateRandomDrinks(drinks, numberOfRandomDrinksToUpdate, -0.1);
            }

            if (difference > 0.1)
            {
                UpdateRandomDrinks(drinks, numberOfRandomDrinksToUpdate, 0.1);
            }
        }

        internal void UpdateRandomDrinks(IEnumerable<Drink> drinks, double numberOfDringsChanged, double change)
        {
            Random random = new Random();
            var drinksList = drinks.ToList();

            for (int i = 0; i < numberOfDringsChanged; i++)
            {
                int randomDrinkInt = random.Next(0, drinksList.Count);
                var newPrice = Math.Round(drinksList[randomDrinkInt].Price + change, 1);
                drinksList[randomDrinkInt].Price = newPrice;
            }
        }

        internal void SellRandomDrink(IEnumerable<Drink> drinks)
        {
            var drinksList = drinks.ToList();

            Random random = new Random();
            int randomDrinkInt = random.Next(0, drinksList.Count);
            drinksList[randomDrinkInt].SoldCount += random.Next(1, 3);
        }

        internal ObservableCollection<Drink> LoadDrinks()
        {
            string fileName = DesktopPath + "drinks.json";
            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<ObservableCollection<Drink>>(json);
            }
            else
            {
                return new ObservableCollection<Drink>
                {
                    new Drink { Name = "Jupiler", DrinksType = DrinksTypes.beer, MinimumPrice = 1.0, MaximumPrice = 3.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Hoegaarden", DrinksType = DrinksTypes.beer, MinimumPrice = 1.0, MaximumPrice = 3.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Cola", DrinksType = DrinksTypes.softdrink, MinimumPrice = 1.0, MaximumPrice = 3.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Water", DrinksType = DrinksTypes.softdrink, MinimumPrice = 1.0, MaximumPrice = 3.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Icetea", DrinksType = DrinksTypes.softdrink, MinimumPrice = 1.0, MaximumPrice = 3.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Duvel", DrinksType = DrinksTypes.beer, MinimumPrice = 2.0, MaximumPrice = 6.0 , DefaultPrice = 3},
                    new Drink { Name = "Karmeliet", DrinksType = DrinksTypes.beer, MinimumPrice = 2.0, MaximumPrice = 6.0 , DefaultPrice = 3},
                    new Drink { Name = "Kasteelbier rouge", DrinksType = DrinksTypes.beer, MinimumPrice = 2.0, MaximumPrice = 6.0 , DefaultPrice = 3},
                    new Drink { Name = "Geuze", DrinksType = DrinksTypes.beer, MinimumPrice = 2.0, MaximumPrice = 6.0 , DefaultPrice = 3},
                    new Drink { Name = "Satan White", DrinksType = DrinksTypes.beer, MinimumPrice = 2.0, MaximumPrice = 6.0 , DefaultPrice = 3},
                    new Drink { Name = "Carolus", DrinksType = DrinksTypes.beer, MinimumPrice = 2.0, MaximumPrice = 6.0 , DefaultPrice = 3}

                };

            }
        }
    }
}
