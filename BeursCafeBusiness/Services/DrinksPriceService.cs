using BeursCafeBusiness.Models;
using BeursCafeBusiness.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static BeursCafeBusiness.Models.Drink;

namespace BeursCafeBusiness.Services
{
    public class DrinksPriceService
    {
        private readonly FileService _fileService;
        private readonly Settings _settings;
        
        private Dictionary<Drink.DrinkTypes, List<Drink>> _randomDrinksToUpdate = new Dictionary<Drink.DrinkTypes, List<Drink>>();
        private Dictionary<Drink.DrinkTypes, bool> _isMarketToHigh = new Dictionary<Drink.DrinkTypes, bool>();

        Random random = new Random();
        public DrinksPriceService(FileService fileService, Settings settings)
        {
            _fileService = fileService;

            fileService.LoadSettings(settings);
            _settings = settings;

            InitalizeRandomDrinksCache();
        }

        private void InitalizeRandomDrinksCache()
        {
            foreach (Drink.DrinkTypes drinkType in Enum.GetValues(typeof(Drink.DrinkTypes)))
            {
                _randomDrinksToUpdate.Add(drinkType, new List<Drink>());
                _isMarketToHigh.Add(drinkType, false);
            }
        }

        public void UpdateExpectedDrinksUpdate(IEnumerable<Drink> allDrinks, BreakingNewsModel breakingNews)
        {
            try
            {
                foreach (var drinks in allDrinks.GroupBy(el => el.DrinksType))
                {
                    var drinkType = drinks.FirstOrDefault().DrinksType;

                    //Update all prices
                    foreach (var drink in drinks)
                    {
                        drink.UpdateExpectedPriceUpdate(drinks, breakingNews);
                    }

                    SelectRandomDrinkToCompensateForMarketImbalance(drinkType, drinks);
                    CompensateForMarketImbalance(drinkType);
                }
            }
            catch (Exception)
            {
            }

        }

        public void UpdateDrinksPrice(IEnumerable<Drink> drinks)
        {
            foreach (var drinkGroup in drinks.GroupBy(el => el.DrinksType))
            {
                //Update all prices
                foreach (var drink in drinkGroup)
                {
                    drink.UpdatePrice();
                    _randomDrinksToUpdate[drinkGroup.FirstOrDefault().DrinksType].Clear();
                }

                //Set all sold counts to 0
                foreach (var drink in drinkGroup)
                {
                    drink.SoldCount = 0;
                }
            }

            _fileService.SaveSettings(_settings);
        }

        public void SelectRandomDrinkToCompensateForMarketImbalance(Drink.DrinkTypes drinksType, IEnumerable<Drink> drinks)
        {
            try
            {
                double totalDefaultPrices = drinks.Sum(d => d.DefaultPrice);
                double totalCurrentPrices = drinks.Sum(d => d.NewPrice);
                double difference = totalDefaultPrices - totalCurrentPrices ;

                var toCompensate = _settings.MaxPriceChangeTocompensateHighMarket;

                if (drinksType == DrinkTypes.frisdrank)
                    toCompensate = toCompensate / 2;

                var numberOfRandomDrinksToUpdate = (int)Math.Min(Math.Abs(difference) / 0.3, toCompensate / 0.3);

                _isMarketToHigh[drinksType] = difference < 0;

                if (_randomDrinksToUpdate[drinksType].Count == numberOfRandomDrinksToUpdate)
                    return;

                var drinksList = drinks.ToList();

                if (_isMarketToHigh[drinksType])
                    drinksList = drinks.Where(el => el.NewPrice > el.DefaultPrice).ToList();
                else
                    drinksList = drinks.Where(el => el.NewPrice < el.DefaultPrice).ToList();

                List<Drink> randomDrinksToUpdate = new List<Drink>();
                for (int i = 1; i <= numberOfRandomDrinksToUpdate; i++)
                {
                    int randomDrinkInt = random.Next(0, drinksList.Count);
                    randomDrinksToUpdate.Add(drinksList[randomDrinkInt]);
                }
                _randomDrinksToUpdate[drinksType] = randomDrinksToUpdate;

            }
            catch (Exception)
            {
            }
        }


