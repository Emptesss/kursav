using CatGame.Models;
using System.Windows.Input;
using System.Windows;
using CatGame.Services;
using CatGame.Helpers;

namespace CatGame.ViewModels
{
    public class ShopViewModel : ViewModelBase
    {
        private readonly GameData _gameData;
        private readonly NavigationService _navigationService;

        public ShopViewModel(GameData gameData, NavigationService navigationService)
        {
            _gameData = gameData;
            _navigationService = navigationService;

            // Инициализация команд
            BuyItemCommand = new RelayCommand(BuyItem);
            ReturnCommand = new RelayCommand(ReturnToMain);
            ExitCommand = new RelayCommand(OnExit);
        }

        public ICommand ExitCommand { get; }
        public ICommand BuyItemCommand { get; }
        public ICommand ReturnCommand { get; }

        public int Balance => _gameData.Balance;

        private void BuyItem(object parameter)
        {
            // Логика покупки предмета
            int itemCost = 10; // Пример стоимости предмета
            if (_gameData.Balance >= itemCost)
            {
                _gameData.Balance -= itemCost;
            }
            else
            {
                MessageBox.Show("Недостаточно монет!");
            }
        }

        private void ReturnToMain(object parameter)
        {
            _navigationService.NavigateTo(new MainGameScreenViewModel(_gameData, _navigationService));
        }

        private void OnExit()
        {
            Application.Current.MainWindow.Close(); // Закрыть окно
        }
    }
}