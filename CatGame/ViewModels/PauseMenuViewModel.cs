using CatGame.Helpers;
using CatGame.Services;
using System.Windows.Input;

namespace CatGame.ViewModels
{
    public class PauseMenuViewModel : ViewModelBase
    {
        private readonly MiniGame1ViewModel _game;
        private readonly NavigationService _navigation;

        public PauseMenuViewModel(MiniGame1ViewModel game, NavigationService navigation)
        {
            _game = game;
            _navigation = navigation;

            ResumeCommand = new RelayCommand(_ => _game.IsPaused = false);
            ExitCommand = new RelayCommand(_ =>
                _navigation.NavigateTo(new MainGameScreenViewModel(_game.GameData, _navigation)));
        }

        public ICommand ResumeCommand { get; }
        public ICommand ExitCommand { get; }
    }
}