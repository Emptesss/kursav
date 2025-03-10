using CatGame.ViewModels;

namespace CatGame.Services
{
    public class NavigationService
    {
        private ViewModelBase? _currentView;
        private static NavigationService? _instance;

        public static NavigationService Instance => _instance ??= new NavigationService();

        public ViewModelBase CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                CurrentViewChanged?.Invoke();
            }
        }

        public event Action CurrentViewChanged;

        public void NavigateTo(ViewModelBase viewModel)
        {
            CurrentView = viewModel;
        }

    }
}
