using CatGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatGame.Services
{
    public class NavigationService
    {
        private ViewModelBase? _currentView;
        private static NavigationService? _instance;
        public event Action CurrentViewChanged;

        public static NavigationService Instance
        {
            get
            {
                _instance ??= new NavigationService();
                return _instance;
            }
        }

        public ViewModelBase? CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnCurrentViewChanged();
            }
        }

        private void OnCurrentViewChanged()
        {
            CurrentViewChanged?.Invoke();
        }
        public void NavigateTo(ViewModelBase viewModel)
        {
            CurrentView = viewModel;
        }
    }
}
