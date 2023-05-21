using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BeursCafeBusiness.Models.Drink;

namespace BeursCafeBusiness.Models
{
    public class Drink : INotifyPropertyChanged
    {
        public enum DrinkTypes
        {
            bier, frisdrank, grotefles
        }
        private string name;
        private double minimumPrice;
        private double maximumPrice;
        private double defaultPrice;
        private double price;
        private int soldCount;
        private double newPrice;

        public event PropertyChangedEventHandler PropertyChanged;
        public Drink()
        {
            Enabled = true;
            newPrice = Price;
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public DrinkTypes DrinksType { get; set; }
        public double MinimumPrice
        {
            get { return minimumPrice; }
            set
            {
                minimumPrice = value;
                OnPropertyChanged("MinimumPrice");
            }
        }

        public double MaximumPrice
        {
            get { return maximumPrice; }
            set
            {
                maximumPrice = value;
                OnPropertyChanged("MaximumPrice");
            }
        }

        public double DefaultPrice
        {
            get { return defaultPrice; }
            set
            {
                defaultPrice = value;
                Price = value;
                OnPropertyChanged("DefaultPrice");
            }
        }

        public double Price
        {
            get
            {
                return Math.Round(price, 1);
            }

            set
            {
                if (value < MinimumPrice)
                {
                    price = MinimumPrice;
                }
                else if (value > MaximumPrice)
                {
                    price = MaximumPrice;
                }
                else
                {
                    price = value;
                }
                newPrice = Price;
                OnPropertyChanged("Price");
            }
        }

        public double NewPrice 
        { 
            get => newPrice;
            set
            {
                if (value < MinimumPrice)
                {
                    newPrice = MinimumPrice;
                }
                else if (value > MaximumPrice)
                {
                    newPrice = MaximumPrice;
                }
                else
                {
                    newPrice = Math.Round(value, 1);
                }

                OnPropertyChanged("PriceWillRise");
                OnPropertyChanged("PriceWillFall");
            }
        }
        public int SoldCount
        {
            get { return soldCount; }
            set
            {
                soldCount = value;
                OnPropertyChanged("SoldCount");
                OnPropertyChanged("PriceWillRise");
            }
        }

        public int SoldCurrentOrder { get; set; }
        public bool PriceWillRise
        {
            get { return NewPrice > Price && (Math.Abs(Price-NewPrice) > 0.1); }
        }

        public bool PriceWillFall
        {
            get { return NewPrice < Price && (Math.Abs(Price - NewPrice) > 0.1); }
        }

        public bool Enabled { get; internal set; }

        public void UpdatePrice()
        {

            if (NewPrice != Price)
                Console.WriteLine($"{Name} Sold:{soldCount} OldPrice: {Price} NewPrice:{NewPrice} Difference:{Math.Round(NewPrice - Price,1)} ");

            Price = NewPrice;
        }

        private double CalculatePriceUpdate(IEnumerable<Drink> allDrinks, BreakingNewsModel breakingNews)
        {
            double totalDefaultPrices = allDrinks.Sum(d => d.DefaultPrice);
            double totalCurrentPrices = allDrinks.Sum(d => d.NewPrice);
            double difference = totalDefaultPrices - totalCurrentPrices;
            bool isMarketToHigh = difference < 0;

            double averageSoldCount = allDrinks.Average(d => d.SoldCount);
            double priceDifference = SoldCount - averageSoldCount;
            double priceUpdate = priceDifference * 0.2; // adjust this factor to control the size of the price changes

           // if (Math.Round(priceUpdate, 1) < 0.1 || (isMarketToHigh && Math.Round(priceUpdate, 1) < 0))
           //     return Price;

            //Dont lower price when item has been sold. Must at least go a bit higher.
            if (soldCount > 0 && priceUpdate <= 0)
                priceUpdate = 0.1;

            if (SoldCount == 0 || (SoldCount > 0 && priceUpdate < 0))
            {
                Random random = new Random();
                var maxChangePercentage = random.Next(0, 25);
                var maxChange = ((MaximumPrice - MinimumPrice) / 100) * maxChangePercentage;

                if (priceUpdate < 0)
                    priceUpdate = Math.Max(priceUpdate, -maxChange);
                else
                    priceUpdate = Math.Min(priceUpdate, maxChange);

                priceUpdate = Math.Round(Price + priceUpdate, 1);
            }
            else
            {
                priceUpdate = Math.Round(Price + priceUpdate, 1);
            }

            priceUpdate = UpdateForBreakingNews(priceUpdate, allDrinks, breakingNews);

            return Math.Round(priceUpdate, 1);
        }

        private double UpdateForBreakingNews(double priceUpdate, IEnumerable<Drink> drinks, BreakingNewsModel breakingNews)
        {
            try
            {
                if (breakingNews != null && breakingNews.Drinks.Any())
                {
                    if (breakingNews.Drinks.Contains(this))
                        priceUpdate += breakingNews.PriceUpdate;
                }

            }
            catch (Exception)
            {
            }

            return priceUpdate;
        }

        public void UpdateExpectedPriceUpdate(IEnumerable<Drink> allDrinks, BreakingNewsModel breakingNews)
        {
            double updateToPrice = CalculatePriceUpdate(allDrinks, breakingNews);

            var priceDifference = Math.Abs(updateToPrice - Price);

            if (priceDifference < 0.1)
                return;

           NewPrice = updateToPrice;

        }

        public override string ToString()
        {
            return Name;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
