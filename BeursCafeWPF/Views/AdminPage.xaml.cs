﻿using BeursCafeBusiness.Models;
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
        private void OnHide(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Confirm?", "Zeker?", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes)
                return;

            Button button = (Button)sender;

            Drink drink = (Drink)button.DataContext;
            viewModel.HideDrink(drink);
        }

        
        private void OnBreakingNewsClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Confirm?", "Zeker?", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes)
                return;

            viewModel.SelectRandomBreakingNews();
        }
        private void OnRommelPromoClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Confirm?", "Zeker?", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes)
                return;

            viewModel.RommelPromo();
        }
        
        private void OnFinishOrder(object sender, RoutedEventArgs e)
        {
            viewModel.FinishOrder();
        }
        private void OnResetOrder(object sender, RoutedEventArgs e)
        {
            viewModel.ResetOrder();
        }

        private void OnBeursCrash(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Beurscrash?", "Tijd voor een beurs crash?", System.Windows.MessageBoxButton.YesNo);

            if(messageBoxResult == MessageBoxResult.Yes)
            {
                viewModel.BeursCrash();
            }
        }

        private void UpdatePrices_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Confirm?", "Zeker?", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes)
                return;

            viewModel.UpdateDrinkPrice();
        }

        private void IncreaseFont_Click(object sender, RoutedEventArgs e)
        {

            foreach (TextBlock tb in FindVisualChildren<TextBlock>(DrinkListBier))
            {
                tb.FontSize += 2;
            }
            foreach (TextBlock tb in FindVisualChildren<TextBlock>(DrinkListFrisdrank))
            {
                tb.FontSize += 2;
            }

        }

        private void DecreaseFont_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBlock tb in FindVisualChildren<TextBlock>(DrinkListBier))
            {
                tb.FontSize -= 2;
            }
            foreach (TextBlock tb in FindVisualChildren<TextBlock>(DrinkListFrisdrank))
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
