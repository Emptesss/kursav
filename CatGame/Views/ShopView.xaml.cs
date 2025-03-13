﻿using CatGame.Models;
using CatGame.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace CatGame.Views
{
    /// <summary>
    /// Логика взаимодействия для ShopView.xaml
    /// </summary>
    public partial class ShopView : UserControl
    {
        public ShopView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Resources["PurchasableSkins"] is CollectionViewSource skinsView)
            {
                skinsView.Filter += (s, args) =>
                {
                    if (args.Item is Skin skin)
                    {
                        Debug.WriteLine($"Skin: {skin.Name}, Price: {skin.Price}, Purchased: {skin.IsPurchased}");
                        args.Accepted = skin.Price > 0;
                    }
                };
                skinsView.View.Refresh();
            }
            if (Resources["PurchasableWallpapers"] is CollectionViewSource wallpapersView)
            {
                wallpapersView.Filter += (s, args) =>
                {
                    if (args.Item is Wallpaper wallpaper)
                    {
                        Debug.WriteLine($"Wallpaper: {wallpaper.Name}, Price: {wallpaper.Price}, Purchased: {wallpaper.IsPurchased}");
                        args.Accepted = wallpaper.Price > 0;
                    }   
                };
                wallpapersView.View.Refresh();
            }
            if (Resources["PurchasableLockers"] is CollectionViewSource lockersView)
            {
                lockersView.Filter += (s, args) =>
                {
                    if (args.Item is Locker locker)
                    {
                        Debug.WriteLine($"Locker: {locker.Name}, Price: {locker.Price}, Purchased: {locker.IsPurchased}");
                        args.Accepted = locker.Price > 0;
                    }
                };

                // Добавляем сортировку
                lockersView.SortDescriptions.Add(
                    new SortDescription("Size", ListSortDirection.Ascending));

                lockersView.View.Refresh();
            }
        }
    }
}

