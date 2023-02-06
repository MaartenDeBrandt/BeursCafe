using BeursCafeWPF.Models;
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
using BeursCafeWPF.Services;
using System.Threading;

namespace BeursCafeWPF.ViewModels
{
    public class DrinksViewModel : INotifyPropertyChanged
    {
        private readonly DrinksPriceService _drinksPriceService;
        private readonly BreakingNewsService _breakingNewsService;
        private readonly SettingsViewModel _settingsViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Drink> _drinks;
        private string _breakingNews;

        public DrinksViewModel(DrinksPriceService drinksPriceService, BreakingNewsService breakingNewsService, SettingsViewModel settingsViewModel)
        {
            _drinksPriceService = drinksPriceService;
            _breakingNewsService = breakingNewsService;
            _settingsViewModel = settingsViewModel;
            Drinks = _drinksPriceService.LoadDrinks();

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

        public void BreakingNewsButton_Click()
        {
            BreakingNews = _breakingNewsService.GetBreakingNews();
        }

        internal virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Properties

        public ObservableCollection<Drink> Drinks
        {
            get => _drinks;
            set
            {
                _drinks = value;
                OnPropertyChanged(nameof(Drinks));
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
        #endregion

        #region timerMethods

        private System.Timers.Timer _timer;
        public void StartTimer()
        {
            _timer = new System.Timers.Timer(_settingsViewModel.PriceUpdateIntervalInMs / (_settingsViewModel.TimesToUpdateExpectedInInterval + 1));
            _timer.Elapsed += Timer;
            _timer.Start();
        }
        int timerCounter;

        public void Timer(object sender, ElapsedEventArgs e)
        {

            if (timerCounter < _settingsViewModel.TimesToUpdateExpectedInInterval)
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
            }
            OnPropertyChanged(nameof(Drinks));
        }

        #endregion
    }
}
