using System;
using System.Windows.Input;
using CatGame.Helpers;

namespace CatGame.ViewModels
{
    public class RulesViewModel : ViewModelBase
    {
        public ICommand CloseCommand { get; }

        public RulesViewModel(Action closeAction)
        {
            CloseCommand = new RelayCommand(_ => closeAction());
        }
    }
}