        //Try to get the total sum of prices the same as the normal prices. Dont drop to fast.
        private void CompensateForMarketImbalance(Drink.DrinkTypes drinksType)
        {
            try
            {
                if (_isMarketToHigh[drinksType])
                {
                    _randomDrinksToUpdate[drinksType].ForEach(drink => { drink.NewPrice -= 0.3; });
                }
                else
                {
                    _randomDrinksToUpdate[drinksType].ForEach(drink => { drink.NewPrice += 0.3; });
                }

            }
            catch (Exception)
            {
            }
        }

        internal Drink UpdateRandomDrinks(IEnumerable<Drink> drinks, double numberOfDringsChanged, double change)
        {
            Drink randomDrink = null;
            var drinksList = drinks.ToList();

            for (int i = 0; i < numberOfDringsChanged; i++)
            {
                int randomDrinkInt = random.Next(0, drinksList.Count);
                var newPrice = Math.Round(drinksList[randomDrinkInt].Price + change, 1);
                randomDrink = drinksList[randomDrinkInt];

                if (randomDrink.Price != newPrice)
                    Console.WriteLine($"{randomDrink.Name} OldPrice: {randomDrink.Price} NewPrice:{newPrice} Difference:{newPrice - randomDrink.Price} ");
                randomDrink.Price = newPrice;
            }
            return randomDrink;
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
            drinks.ToList().ForEach(el => el.NewPrice = el.MinimumPrice);
        }

        public void HideDrink(Drink drink)
        {
            drink.Enabled = !drink.Enabled;
        }

        public void FinishOrder(IEnumerable<Drink> drinks)
        {
            drinks.ToList().ForEach(el => el.SoldCount += el.SoldCurrentOrder);
            ResetOrder(drinks);
        }

        public void ResetOrder(IEnumerable<Drink> drinks)
        {
            drinks.ToList().ForEach(el => el.SoldCurrentOrder = 0);
        }

        public BreakingNewsModel PromoOrveloBonVieux(IEnumerable<Drink> drinks)
        {
            var promos = new List<BreakingNewsModel>()
            {
                new BreakingNewsModel { DrinkNames = "Bons vieux 75cl", PriceUpdate = -10, BreakingNews = "Promotie: Bons Vieux binnenkort goedkoper - Mis deze kans niet om te genieten van dit klassieke bier voor een geweldige prijs!" },
                new BreakingNewsModel { DrinkNames = "Orvelo 75cl", PriceUpdate = -10, BreakingNews = "Promotie: Orvelo binnenkort in prijs verlaagd - Proef de smaakvolle sensatie zonder de bank te breken!" },
                new BreakingNewsModel { DrinkNames = "Kwak blond 75cl,Kwak Blond", PriceUpdate = -10, BreakingNews = "Promotie: Kwak Blond binnenkort in de aanbieding - Ontdek dit karaktervolle blond bier voor een scherpe prijs!"},
                new BreakingNewsModel { DrinkNames = "Kwak amber 75cl,Kwak Amber", PriceUpdate = -10, BreakingNews = "Promotie: Kwak Amber binnenkort goedkoper - Geniet van de rijke smaak van dit amberkleurige bier tegen een gereduceerde prijs!"},
                new BreakingNewsModel { DrinkNames = "Aymom 75cl", PriceUpdate = -10, BreakingNews = "Promotie: Aymon binnenkort in de aanbieding - Profiteer van een beperkte periode van lagere prijzen voor dit heerlijke brouwsel!"},
            };

            foreach (var item in promos)
            {
                var drinkNames = item.DrinkNames.Split(',');
                item.Drinks = drinks.Where(el => drinkNames.Contains(el.Name)).ToList();
            }

            Random random = new Random();
            return promos[random.Next(promos.Count)];
            
        }

    }
}

