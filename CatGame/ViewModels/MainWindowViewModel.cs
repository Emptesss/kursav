using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CatGame.Helpers;
using CatGame.Models;
using CatGame.Services;
using CatGame.ViewModels;

namespace CatGame.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        public ICommand ToggleMusicCommand { get; }
        public MainWindowViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.CurrentViewChanged += OnCurrentViewChanged;
        }

        public MainWindowViewModel()
        {
            ToggleMusicCommand = new RelayCommand(_ => App.MusicPlayer.TogglePause());
        }

        public ViewModelBase CurrentView => _navigationService.CurrentView;

        private void OnCurrentViewChanged()
        {
            OnPropertyChanged(nameof(CurrentView));
        }
    }
}
