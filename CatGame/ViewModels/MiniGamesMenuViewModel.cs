using CatGame.Helpers;
using CatGame.Models;
using CatGame.Services;
using System.Windows.Input;

namespace CatGame.ViewModels
{
    public class MiniGamesMenuViewModel : ViewModelBase
    {
        private readonly GameData _gameData;
        private readonly NavigationService _navigationService;

        public MiniGamesMenuViewModel(GameData gameData, NavigationService navigationService)
        {
            _gameData = gameData;
            _navigationService = navigationService;

            // Добавьте команды для других мини-игр по аналогии
            SelectMiniGame1Command = new RelayCommand(SelectMiniGame1);
            SelectMiniGame2Command = new RelayCommand(_ =>
            _navigationService.NavigateTo(new MiniGame2ViewModel(_gameData, _navigationService)));
        }

        public ICommand SelectMiniGame1Command { get; }
        public ICommand SelectMiniGame2Command { get; }

        private void SelectMiniGame1(object parameter)
        {
            _navigationService.NavigateTo(new MiniGame1ViewModel(_gameData, _navigationService));
        }

        public ICommand ReturnCommand => new RelayCommand(_ =>
            _navigationService.NavigateTo(new MainGameScreenViewModel(_gameData, _navigationService)));
    }
}