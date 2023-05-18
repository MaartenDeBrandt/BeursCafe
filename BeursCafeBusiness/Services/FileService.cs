using BeursCafeBusiness.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                    new Drink { Name = "Jupiler", DrinksType = "bier", MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Cristal", DrinksType = "bier", MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Stella Artois", DrinksType = "bier", MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Kriek Lindemans", DrinksType = "bier", MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Hoegaarden", DrinksType = "bier", MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Cola", DrinksType = "frisdrank", MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Cola zero", DrinksType = "frisdrank", MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Fanta", DrinksType = "frisdrank", MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Sprite", DrinksType = "frisdrank", MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Rodeo", DrinksType = "frisdrank", MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Ice Tea", DrinksType = "frisdrank", MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Water plat", DrinksType = "frisdrank", MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Water bruis", DrinksType = "frisdrank", MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Duvel", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Karmeliet", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "La Chouffe", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Kasteelbier rouge", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Geuze Boon", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Satan White", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Carolus Tripel", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Carolus Classic", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Chimay Blauw", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Chimay Groen", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Kwak Blond", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Kwak Rouge", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Cornet", DrinksType = "bier", MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "St Bernardus abt 12 75cl", DrinksType = "bier", MinimumPrice = 5.0, MaximumPrice = 15.0 , DefaultPrice = 9},

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
