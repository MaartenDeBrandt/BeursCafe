using BeursCafeWPF.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeursCafeWPF.Services
{
    public class BreakingNewsService
    {
        public BreakingNewsService()
        {
        }

        internal string GetBreakingNews()
        {
            // Read breaking news texts from JSON file
            var breakingNewsJson = File.ReadAllText("breakingnews.json");
            var breakingNews = JsonConvert.DeserializeObject<string[]>(breakingNewsJson);

            // Choose a random breaking news text
            var random = new Random();
            var index = random.Next(breakingNews.Length);
            return breakingNews[index];
        }
    }
}
