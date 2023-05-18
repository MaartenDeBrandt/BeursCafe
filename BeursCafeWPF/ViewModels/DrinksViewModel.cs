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

            Drinks = _fileService.LoadDrinks();

            StartTimer();
        }       
        
        public void UpdateDrinkPrice()
        {
            _drinksPriceService.UpdateDrinksPrice(Drinks);

            OnPropertyChanged(nameof(Drinks));
        }       

        public void DrinkSold(Drink drink, int numberSold = 1)
        {
            drink.SoldCount += numberSold;
        }
        internal void BeursCrash()
        {
            BreakingNews = _breakingNewsService.BeursCrash();
            _drinksPriceService.BeursCrash(Drinks);
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
        public IEnumerable<Drink> Frisdrank
        {
            get
            {
                return Drinks.Where(el => el.DrinksType == "frisdrank");
            }
        }
        public IEnumerable<Drink> Bier
        {
            get 
            {
                return Drinks.Where(el => el.DrinksType == "bier");
            }
        }
        public ObservableCollection<Drink> Drinks
        {
            get => _drinks;
            set
            {
                _drinks = value;
                OnPropertyChanged(nameof(Drinks));

                OnPropertyChanged(nameof(Bier));
                OnPropertyChanged(nameof(Frisdrank));
            }
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
                        _drinksPriceService.SellRandomDrink(Drinks);
                    }

                    //Update what the price of drinks is expected to do. Will show red or green arrows in page
                    _drinksPriceService.UpdateExpectedDrinksUpdate(Drinks);
                    timerCounter++;
                }
                else
                {
                    //Actually update the drink prices
                    _drinksPriceService.UpdateDrinksPrice(Drinks);
                    timerCounter = 0;
                    Counter = _settings.PriceUpdateIntervalInMs / 1000;
                }

                OnPropertyChanged(nameof(Drinks));                
            });
        }

     

        #endregion
    }
}
