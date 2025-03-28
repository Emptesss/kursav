﻿using CatGame.ViewModels;
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

namespace CatGame.Views
{
    /// <summary>
    /// Логика взаимодействия для MainMenuView.xaml
    /// </summary>
    public partial class MainMenuView : UserControl
    {
        public MainMenuView()
        {
            InitializeComponent();
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (DataContext is MiniGame2ViewModel viewModel)
            {
                Point mousePos = e.GetPosition((Canvas)sender);
                viewModel.UpdateAimDirection(mousePos);
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MiniGame2ViewModel viewModel)
            {
                viewModel.MouseShoot();
            }
        }
    }
}
