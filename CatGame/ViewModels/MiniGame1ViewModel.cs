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
        public ICommand CompleteMiniGameCommand { get; }

        public MiniGame1ViewModel(GameData gameData)
        {
            _gameData = gameData;
            CompleteMiniGameCommand = new RelayCommand(CompleteMiniGame);
        }

        private void CompleteMiniGame(object parameter)
        {
            // Логика завершения мини-игры, например, начисление монет
            int reward = 25; // Пример награды
            _gameData.Balance += reward;

            // Вернуться на главный экран
            NavigationService.Instance.CurrentView = new MainGameScreenViewModel();
        }
    }
}
