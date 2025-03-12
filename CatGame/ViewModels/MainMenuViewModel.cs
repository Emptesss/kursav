using System.Windows;
using System.Windows.Input;
using CatGame.Helpers;
using CatGame.Models;
using CatGame.Services;
using CatGame.Views;
namespace CatGame.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly GameData _gameData;
        private object _currentView;
        private bool _hasOpenRules;
        public bool HasOpenRules
        {
            get => _hasOpenRules;
            set
            {
                _hasOpenRules = value;
                OnPropertyChanged();
            }
        }
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                HasOpenRules = value != null;
                OnPropertyChanged();
            }
        }

        public MainMenuViewModel(NavigationService navigationService, GameData gameData)
        {
            _navigationService = navigationService;
            _gameData = gameData;

            PlayCommand = new RelayCommand(Play);
            RulesCommand = new RelayCommand(ShowRules);
            ExitCommand = new RelayCommand(Exit);
        }

        public ICommand PlayCommand { get; }
        public ICommand RulesCommand { get; }
        public ICommand ExitCommand { get; }

        private void Play(object parameter)
        {
            _navigationService.NavigateTo(new MainGameScreenViewModel(_gameData, _navigationService));
        }

        private void ShowRules(object parameter)
        {
            var rulesView = new RulesView();
            rulesView.DataContext = new RulesViewModel(() => CurrentView = null);
            CurrentView = rulesView;
        }

        private void Exit(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
