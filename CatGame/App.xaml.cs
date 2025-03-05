using CatGame.Models;
using CatGame.Services;
using CatGame.ViewModels;
using System.Windows;

namespace CatGame
{
    public partial class App : Application
    {
        public static MusicPlayer MusicPlayer { get; } = new MusicPlayer();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string musicPath = System.IO.Path.Combine(
           AppDomain.CurrentDomain.BaseDirectory,
           "Views/music.mp3");

            MusicPlayer.Play(musicPath);

            // Создаем общие данные игры
            var gameData = new GameData { Balance = 100 };

            // Инициализация сервиса навигации
            var navigationService = NavigationService.Instance;

            // Создание главного окна
            var mainWindow = new MainWindow();

            // Передаем gameData и navigationService в MainMenuViewModel
            var mainMenuViewModel = new MainMenuViewModel(navigationService, gameData);

            // Устанавливаем DataContext для главного окна
            mainWindow.DataContext = new MainWindowViewModel(navigationService);

            // Устанавливаем начальный экран
            navigationService.NavigateTo(mainMenuViewModel);

            // Показываем главное окно
            mainWindow.Show();
        }
    }
}