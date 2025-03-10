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
        private readonly string _sourceGame; // Добавляем поле для хранения источника

        public int Score => _gameData.CurrentGameBalance;

        public ICommand RestartCommand { get; }
        public ICommand ExitCommand { get; }

        // Добавляем параметр sourceGame в конструктор
        public GameOverViewModel(GameData gameData, NavigationService navigation, string sourceGame = "MiniGame1")
        {
            _gameData = gameData;
            _navigation = navigation;
            _sourceGame = sourceGame;

            Debug.WriteLine($"Создано меню проигрыша. Текущий баланс: {_gameData.CurrentGameBalance}");

            _gameData.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(GameData.CurrentGameBalance))
                {
                    OnPropertyChanged(nameof(Score));
                }
            };

            RestartCommand = new RelayCommand(_ =>
            {
                Debug.WriteLine($"Нажата кнопка 'Заново'. Общий баланс до: {_gameData.Balance}, текущий баланс: {_gameData.CurrentGameBalance}");
                _gameData.Balance += _gameData.CurrentGameBalance;
                Debug.WriteLine($"Общий баланс после: {_gameData.Balance}");

                // В зависимости от источника запускаем соответствующую мини-игру
                if (_sourceGame == "MiniGame2")
                {
                    _navigation.NavigateTo(new MiniGame2ViewModel(_gameData, _navigation));
                }
                else
                {
                    _navigation.NavigateTo(new MiniGame1ViewModel(_gameData, _navigation));
                }
            });

            ExitCommand = new RelayCommand(_ =>
            {
                Debug.WriteLine($"Нажата кнопка 'Выход'. Общий баланс до: {_gameData.Balance}, текущий баланс: {_gameData.CurrentGameBalance}");
                _gameData.Balance += _gameData.CurrentGameBalance;
                Debug.WriteLine($"Общий баланс после: {_gameData.Balance}");
                _navigation.NavigateTo(new MainGameScreenViewModel(_gameData, _navigation));
            });
        }
    }
}