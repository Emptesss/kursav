using CatGame.Models;
using System.Windows;
using CatGame.Services;
using CatGame.Helpers;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Data;

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

            BuyItemCommand = new RelayCommand(BuyItem);
            ReturnCommand = new RelayCommand(ReturnToMain);
            ExitCommand = new RelayCommand(OnExit);
            ToggleSkinCommand = new RelayCommand(ToggleSkin);
        }

        public ICommand ExitCommand { get; }
        public ICommand BuyItemCommand { get; }
        public ICommand ReturnCommand { get; }
        public ICommand ToggleSkinCommand { get; }

        public int Balance => _gameData.Balance;
        public ObservableCollection<Skin> Skins => _gameData.Skins;

        private void BuyItem(object parameter)
        {
            if (parameter is Skin skin)
            {
                if (_gameData.Balance >= skin.Price && !skin.IsPurchased)
                {
                    _gameData.Balance -= skin.Price;
                    // В ShopViewModel после покупки скина:
                    skin.IsPurchased = true;
                    var skinsView = (CollectionViewSource)Application.Current.MainWindow.Resources["PurchasableSkins"];
                    skinsView?.View.Refresh();
                }
            }
        }

        private void ToggleSkin(object parameter)
        {
            if (parameter is Skin skin)
            {
                if (_gameData.SelectedSkin == skin)
                {
                    _gameData.SelectedSkin = _gameData.Skins.First(); // Возврат к базовому скину
                }
                else
                {
                    _gameData.SelectedSkin = skin;
                }
            }
        }

        private void ReturnToMain(object parameter)
        {
            _navigationService.NavigateTo(new MainGameScreenViewModel(_gameData, _navigationService));
        }

        private void OnExit(object parameter)
        {
            Application.Current.MainWindow.Close();
        }
    }
}