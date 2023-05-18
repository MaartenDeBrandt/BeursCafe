using BeursCafeBusiness.Models;
using BeursCafeWPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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

            DataContext = viewModel;

            
        }

        private void OnButtonSoldClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            Drink drink = (Drink)button.DataContext;
            viewModel.DrinkSold(drink);
        }

        private void IncreaseFont_Click(object sender, RoutedEventArgs e)
        {
            BreakingNewsTextBlock.FontSize += 2;
            textBlockCounter.FontSize += 2;

            foreach (TextBlock tb in FindVisualChildren<TextBlock>(DrinksDataGridBier))
            {
                tb.FontSize += 2;
            }

            foreach (TextBlock tb in FindVisualChildren<TextBlock>(DrinksDataGridFrisdrank))
            {
                tb.FontSize += 2;
            }

        }

        private void DecreaseFont_Click(object sender, RoutedEventArgs e)
        {
            BreakingNewsTextBlock.FontSize -= 2;

            foreach (TextBlock tb in FindVisualChildren<TextBlock>(DrinksDataGridBier))
            {
                tb.FontSize -= 2;
            }
            foreach (TextBlock tb in FindVisualChildren<TextBlock>(DrinksDataGridFrisdrank))
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
