using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CatGame.ViewModels;

namespace CatGame.Views
{
    public partial class MiniGame2View : UserControl
    {
        public MiniGame2View()
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