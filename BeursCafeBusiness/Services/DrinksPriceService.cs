using BeursCafeBusiness.Models;
using BeursCafeBusiness.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeursCafeBusiness.Services
{
    public class DrinksPriceService
    {
        private readonly FileService _fileService;
        private readonly Settings _settings;

        public DrinksPriceService(FileService fileService, Settings settings)
        {
            _fileService = fileService;

            fileService.LoadSettings(settings);
            _settings = settings;

        }

        public void UpdateExpectedDrinksUpdate(IEnumerable<Drink> allDrinks)
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

        public void UpdateDrinksPrice(IEnumerable<Drink> allDrinks)
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
            _fileService.SaveDrinks(allDrinks);
            _fileService.SaveSettings(_settings);
        }

        //Try to get the total sum of prices the same as the normal prices. Dont drop to fast.
        private void CompensateForMarketImbalance(IEnumerable<Drink> drinks)
        {
            double totalDefaultPrices = drinks.Sum(d => d.DefaultPrice);
            double totalCurrentPrices = drinks.Sum(d => d.Price);
            double difference = totalDefaultPrices - totalCurrentPrices;

            var numberOfRandomDrinksToUpdate = Math.Min(Math.Abs(difference) / 0.1, _settings.MaxPriceChangeTocompensateHighMarket / 0.1);

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

        public void SellRandomDrink(IEnumerable<Drink> drinks, double numberOfDringsChanged)
        {
            var drinksList = drinks.ToList();

            Random random = new Random();
            for (int i = 0; i < numberOfDringsChanged; i++)
            {
                int randomDrinkInt = random.Next(0, drinksList.Count);
                drinksList[randomDrinkInt].SoldCount += random.Next(1, 3);
            }
        }

        public void BeursCrash(IEnumerable<Drink> drinks)
        {
            drinks.ToList().ForEach(el => el.Price = el.MinimumPrice);
        }

        public void HideDrink(Drink drink)
        {
            drink.Enabled = !drink.Enabled;
        }

        public void FinishOrder(IEnumerable<Drink> drinks)
        {
            drinks.ToList().ForEach(el => el.SoldCount = el.SoldCurrentOrder);
            ResetOrder(drinks);
        }

        public void ResetOrder(IEnumerable<Drink> drinks)
        {
            drinks.ToList().ForEach(el => el.SoldCurrentOrder = 0);
        }
    }
}
