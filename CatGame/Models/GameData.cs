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
                    // Сбрасываем активность предыдущего скина
                    if (_selectedSkin != null)
                        _selectedSkin.IsActive = false;

                    _selectedSkin = value;

                    // Устанавливаем активность нового скина
                    if (_selectedSkin != null)
                        _selectedSkin.IsActive = true;

                    OnPropertyChanged();
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
            IsPurchased = true
        },
        new Skin
        {
            Name = "Черничный кот",
            ImagePath = "/CatGame;component/Views/котчерника.png",
            Price = 20
        },
        new Skin
        {
            Name = "Белый кот",
            ImagePath = "/CatGame;component/Views/котбели.png",
            Price = 20
        },
        new Skin
        {
            Name = "Сиамский кот",
            ImagePath = "/CatGame;component/Views/котсиам.png",
            Price = 20
        },
        new Skin
        {
            Name = "Черный кот",
            ImagePath = "/CatGame;component/Views/котчерни.png",
            Price = 20
        }
    };
            SelectedSkin = Skins.First();
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