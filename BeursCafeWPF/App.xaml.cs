using BeursCafeWPF.Services;
using BeursCafeWPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BeursCafeWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ServiceCollection services = new ServiceCollection();
            services.AddScoped<MainWindow>();
            services.AddScoped<AdminPage>();
            services.AddScoped<DrinksViewModel>();
            services.AddScoped<SettingsViewModel>();
            services.AddScoped<DrinksPriceService>();
            services.AddScoped<BreakingNewsService>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            
            MainWindow mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();

            AdminPage adminWindow = serviceProvider.GetService<AdminPage>();
            adminWindow.Show();
        }

    }
}
