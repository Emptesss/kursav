using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CatGame.Helpers;
using CatGame.Services;
using CatGame.Views;
namespace CatGame.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        public ICommand NavigateToMainGameCommand { get; }

        public MainMenuViewModel()
        {
            NavigateToMainGameCommand = new RelayCommand(NavigateToMainGame);
        }

        private void NavigateToMainGame(object parameter)
        {
            NavigationService.Instance.CurrentView = new MainGameScreenViewModel();
        }
        private ICommand _playCommand;
        private ICommand _rulesCommand;
        private ICommand _exitCommand;
        private NavigationService _navigationService;  // Добавляем NavigationService

        public MainMenuViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;  // Инициализируем NavigationService
            _playCommand = new RelayCommand(Play);
            _rulesCommand = new RelayCommand(Rules);
            _exitCommand = new RelayCommand(Exit);
        }

        public ICommand PlayCommand => _playCommand;
        public ICommand RulesCommand => _rulesCommand;
        public ICommand ExitCommand => _exitCommand;

        private void Play(object parameter)
        {
            // Создаем экземпляр MainGameScreenViewModel
            MainGameScreenViewModel gameScreenViewModel = new MainGameScreenViewModel();

            // Передаем экземпляр ViewModel в NavigateTo
            _navigationService.NavigateTo(gameScreenViewModel);
        }


        private void Rules(object parameter)
        {
            // Логика для отображения правил (например, открыть новое окно или View)
            // ...
        }

        private void Exit(object parameter)
        {
            // Логика для выхода из приложения
            System.Windows.Application.Current.Shutdown();
        }
    }

}
