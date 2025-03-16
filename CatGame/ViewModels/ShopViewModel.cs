using CatGame.Models;
using System.Windows;
using CatGame.Services;
using CatGame.Helpers;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Data;
using CatGame.Views;
using System.Diagnostics;

namespace CatGame.ViewModels
{
    public class ShopViewModel : ViewModelBase
    {
        private readonly GameData _gameData;
        private bool _isSkinsTabSelected = true;
        private readonly NavigationService _navigationService;
        private bool _isLockersTabSelected = false;
        private bool _isWallpapersTabSelected = false;
        private bool _showInsufficientFunds;
        public bool IsSkinsTabSelected
        {
            get => _isSkinsTabSelected;
            set
            {
                if (SetProperty(ref _isSkinsTabSelected, value))
                {
                    if (value)
                    {
                        _isWallpapersTabSelected = false;
                        _isLockersTabSelected = false;
                    }
                    OnPropertyChanged(nameof(IsWallpapersTabSelected));
                    OnPropertyChanged(nameof(IsLockersTabSelected));
                    OnPropertyChanged(nameof(Wallpapers));
                    OnPropertyChanged(nameof(Skins));
                    OnPropertyChanged(nameof(Lockers));
                }
            }
        }
        public bool ShowInsufficientFunds
        {
            get => _showInsufficientFunds;
            set
            {
                _showInsufficientFunds = value;
                OnPropertyChanged(nameof(ShowInsufficientFunds));
            }
        }
        private Dictionary<string, bool> _insufficientFundsMessages = new Dictionary<string, bool>();
       
        private async Task ShowInsufficientFundsMessage(string itemId)
        {
            _insufficientFundsMessages[itemId] = true;
            OnPropertyChanged($"IsShowingInsufficientFunds_{itemId}");
            await Task.Delay(2000);
            _insufficientFundsMessages[itemId] = false;
            OnPropertyChanged($"IsShowingInsufficientFunds_{itemId}");
        }

        public bool IsWallpapersTabSelected
        {
            get => _isWallpapersTabSelected;
            set
            {
                if (SetProperty(ref _isWallpapersTabSelected, value))
                {
                    if (value)
                    {
                        _isSkinsTabSelected = false;
                        _isLockersTabSelected = false;
                    }
                    OnPropertyChanged(nameof(IsSkinsTabSelected));
                    OnPropertyChanged(nameof(IsLockersTabSelected));
                    OnPropertyChanged(nameof(Wallpapers));
                    OnPropertyChanged(nameof(Skins));
                    OnPropertyChanged(nameof(Lockers));
                }
            }
        }
        public bool IsLockersTabSelected
        {
            get => _isLockersTabSelected;
            set
            {
                if (SetProperty(ref _isLockersTabSelected, value))
                {
                    if (value)
                    {
                        _isSkinsTabSelected = false;
                        _isWallpapersTabSelected = false;
                    }
                    OnPropertyChanged(nameof(IsSkinsTabSelected));
                    OnPropertyChanged(nameof(IsWallpapersTabSelected));
                    OnPropertyChanged(nameof(Wallpapers));
                    OnPropertyChanged(nameof(Skins));
                    OnPropertyChanged(nameof(Lockers));
                }
            }
        }
        public ShopViewModel(GameData gameData, NavigationService navigationService)
        {
            _gameData = gameData;
            _navigationService = navigationService;

            BuyItemCommand = new RelayCommand(BuyItem);
            ReturnCommand = new RelayCommand(ReturnToMain);
            ExitCommand = new RelayCommand(OnExit);
            ToggleSkinCommand = new RelayCommand(ToggleSkin);
            SwitchToSkinsCommand = new RelayCommand(_ => IsSkinsTabSelected = true);
            SwitchToWallpapersCommand = new RelayCommand(_ => IsWallpapersTabSelected = true); // Было IsSkinsTabSelected = false
            BuyWallpaperCommand = new RelayCommand(BuyWallpaper);
            ToggleWallpaperCommand = new RelayCommand(ToggleWallpaper);
            SwitchToLockersCommand = new RelayCommand(_ => IsLockersTabSelected = true);
            BuyLockerCommand = new RelayCommand(BuyLocker);
            ToggleLockerCommand = new RelayCommand(ToggleLocker);


        }

