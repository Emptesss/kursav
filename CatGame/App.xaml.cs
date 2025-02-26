using CatGame.Services;
using CatGame.ViewModels;
using CatGame.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CatGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();
            var navigationService = new NavigationService(mainWindow);
            var mainMenuViewModel = new MainMenuViewModel();
            navigationService.CurrentView = mainMenuViewModel;

            // Create MainWindowViewModel and pass NavigationService
            var mainWindowViewModel = new MainWindowViewModel(navigationService);
            mainWindow.DataContext = mainWindowViewModel;

            mainWindow.Content = new MainMenuView() { DataContext = mainMenuViewModel };
            mainWindow.Show();

        }
    }
}
