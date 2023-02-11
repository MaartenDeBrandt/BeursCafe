using BeursCafeWPF.Models;
using BeursCafeWPF.ViewModels;
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
    public partial class AdminPage : Window
    {
        private readonly DrinksViewModel viewModel;

        public AdminPage(DrinksViewModel drinksViewModel)
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
        private void OnButtonSoldClick3(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            Drink drink = (Drink)button.DataContext;
            viewModel.DrinkSold(drink,3);
        }
        private void OnButtonSoldClick5(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            Drink drink = (Drink)button.DataContext;
            viewModel.DrinkSold(drink,5);
        }
        private void OnButtonSoldClick10(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            Drink drink = (Drink)button.DataContext;
            viewModel.DrinkSold(drink, 10);
        }

        private void OnBreakingNewsClick(object sender, RoutedEventArgs e)
        {
            viewModel.BreakingNews = "Test";
        }

        private void UpdatePrices_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateDrinkPrice();
        }

        private void IncreaseFont_Click(object sender, RoutedEventArgs e)
        {

            foreach (TextBlock tb in FindVisualChildren<TextBlock>(DrinkList))
            {
                tb.FontSize += 2;
            }

        }

        private void DecreaseFont_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBlock tb in FindVisualChildren<TextBlock>(DrinkList))
            {
                tb.FontSize -= 2;
            }
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

    }
    
}
