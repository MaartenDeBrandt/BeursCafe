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
using static BeursCafeBusiness.Models.Drink;

namespace BeursCafeBusiness.Services
{
    public class FileService
    {
        private readonly Settings _settings;

        string drinksFileLocation;
        string breakingNewsFileLocation;
        string settingsFileLocation;
        string newsFileLocation;

        public FileService(Settings settings)
        {
            _settings = settings;

            drinksFileLocation = settings.FileLocation + @"\drinks.json";
            newsFileLocation = settings.FileLocation + @"\news.json";
            settingsFileLocation = settings.FileLocation + @"\settings.json";
            breakingNewsFileLocation = settings.FileLocation + @"\breakingNews.json";

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
                    new Drink { Name = "Jupiler", DrinksType = DrinkTypes.bier, MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Cristal", DrinksType = DrinkTypes.bier, MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Stella Artois", DrinksType = DrinkTypes.bier, MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Kriek Lindemans", DrinksType = DrinkTypes.bier, MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Cola", DrinksType = DrinkTypes.frisdrank, MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Cola zero", DrinksType = DrinkTypes.frisdrank, MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Fanta", DrinksType = DrinkTypes.frisdrank, MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Sprite", DrinksType = DrinkTypes.frisdrank, MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Rodeo", DrinksType = DrinkTypes.frisdrank, MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Ice Tea", DrinksType = DrinkTypes.frisdrank, MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Water plat", DrinksType = DrinkTypes.frisdrank, MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Water bruis", DrinksType = DrinkTypes.frisdrank, MinimumPrice = 1.0, MaximumPrice = 8.0 , DefaultPrice = 1.5},
                    new Drink { Name = "Duvel", DrinksType = DrinkTypes.bier, MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Karmeliet", DrinksType = DrinkTypes.bier, MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "La Chouffe", DrinksType = DrinkTypes.bier, MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Kasteelbier rouge", DrinksType = DrinkTypes.bier, MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Geuze Boon", DrinksType = DrinkTypes.bier, MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Carolus Tripel", DrinksType = DrinkTypes.bier, MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Carolus Classic", DrinksType = DrinkTypes.bier, MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Chimay Blauw", DrinksType = DrinkTypes.bier, MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Chimay Groen", DrinksType = DrinkTypes.bier, MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Kwak Blond", DrinksType = DrinkTypes.bier, MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Kwak Rouge", DrinksType = DrinkTypes.bier, MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "Cornet", DrinksType = DrinkTypes.bier, MinimumPrice = 2.0, MaximumPrice = 10.0 , DefaultPrice = 3},
                    new Drink { Name = "St Bernardus abt 12 75cl", DrinksType = DrinkTypes.grotefles, MinimumPrice = 6.0, MaximumPrice = 15.0 , DefaultPrice = 9},
                    new Drink { Name = "Aymom 75cl", DrinksType = DrinkTypes.grotefles, MinimumPrice = 6.0, MaximumPrice = 12 , DefaultPrice = 9},
                    new Drink { Name = "Bons vieux 75cl", DrinksType = DrinkTypes.grotefles, MinimumPrice = 6.0, MaximumPrice = 12 , DefaultPrice = 9},
                    new Drink { Name = "Orvelo 75cl", DrinksType = DrinkTypes.grotefles, MinimumPrice = 6.0, MaximumPrice = 12 , DefaultPrice = 9},
                    new Drink { Name = "Kwak blond 75cl", DrinksType = DrinkTypes.grotefles, MinimumPrice = 6.0, MaximumPrice = 12 , DefaultPrice = 9},
                    new Drink { Name = "Kwak amber 75cl", DrinksType = DrinkTypes.grotefles, MinimumPrice = 6.0, MaximumPrice = 12 , DefaultPrice = 9},
                    new Drink { Name = "Cornet 75cl", DrinksType = DrinkTypes.grotefles, MinimumPrice = 6.0, MaximumPrice = 12 , DefaultPrice = 9},
                    new Drink { Name = "Cava brut 75cl", DrinksType = DrinkTypes.grotefles, MinimumPrice = 9.0, MaximumPrice = 20 , DefaultPrice = 12},
                };
            }
        }
        public List<BreakingNewsModel> LoadBreakingNews(IEnumerable<Drink> allDrinks)
        {
            var breakingNews = new List<BreakingNewsModel>();
            if (File.Exists(breakingNewsFileLocation))
            {
                string json = File.ReadAllText(breakingNewsFileLocation); 
                breakingNews = JsonConvert.DeserializeObject<List<BreakingNewsModel>>(json);
            }
            else
            {
                breakingNews = new List<BreakingNewsModel>
                {
                    new BreakingNewsModel { DrinkNames = "Jupiler,Stella Artois", PriceUpdate = 2, BreakingNews ="Breaking news: Staking bij AB InBev zorgt voor bezorgdheid over mogelijke stijging van bierprijzen."},
                    new BreakingNewsModel { DrinkNames = "Fanta,Cola,Sprite,Rodeo,Ice Tea", PriceUpdate = 1, BreakingNews ="Breaking news: Overheid voert suikertaks in op frisdrank om de volksgezondheid te bevorderen en obesitas tegen te gaan."},
                    new BreakingNewsModel { DrinkNames = "Karmeliet", PriceUpdate = 2, BreakingNews ="Breaking news: Tijdelijke prijsverhoging voor Karmeliet-bier vanwege beperkte beschikbaarheid en exclusiviteit."},
                    new BreakingNewsModel { DrinkNames = "Carolus Tripel", PriceUpdate = 1.5, BreakingNews ="Breaking news: Carolus Tripel ziet prijsstijging door erkenning van superieure kwaliteit en vakmanschap."},
                    new BreakingNewsModel { DrinkNames = "Cristal", PriceUpdate = 1.8, BreakingNews ="Breaking news: Cristal-bier ziet prijsstijging door de toenemende vraag naar deze verfrissende en iconische Belgische dorstlesser."},                    
                    new BreakingNewsModel { DrinkNames = "Geuze Boon", PriceUpdate = 2.1, BreakingNews ="Breaking news: Geuze Boon-prijzen stijgen tijdelijk vanwege groeiende internationale populariteit van deze traditionele Belgische geuze."},
                    new BreakingNewsModel { DrinkNames = "Kasteelbier rouge", PriceUpdate = 1.3, BreakingNews ="Breaking news: Kasteelbier Rouge tijdelijk duurder door de zeldzame blend van fruitige aroma's en dieprode kleur."},
                    new BreakingNewsModel { DrinkNames = "Carolus Tripel", PriceUpdate = 1.8, BreakingNews ="Breaking news: Carolus Classic-bier tijdelijk duurder vanwege zeldzame ingrediënten en langdurige rijping voor unieke smaakervaring."},
                    new BreakingNewsModel { DrinkNames = "Stella Artois", PriceUpdate = 1.8, BreakingNews ="Breaking news: Tijdelijke prijsverhoging voor Stella Artois vanwege de groeiende wereldwijde populariteit van dit iconische Belgische bier."},
                    new BreakingNewsModel { DrinkNames = "Chimay Blauw", PriceUpdate = 1.8, BreakingNews ="Breaking news: Chimay Blauw ervaart tijdelijke prijsstijging vanwege de beperkte beschikbaarheid en de prestigieuze reputatie van dit trappistenbier."},


                    new BreakingNewsModel { DrinkNames = "Kriek Lindemans", PriceUpdate = -2, BreakingNews ="Promotie: Kriek Lindemans-bier tijdelijk in prijs verlaagd - Geniet van de fruitige frisheid van deze kriek tegen een voordelige prijs!"},
                    new BreakingNewsModel { DrinkNames = "Chimay Groen", PriceUpdate = -2, BreakingNews ="Promotie: Chimay Groen-bier nu goedkoper - Profiteer van de gelegenheid om dit karaktervolle trappistenbier te proeven voor een scherpe prijs!"},
                    new BreakingNewsModel { DrinkNames = "Duvel", PriceUpdate = -2, BreakingNews ="Promotie: Duvel-bier in de aanbieding - Profiteer van tijdelijk lagere prijzen voor dit heerlijke Belgische bier!"},
                    new BreakingNewsModel { DrinkNames = "Kwak Blond,Kwak Rouge,Kwak blond 75cl,Kwak amber 75cl", PriceUpdate = -3, BreakingNews ="Promotie: Brouwerij Bosteels verrast bierliefhebbers met spannende promotie op Kwak bieren."},
                    new BreakingNewsModel { DrinkNames = "Bons vieux 75cl", PriceUpdate = -2, BreakingNews = "Promotie: Brasserie Dupont kondigt prijsverlaging aan voor Bons Vieux-bier: een traktatie voor bierliefhebbers!"},                    
                    new BreakingNewsModel { DrinkNames = "St Bernardus abt 12 75cl", PriceUpdate = -2, BreakingNews = "Promotie: Tijdelijke prijsverlaging voor St. Bernardus Abt 12 - Geniet van dit kwaliteitsbier voor een voordelige prijs!"},
                    new BreakingNewsModel { DrinkNames = "Aymom 75cl", PriceUpdate = -2, BreakingNews = "Promotie: Aymon-bier in de aanbieding - Profiteer van een beperkte periode van lagere prijzen voor dit heerlijke brouwsel!"},
                    new BreakingNewsModel { DrinkNames = "Bons vieux 75cl", PriceUpdate = -2, BreakingNews = "Promotie: Bons Vieux-bier nu goedkoper - Mis deze kans niet om te genieten van dit klassieke bier voor een geweldige prijs!"},
                    new BreakingNewsModel { DrinkNames = "Orvelo 75cl", PriceUpdate = -2, BreakingNews = "Promotie: Orvelo-bier tijdelijk in prijs verlaagd - Proef de smaakvolle sensatie zonder de bank te breken!"},
                    new BreakingNewsModel { DrinkNames = "Kwak blond 75cl,Kwak Blond", PriceUpdate = -2, BreakingNews = "Promotie: Kwak Blond in de aanbieding - Ontdek dit karaktervolle blond bier voor een scherpe prijs!"},
                    new BreakingNewsModel { DrinkNames = "Kwak amber 75cl", PriceUpdate = -2, BreakingNews = "Promotie: Kwak Amber tijdelijk goedkoper - Geniet van de rijke smaak van dit amberkleurige bier tegen een gereduceerde prijs!"},
                    new BreakingNewsModel { DrinkNames = "Cava brut 75cl", PriceUpdate = -2, BreakingNews = "Promotie: Tijdelijke prijsverlaging voor Cava Brut - Verwen jezelf met deze sprankelende traktatie zonder je budget te belasten!"},
                };
            }

            foreach (var item in breakingNews)
            {
                var drinkNames = item.DrinkNames.Split(',');
                item.Drinks = allDrinks.Where(el => drinkNames.Contains(el.Name)).ToList();
            }

            return breakingNews;
        }
        public void SaveBreakingNews(IEnumerable<BreakingNewsModel> breakingNews)
        {
            string json = JsonConvert.SerializeObject(breakingNews, Formatting.Indented);
            File.WriteAllText(breakingNewsFileLocation, json);
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
