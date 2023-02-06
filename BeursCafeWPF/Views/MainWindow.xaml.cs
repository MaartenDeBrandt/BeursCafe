using BeursCafeWPF.Models;
using BeursCafeWPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeursCafeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DrinksViewModel viewModel;

        public MainWindow(DrinksViewModel drinksViewModel)
        {
            InitializeComponent();
            viewModel = drinksViewModel;

            DataContext= viewModel;
        }

        private void OnButtonSoldClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            Drink drink = (Drink)button.DataContext;
            viewModel.DrinkSold(drink);
        }
            

    }
}
