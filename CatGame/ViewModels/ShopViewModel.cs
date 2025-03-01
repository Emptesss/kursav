using CatGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using CatGame.Helpers;
using CatGame.Services;
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

            // Инициализация команды
            BuyItemCommand = new RelayCommand(BuyItem);
            ReturnCommand = new RelayCommand(ReturnToMain);
        }

        public int Balance => _gameData.Balance;

        public ICommand BuyItemCommand { get; }
        public ICommand ReturnCommand { get; }

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
                System.Windows.MessageBox.Show("Недостаточно монет!");
            }
        }
        private void ReturnToMain(object parameter)
        {
            _navigationService.NavigateTo(new MainGameScreenViewModel(_gameData, _navigationService));
        }
    }
}