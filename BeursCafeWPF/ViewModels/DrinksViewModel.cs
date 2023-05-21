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
using static BeursCafeBusiness.Models.Drink;

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
        private BreakingNewsModel _breakingNews;
        private int _counter;
        Random _random = new Random();

        public DrinksViewModel(DrinksPriceService drinksPriceService, BreakingNewsService breakingNewsService, FileService fileService, Settings settings)
        {
            _drinksPriceService = drinksPriceService;
            _breakingNewsService = breakingNewsService;
            _fileService = fileService;
            _settings = settings;

            AllDrinks = _fileService.LoadDrinks();
            BreakingNewsList = _fileService.LoadBreakingNews(AllDrinks);
            StartTimer();
        }       
        
        public void UpdateDrinkPrice()
        {
            _drinksPriceService.UpdateDrinksPrice(ActiveDrinks, BreakingNews);

            FireDrinksPropertyChanged();
        }       

        public void DrinkSold(Drink drink, int numberSold = 1)
        {
            drink.SoldCurrentOrder += numberSold;
            FireDrinksPropertyChanged();
        }
        internal void BeursCrash()
        {
            BreakingNews = new BreakingNewsModel() { BreakingNews = _breakingNewsService.BeursCrash() };
            _drinksPriceService.BeursCrash(ActiveDrinks);
        }
        public void SelectRandomBreakingNews()
        {
            if (BreakingNews != null)
                return;

            var breakingNewsList = BreakingNewsList?.Where(el => !el.AlreadyRun).ToArray();

            if (breakingNewsList != null && breakingNewsList.Any())
            {
                var index = _random.Next(breakingNewsList.Count());
                var selectedBreakingNews = breakingNewsList[index];

                if (_breakingNewsService.CheckIfBreakingNewsStillRelevant(ActiveDrinks, selectedBreakingNews))
                    BreakingNews = selectedBreakingNews;
                else
                {
                    selectedBreakingNews.AlreadyRun = true;
                    SelectRandomBreakingNews();
                }
            }
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
                return ActiveDrinks.Where(el => el.DrinksType == DrinkTypes.frisdrank);
            }
        }
        public IEnumerable<Drink> BierEnabled
        {
            get 
            {
                return ActiveDrinks.Where(el => el.DrinksType == DrinkTypes.bier);
            }
        }
        public IEnumerable<Drink> GroteFlesEnabled
        {
            get
            {
                return ActiveDrinks.Where(el => el.DrinksType == DrinkTypes.grotefles);
            }
        }
        public IEnumerable<Drink> FrisdrankAll
        {
            get
            {
                return AllDrinks.Where(el => el.DrinksType == DrinkTypes.frisdrank);
            }
        }
        public IEnumerable<Drink> BierAll
        {
            get
            {
                return AllDrinks.Where(el => el.DrinksType == DrinkTypes.bier);
            }
        }
        public IEnumerable<Drink> GroteFlesAll
        {
            get
            {
                return AllDrinks.Where(el => el.DrinksType == DrinkTypes.grotefles);
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
        public IEnumerable<BreakingNewsModel> BreakingNewsList { get; set; }
        public double CurrentOrderTotalPrice => Math.Round(ActiveDrinks.Sum(drink => drink.SoldCurrentOrder * drink.Price),1);


        public IEnumerable<Drink> ActiveDrinks
        {
            get => _drinks.Where(el => el.Enabled);
        }
        public BreakingNewsModel BreakingNews
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
            OnPropertyChanged(nameof(GroteFlesEnabled));            
            OnPropertyChanged(nameof(BierAll));
            OnPropertyChanged(nameof(FrisdrankAll));
            OnPropertyChanged(nameof(GroteFlesAll));

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
                        //Select random drinks to be sold a random amount at the beginning of an interval
                      
                        var numberOfDrinksToUpdate = _random.Next(2,6);
                        _drinksPriceService.SellRandomDrink(ActiveDrinks, numberOfDrinksToUpdate);
                    }

                    //Update what the price of drinks is expected to do. Will show red or green arrows in page
                    _drinksPriceService.UpdateExpectedDrinksUpdate(ActiveDrinks, BreakingNews);
                    timerCounter++;
                }
                else
                {
                    //Actually update the drink prices
                    _drinksPriceService.UpdateDrinksPrice(ActiveDrinks, BreakingNews);
                    timerCounter = 0;
                    Counter = _settings.PriceUpdateIntervalInMs / 1000;
                    
                    if(BreakingNews != null)
                    {
                        BreakingNews.AlreadyRun = true;
                        BreakingNews = null;
                    }

                    _fileService.SaveDrinks(AllDrinks);
                    _fileService.SaveBreakingNews(BreakingNewsList);
                }

                FireDrinksPropertyChanged();            
            });
        }

        internal void HideDrink(Drink drink)
        {
            _drinksPriceService.HideDrink(drink);
            if(!drink.Enabled & BreakingNews == null)
                BreakingNews = new BreakingNewsModel() { BreakingNews = $"Breaking news: Door uitzonderlijke vraag is {drink.Name} niet langer beschikbaar." };
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
