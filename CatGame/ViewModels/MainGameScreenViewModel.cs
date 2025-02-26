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
    public class MainGameScreenViewModel : ViewModelBase
    {
        private readonly GameData _gameData;
        public ICommand NavigateToMiniGame1Command { get; }
        public ICommand NavigateToShopCommand { get; }

        public MainGameScreenViewModel()
        {
            _gameData = new GameData { Balance = 100 }; // Пример начального баланса
            NavigateToMiniGame1Command = new RelayCommand(NavigateToMiniGame1);
            NavigateToShopCommand = new RelayCommand(NavigateToShop);
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

        private void NavigateToMiniGame1(object parameter)
        {
            NavigationService.Instance.CurrentView = new MiniGame1ViewModel(_gameData);
        }

        private void NavigateToShop(object parameter)
        {
            NavigationService.Instance.CurrentView = new ShopViewModel(_gameData);
        }
    }
}