        public ICommand BuyItemCommand { get; }
        public ICommand ReturnCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand ToggleSkinCommand { get; }

        // Новые команды
        public ICommand SwitchToSkinsCommand { get; }
        public ICommand SwitchToWallpapersCommand { get; }
        public ICommand BuyWallpaperCommand { get; }
        public ICommand ToggleWallpaperCommand { get; }
        public ObservableCollection<Locker> Lockers => _gameData.Lockers;
        public ICommand SwitchToLockersCommand { get; }
        public ICommand BuyLockerCommand { get; }
        public ICommand ToggleLockerCommand { get; }
        public ObservableCollection<Wallpaper> Wallpapers => _gameData.Wallpapers;
        public ObservableCollection<Skin> Skins => _gameData.Skins;
        public int Balance => _gameData.Balance;
        private async void BuyWallpaper(object parameter)
        {
            if (parameter is Wallpaper wallpaper)
            {
                if (wallpaper == _gameData.Wallpapers.First())
                    return;

                if (_gameData.Balance >= wallpaper.Price && !wallpaper.IsPurchased)
                {
                    _gameData.Balance -= wallpaper.Price;
                    wallpaper.IsPurchased = true;
                    var wallpapersView = (CollectionViewSource)Application.Current.MainWindow.Resources["PurchasableWallpapers"];
                    wallpapersView?.View.Refresh();
                }
                else if (!wallpaper.IsPurchased)
                {
                    wallpaper.ShowInsufficientFunds = true;
                    await Task.Delay(2000);
                    wallpaper.ShowInsufficientFunds = false;
                }
            }
        }
        private async void BuyLocker(object parameter)
        {
            if (parameter is Locker locker)
            {
                if (locker == _gameData.Lockers.First())
                    return;

                if (_gameData.Balance >= locker.Price && !locker.IsPurchased)
                {
                    _gameData.Balance -= locker.Price;
                    locker.IsPurchased = true;
                    var lockersView = (CollectionViewSource)Application.Current.MainWindow.Resources["PurchasableLockers"];
                    lockersView?.View.Refresh();
                }
                else if (!locker.IsPurchased)
                {
                    locker.ShowInsufficientFunds = true;
                    await Task.Delay(2000);
                    locker.ShowInsufficientFunds = false;
                }
            }
        }
        private void ToggleLocker(object parameter)
        {
            if (parameter is Locker locker)
            {
                if (locker == _gameData.Lockers.First())
                    return;

                if (_gameData.SelectedLocker == locker)
                {
                    _gameData.SelectedLocker = _gameData.Lockers.First();
                }
                else
                {
                    _gameData.SelectedLocker = locker;
                }
            }
        }
        private void ToggleWallpaper(object parameter)
        {
            if (parameter is Wallpaper wallpaper)
            {
                // Проверяем, не являются ли это базовые обои
                if (wallpaper == _gameData.Wallpapers.First())
                    return;

                if (_gameData.SelectedWallpaper == wallpaper)
                {
                    _gameData.SelectedWallpaper = _gameData.Wallpapers.First();
                }
                else
                {
                    _gameData.SelectedWallpaper = wallpaper;
                }
            }
        }


        private async void BuyItem(object parameter)
        {
            if (parameter is Skin skin)
            {
                if (_gameData.Balance >= skin.Price && !skin.IsPurchased)
                {
                    _gameData.Balance -= skin.Price;
                    skin.IsPurchased = true;
                    var skinsView = (CollectionViewSource)Application.Current.MainWindow.Resources["PurchasableSkins"];
                    skinsView?.View.Refresh();
                }
                else if (!skin.IsPurchased)
                {
                    skin.ShowInsufficientFunds = true;
                    await Task.Delay(2000);
                    skin.ShowInsufficientFunds = false;
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