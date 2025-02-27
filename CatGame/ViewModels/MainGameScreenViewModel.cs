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
        private readonly NavigationService _navigationService;

        public MainGameScreenViewModel(GameData gameData, NavigationService navigationService)
        {
            _gameData = gameData;
            _navigationService = navigationService;

            // Инициализация команд
            NavigateToMiniGame1Command = new RelayCommand(NavigateToMiniGame1);
            NavigateToShopCommand = new RelayCommand(NavigateToShop);
        }

        public int Balance => _gameData.Balance;

        public ICommand NavigateToMiniGame1Command { get; }
        public ICommand NavigateToShopCommand { get; }

        private void NavigateToMiniGame1(object parameter)
        {
            // Переход к мини-игре 1
            _navigationService.NavigateTo(new MiniGame1ViewModel(_gameData, _navigationService));
        }

        private void NavigateToShop(object parameter)
        {
            // Переход к магазину
            _navigationService.NavigateTo(new ShopViewModel(_gameData, _navigationService));
        }
    }
}
