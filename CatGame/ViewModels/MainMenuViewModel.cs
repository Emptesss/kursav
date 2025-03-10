using System.Windows;
using System.Windows.Input;
using CatGame.Helpers;
using CatGame.Models;
using CatGame.Services;
using CatGame.Views;
namespace CatGame.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly GameData _gameData;

        public MainMenuViewModel(NavigationService navigationService, GameData gameData)
        {
            _navigationService = navigationService;
            _gameData = gameData;

            // Инициализация команд
            PlayCommand = new RelayCommand(Play);
            RulesCommand = new RelayCommand(Rules);
            ExitCommand = new RelayCommand(Exit);
        }

        public ICommand PlayCommand { get; }
        public ICommand RulesCommand { get; }
        public ICommand ExitCommand { get; }

        private void Play(object parameter)
        {
            // Переход к игровому экрану с передачей gameData и navigationService
            _navigationService.NavigateTo(new MainGameScreenViewModel(_gameData, _navigationService));
        }

        private void Rules(object parameter)
        {
            // Логика для отображения правил
            MessageBox.Show("Правила игры: ...");
        }

        private void Exit(object parameter)
        {
            // Закрытие приложения
            Application.Current.Shutdown();
        }
    }
}
