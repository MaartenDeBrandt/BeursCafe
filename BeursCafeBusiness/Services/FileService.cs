using BeursCafeBusiness.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace BeursCafeBusiness.Services
{
    public class FileService
    {
        private readonly Settings _settings;

        string drinksFileLocation;
        string settingsFileLocation;
        string newsFileLocation;

        public FileService(Settings settings)
        {
            _settings = settings;

            drinksFileLocation = settings.FileLocation + @"\drinks.json";
            newsFileLocation = settings.FileLocation + @"\news.json";
            settingsFileLocation = settings.FileLocation + @"\settings.json";

            if (!Directory.Exists(_settings.FileLocation))
            {
                Directory.CreateDirectory(_settings.FileLocation);
            }
        }
        public ObservableCollection<Drink> LoadDrinks()
        {
            if (File.Exists(drinksFileLocation))
            {
                string json = File.ReadAllText(drinksFileLocation);
                return JsonConvert.DeserializeObject<ObservableCollection<Drink>>(json);
            }
            else
            {
                return new ObservableCollection<Drink>
                {
                    new Drink { Name = "Jupiler", DrinksType = "bier", MinimumPrice = 1.0, MaximumPrice = 3.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Hoegaarden", DrinksType = "bier", MinimumPrice = 1.0, MaximumPrice = 3.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Cola", DrinksType = "frisdrank", MinimumPrice = 1.0, MaximumPrice = 3.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Water", DrinksType = "frisdrank", MinimumPrice = 1.0, MaximumPrice = 3.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Icetea", DrinksType = "frisdrank", MinimumPrice = 1.0, MaximumPrice = 3.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Duvel", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 6.0 , DefaultPrice = 3},
                    new Drink { Name = "Karmeliet", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 6.0 , DefaultPrice = 3},
                    new Drink { Name = "Kasteelbier rouge", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 6.0 , DefaultPrice = 3},
                    new Drink { Name = "Geuze", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 6.0 , DefaultPrice = 3},
                    new Drink { Name = "Satan White", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 6.0 , DefaultPrice = 3},
                    new Drink { Name = "Carolus", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 6.0 , DefaultPrice = 3}

                };
            }
        }

        public void SaveDrinks(IEnumerable<Drink> drinks)
        {
            string json = JsonConvert.SerializeObject(drinks, Formatting.Indented);
            File.WriteAllText(drinksFileLocation, json);
        }

        public void LoadSettings(Settings settings)
        {
            if (File.Exists(settingsFileLocation))
            {
                string json = File.ReadAllText(settingsFileLocation);
                var settingsFile = JsonConvert.DeserializeObject<Settings>(json);
                
                if(settingsFile != null) 
                    settings.LoadSettings(settingsFile);
            }
        }
        public void SaveSettings(Settings settings)
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(settingsFileLocation, json);
        }
    }
}
