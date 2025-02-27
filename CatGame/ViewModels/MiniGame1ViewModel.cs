using CatGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CatGame.Helpers;
using CatGame.Services;
namespace CatGame.ViewModels
{
    public class MiniGame1ViewModel : ViewModelBase
    {
        private readonly GameData _gameData;
        private readonly NavigationService _navigationService;

        public MiniGame1ViewModel(GameData gameData, NavigationService navigationService)
        {
            _gameData = gameData;
            _navigationService = navigationService;

            // Инициализация команды
            CompleteMiniGameCommand = new RelayCommand(CompleteMiniGame);
        }

        public ICommand CompleteMiniGameCommand { get; }

        private void CompleteMiniGame(object parameter)
        {
            // Логика завершения мини-игры, например, начисление монет
            int reward = 25; // Пример награды
            _gameData.Balance += reward;

            // Возврат на главный экран
            _navigationService.NavigateTo(new MainGameScreenViewModel(_gameData, _navigationService));
        }
    }
}
