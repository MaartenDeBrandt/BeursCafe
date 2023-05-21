using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeursCafeBusiness.Models
{
    public class Drink : INotifyPropertyChanged
    {
        public enum DrinkTypes
        {
            bier,frisdrank,grotefles
        }
        private string name;
        private double minimumPrice;
        private double maximumPrice;
        private double defaultPrice;
        private double price;
        private int soldCount;
        private int updatePricePercentage;
        private bool priceWillRise;
        private bool priceWillFall;

        public event PropertyChangedEventHandler PropertyChanged;
        public Drink()
        {
            Enabled= true;
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
                return price;
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
                OnPropertyChanged("Price");
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
            get { return priceWillRise; }
            set
            {
                priceWillRise = value;
                OnPropertyChanged("PriceWillRise");
                OnPropertyChanged("PriceWillFall");
            }
        }

        public bool PriceWillFall
        {
            get { return priceWillFall; }
            set
            {
                priceWillFall = value;
                OnPropertyChanged("PriceWillFall");
                OnPropertyChanged("PriceWillRise");
            }
        }

        public bool Enabled { get; internal set; }

        public void UpdatePrice(IEnumerable<Drink> allDrinks, BreakingNewsModel breakingNews)
        {
            double priceUpdate = CalculatePriceUpdate(allDrinks, breakingNews);

            Console.WriteLine($"{Name} Sold:{soldCount} OldPrice: {Price} NewPrice:{priceUpdate} Difference:{priceUpdate- Price} ");
            Price = priceUpdate;
        }

        private double CalculatePriceUpdate(IEnumerable<Drink> allDrinks, BreakingNewsModel breakingNews)
        {
            double averageSoldCount = allDrinks.Average(d => d.SoldCount);
            double priceDifference = SoldCount - averageSoldCount;
            double priceUpdate = priceDifference * 0.1; // adjust this factor to control the size of the price changes

            //Dont lower price when item has been sold. Keep it neutral at least
            if (soldCount > 0 && priceUpdate < 0)
                priceUpdate = 0;

            if (SoldCount == 0 || (SoldCount > 0 && priceUpdate < 0))
            {
                Random random = new Random();
                var maxChangePercentage = random.Next(0,25);
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
            double newPrice =  CalculatePriceUpdate(allDrinks, breakingNews);

            PriceWillRise = false;
            PriceWillFall = false;

            if (newPrice > Price && Price < MaximumPrice)
                PriceWillRise = true;

            if (Price > newPrice && Price > MinimumPrice)
                PriceWillFall = true;

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
