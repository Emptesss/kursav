using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CatGame.Models
{
    public class GameData : INotifyPropertyChanged
    {
        private int _balance;
        private int _currentGameBalance;

        public int Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged();
            }
        }

        public int CurrentGameBalance
        {
            get => _currentGameBalance;
            set
            {
                _currentGameBalance = value;
                OnPropertyChanged();
                Debug.WriteLine($"CurrentGameBalance изменен: {_currentGameBalance}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}