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
        public ICommand BuyItemCommand { get; }

        public ShopViewModel(GameData gameData)
        {
            _gameData = gameData;
            BuyItemCommand = new RelayCommand(BuyItem);
        }

        public int Balance
        {
            get { return _gameData.Balance; }
            set
            {
                _gameData.Balance = value;
                OnPropertyChanged(nameof(Balance));
            }
        }

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
            OnPropertyChanged(nameof(Balance));
        }
    }
}
