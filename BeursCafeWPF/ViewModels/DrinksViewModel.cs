using BeursCafeBusiness.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using BeursCafeBusiness.Services;
using System.Threading;
using System.Diagnostics.Metrics;
using BeursCafeBusiness.Services;
using System.Configuration;

namespace BeursCafeWPF.ViewModels
{
    public class DrinksViewModel : INotifyPropertyChanged
    {
        private readonly DrinksPriceService _drinksPriceService;
        private readonly BreakingNewsService _breakingNewsService;
        private readonly FileService _fileService;
        private readonly Settings _settings;

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Drink> _drinks;
        private string _breakingNews;
        private int _counter;

        public DrinksViewModel(DrinksPriceService drinksPriceService, BreakingNewsService breakingNewsService, FileService fileService, Settings settings)
        {
            _drinksPriceService = drinksPriceService;
            _breakingNewsService = breakingNewsService;
            _fileService = fileService;
            _settings = settings;

            AllDrinks = _fileService.LoadDrinks();

            StartTimer();
        }       
        
        public void UpdateDrinkPrice()
        {
            _drinksPriceService.UpdateDrinksPrice(ActiveDrinks);

            FireDrinksPropertyChanged();
        }       

        public void DrinkSold(Drink drink, int numberSold = 1)
        {
            drink.SoldCurrentOrder += numberSold;
            FireDrinksPropertyChanged();
        }
        internal void BeursCrash()
        {
            BreakingNews = _breakingNewsService.BeursCrash();
            _drinksPriceService.BeursCrash(ActiveDrinks);
        }
        public void BreakingNewsButton_Click()
        {
            BreakingNews = _breakingNewsService.GetBreakingNews();
        }

        internal virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Properties
        public IEnumerable<Drink> FrisdrankEnabled
        {
            get
            {
                return ActiveDrinks.Where(el => el.DrinksType == "frisdrank");
            }
        }
        public IEnumerable<Drink> BierEnabled
        {
            get 
            {
                return ActiveDrinks.Where(el => el.DrinksType == "bier");
            }
        }
        public IEnumerable<Drink> FrisdrankAll
        {
            get
            {
                return AllDrinks.Where(el => el.DrinksType == "frisdrank");
            }
        }
        public IEnumerable<Drink> BierAll
        {
            get
            {
                return AllDrinks.Where(el => el.DrinksType == "bier");
            }
        }
        public ObservableCollection<Drink> AllDrinks
        {
            get => _drinks;
            set
            {
                _drinks = value;
                FireDrinksPropertyChanged();
            }
        }
        
        public double CurrentOrderTotalPrice => Math.Round(ActiveDrinks.Sum(drink => drink.SoldCurrentOrder * drink.Price),1);


        public IEnumerable<Drink> ActiveDrinks
        {
            get => _drinks.Where(el => el.Enabled);
        }
        public string BreakingNews
        {
            get => _breakingNews;
            set
            {
                _breakingNews = value;
                OnPropertyChanged(nameof(BreakingNews));
            }
        }

        public int Counter
        {
            get => _counter;
            set
            {
                _counter = value;
                OnPropertyChanged(nameof(Counter));
            }
        }

        private void FireDrinksPropertyChanged()
        {
            OnPropertyChanged(nameof(ActiveDrinks));
            OnPropertyChanged(nameof(AllDrinks));
            OnPropertyChanged(nameof(BierEnabled));
            OnPropertyChanged(nameof(FrisdrankEnabled));
            OnPropertyChanged(nameof(BierAll));
            OnPropertyChanged(nameof(FrisdrankAll));
            OnPropertyChanged(nameof(BreakingNews));
            OnPropertyChanged(nameof(CurrentOrderTotalPrice));            
        }
        #endregion

        #region timerMethods

        private System.Timers.Timer _timer;
        private System.Timers.Timer _timerVisual;
        public void StartTimer()
        {
            _timer = new System.Timers.Timer(_settings.PriceUpdateIntervalInMs / (_settings.TimesToUpdateExpectedInInterval + 1));
            _timer.Elapsed += Timer;
            _timer.Start();

            _timerVisual = new System.Timers.Timer(1000);
            _timerVisual.Elapsed += TimerUpdateCounter;
            _timerVisual.Start();

            Counter = _settings.PriceUpdateIntervalInMs / 1000;
        }


        public async void TimerUpdateCounter(object sender, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                Counter -= 1;
            });
        }
        
        int timerCounter;

        public async void Timer(object sender, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                if (timerCounter < _settings.TimesToUpdateExpectedInInterval)
                {
                    if (timerCounter == 0)
                    {
                        //Select 1 drink to be sold a random amount at the beginning of an interval
                        _drinksPriceService.SellRandomDrink(ActiveDrinks, 3);
                    }

                    //Update what the price of drinks is expected to do. Will show red or green arrows in page
                    _drinksPriceService.UpdateExpectedDrinksUpdate(ActiveDrinks);
                    timerCounter++;
                }
                else
                {
                    //Actually update the drink prices
                    _drinksPriceService.UpdateDrinksPrice(ActiveDrinks);
                    timerCounter = 0;
                    Counter = _settings.PriceUpdateIntervalInMs / 1000;
                    BreakingNews = "";
                }

                FireDrinksPropertyChanged();            
            });
        }

        internal void HideDrink(Drink drink)
        {
            _drinksPriceService.HideDrink(drink);
            FireDrinksPropertyChanged();
        }

        internal void FinishOrder()
        {
            _drinksPriceService.FinishOrder(ActiveDrinks);
            FireDrinksPropertyChanged();
        }

        internal void ResetOrder()
        {
            _drinksPriceService.ResetOrder(ActiveDrinks);
            FireDrinksPropertyChanged();
        }

        #endregion
    }
}
