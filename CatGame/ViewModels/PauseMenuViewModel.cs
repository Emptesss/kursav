using CatGame.Helpers;
using CatGame.Models;
using CatGame.Services;
using System.Windows.Input;

namespace CatGame.ViewModels
{
    public class PauseMenuViewModel : ViewModelBase
    {
        private readonly NavigationService _navigation;
        private readonly GameData _gameData;
        private readonly Action _resumeAction;

        // Конструктор для MiniGame1
        public PauseMenuViewModel(MiniGame1ViewModel game, NavigationService navigation)
        {
            _navigation = navigation;
            _gameData = game.GameData;
            _resumeAction = () => game.IsPaused = false;

            InitializeCommands();
        }

        // Конструктор для MiniGame2
        public PauseMenuViewModel(MiniGame2ViewModel game, NavigationService navigation)
        {
            _navigation = navigation;
            _gameData = game.GameData;

            ResumeCommand = new RelayCommand(_ =>
            {
                game.HidePauseMenu();
            });

            ExitCommand = new RelayCommand(_ =>
            {
                SaveBalance();
                _navigation.NavigateTo(new MainGameScreenViewModel(_gameData, _navigation));
            });
        }

        private void InitializeCommands()
        {
            ResumeCommand = new RelayCommand(_ => _resumeAction());
            ExitCommand = new RelayCommand(_ =>
            {
                SaveBalance();
                _navigation.NavigateTo(new MainGameScreenViewModel(_gameData, _navigation));
            });
        }

        private void SaveBalance()
        {
            _gameData.Balance += _gameData.CurrentGameBalance;
            _gameData.CurrentGameBalance = 0;
        }

        public ICommand ResumeCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
    }
}