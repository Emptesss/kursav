using CatGame.Models;
using System.Windows.Input;
using CatGame.Helpers;
using CatGame.Services;
using System.Windows;
using CatGame.Views;
namespace CatGame.ViewModels
{
    public class MainGameScreenViewModel : ViewModelBase
    {
        private readonly GameData _gameData;
        private readonly NavigationService _navigationService;
        private object _currentView;
        private string _currentProfileAvatar;
        private bool _hasOpenProfile;
        private string _catName;
        public string SelectedLockerName => _gameData.SelectedLocker?.Name;
        public string SelectedLockerPath => _gameData.SelectedLocker?.ImagePath;
        public string CatName => _gameData.CatProfile?.Name ?? "";
        public MainGameScreenViewModel(GameData gameData, NavigationService navigationService)

        {
            _gameData = gameData;
            _navigationService = navigationService;
            _currentProfileAvatar = _gameData.CatProfile?.AvatarPath ?? _gameData.AvatarPaths[0];
            _gameData.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_gameData.SelectedSkin))
                {
                    OnPropertyChanged(nameof(CurrentCatImage));
                }
                if (e.PropertyName == nameof(_gameData.SelectedWallpaper))
                {
                    OnPropertyChanged(nameof(SelectedWallpaperPath));
                }
                if (e.PropertyName == nameof(_gameData.CatProfile))
                {
                    UpdateProfileData();
                    OnPropertyChanged(nameof(CatName)); // Добавьте это
                }
                if (e.PropertyName == nameof(GameData.SelectedLocker))
                {
                    OnPropertyChanged(nameof(SelectedLockerPath));
                    OnPropertyChanged(nameof(SelectedLockerName));
                }
                // Добавляем обработку изменения профиля
                if (e.PropertyName == nameof(_gameData.CatProfile))
                {
                    OnPropertyChanged(nameof(CatName));
                }

            };
            // Инициализация команд
            NavigateToMiniGame1Command = new RelayCommand(NavigateToMiniGame1);
            OpenProfileCommand = new RelayCommand(OpenProfile);
            OpenMiniGamesMenuCommand = new RelayCommand(OpenMiniGamesMenu);
            NavigateToShopCommand = new RelayCommand(NavigateToShop);
            ExitCommand = new RelayCommand(ExitGame);

        }
        public string SelectedWallpaperPath => _gameData.SelectedWallpaper?.ImagePath ?? "/Views/fonmenu.png";
        public string CurrentCatImage => _gameData.SelectedSkin?.ImagePath;
        public string CurrentProfileAvatar
        {
            get => _currentProfileAvatar;
            set => SetProperty(ref _currentProfileAvatar, value);
        }
        public int Balance => _gameData.Balance;

        public ICommand NavigateToMiniGame1Command { get; }
        public ICommand NavigateToShopCommand { get; }
        public ICommand OpenProfileCommand { get; }

        public ICommand OpenMiniGamesMenuCommand { get; }
        public ICommand ExitCommand { get; }

        private void NavigateToMiniGame1(object parameter)
        {
            // Переход к мини-игре 1
            _navigationService.NavigateTo(new MiniGame1ViewModel(_gameData, _navigationService));
        }
        public bool HasOpenProfile
        {
            get => _hasOpenProfile;
            set
            {
                _hasOpenProfile = value;
                OnPropertyChanged();
            }

        }
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                HasOpenProfile = value != null;
                OnPropertyChanged();
            }
        }
        private void OnProfileClosed()
        {
            HasOpenProfile = false;
            UpdateProfileData();
        }
        private void UpdateProfileData()
        {
            OnPropertyChanged(nameof(CatName));
            CurrentProfileAvatar = _gameData.CatProfile?.AvatarPath ?? _gameData.AvatarPaths[0];
            OnPropertyChanged(nameof(CurrentProfileAvatar));
        }

        private void NavigateToShop(object parameter)
        {
            // Переход к магазину
            _navigationService.NavigateTo(new ShopViewModel(_gameData, _navigationService));
        }
       
        private void OpenMiniGamesMenu(object parameter)
        {
            _navigationService.NavigateTo(new MiniGamesMenuViewModel(_gameData, _navigationService));
        }
        private void ExitGame(object parameter)
        {
            Application.Current.Shutdown();
        }
        private void OpenProfile(object parameter)
        {
            var profileView = new CatProfileView();
            profileView.DataContext = new CatProfileViewModel(_gameData, () =>
            {
                CurrentView = null;
                _navigationService.NavigateTo(this);
            });
            CurrentView = profileView;
        }
    }
}
