using CatGame.Helpers;
using CatGame.Services;
using System.Windows.Input;

namespace CatGame.ViewModels
{
    public class PauseMenuViewModel : ViewModelBase
    {
        private readonly MiniGame1ViewModel _gameViewModel;
        private readonly NavigationService _navigation;

        public PauseMenuViewModel(MiniGame1ViewModel gameViewModel, NavigationService navigation)
        {
            _gameViewModel = gameViewModel;
            _navigation = navigation;

            InitializeCommands();
        }

        public ICommand ResumeCommand { get; private set; }
        public ICommand RestartCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }

        private void InitializeCommands()
        {
            ResumeCommand = new RelayCommand(_ => _gameViewModel.IsPaused = false);
            RestartCommand = new RelayCommand(_ => _gameViewModel.InitializeGame());
            ExitCommand = new RelayCommand(_ =>
    _navigation.NavigateTo(new MainGameScreenViewModel(_gameViewModel.GameData, _navigation)));
        }
    }
}