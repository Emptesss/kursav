using CatGame.Helpers;
using CatGame.Models;
using CatGame.Services;
using System.Diagnostics;
using System.Windows.Input;

namespace CatGame.ViewModels
{
    public class GameOverViewModel : ViewModelBase
    {
        private readonly GameData _gameData;
        private readonly NavigationService _navigation;

        public int Score => _gameData.CurrentGameBalance; // Используем CurrentGameBalance

        public ICommand RestartCommand { get; }
        public ICommand ExitCommand { get; }

        public GameOverViewModel(GameData gameData, NavigationService navigation)
        {
            _gameData = gameData;
            _navigation = navigation;
            Debug.WriteLine($"Создано меню проигрыша. Текущий баланс: {_gameData.CurrentGameBalance}");

            _gameData.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(GameData.CurrentGameBalance))
                {
                    OnPropertyChanged(nameof(Score)); // Уведомляем об изменении Score
                }
            };

            RestartCommand = new RelayCommand(_ =>
            {
                Debug.WriteLine($"Нажата кнопка 'Заново'. Общий баланс до: {_gameData.Balance}, текущий баланс: {_gameData.CurrentGameBalance}");
                _gameData.Balance += _gameData.CurrentGameBalance; // Добавляем текущий баланс к общему
                Debug.WriteLine($"Общий баланс после: {_gameData.Balance}");
                _navigation.NavigateTo(new MiniGame1ViewModel(_gameData, _navigation));
            });

            ExitCommand = new RelayCommand(_ =>
            {
                Debug.WriteLine($"Нажата кнопка 'Выход'. Общий баланс до: {_gameData.Balance}, текущий баланс: {_gameData.CurrentGameBalance}");
                _gameData.Balance += _gameData.CurrentGameBalance; // Добавляем текущий баланс к общему
                Debug.WriteLine($"Общий баланс после: {_gameData.Balance}");
                _navigation.NavigateTo(new MainGameScreenViewModel(_gameData, _navigation));
            });
        }
    }
}