using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CatGame.Models
{
    public class GameData : INotifyPropertyChanged
    {
        private int _balance;
        private int _currentGameBalance;
        private ObservableCollection<Skin> _skins;
        private Skin _selectedSkin;
        public Skin SelectedSkin
        {
            get => _selectedSkin;
            set
            {
                if (_selectedSkin != value)
                {
                    _selectedSkin = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CurrentCatImage));
                }
            }
        }
        public string CurrentCatImage => SelectedSkin?.ImagePath;

        public GameData()
        {
            Skins = new ObservableCollection<Skin>
        {
            new Skin
            {
                Name = "Базовый кот",
                ImagePath = "/CatGame;component/Views/котправо.png",
                Price = 0,
                IsPurchased = true // Базовый скин уже куплен
            },
            new Skin
            {
                Name = "Черничный кот",
                ImagePath = "/CatGame;component/Views/котчерника.png",
                Price = 20
            },
            // ... другие скины
        };
            SelectedSkin = Skins.First(); // Установить базовый скин по умолчанию
        }

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

        public ObservableCollection<Skin> Skins
        {
            get => _skins;
            set
            {
                _skins = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}