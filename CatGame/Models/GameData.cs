using CatGame.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class GameData : INotifyPropertyChanged
{
    private int _balance;
    private int _currentGameBalance;
    private ObservableCollection<Skin> _skins;
    private Skin _selectedSkin;
    private ObservableCollection<Wallpaper> _wallpapers; // Новое поле
    private Wallpaper _selectedWallpaper; // Новое поле

    public Wallpaper SelectedWallpaper // Новое свойство
    {
        get => _selectedWallpaper;
        set
        {
            if (_selectedWallpaper != value)
            {
                if (_selectedWallpaper != null)
                    _selectedWallpaper.IsActive = false;

                _selectedWallpaper = value;

                if (_selectedWallpaper != null)
                    _selectedWallpaper.IsActive = true;

                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Wallpaper> Wallpapers // Новое свойство
    {
        get => _wallpapers;
        set
        {
            _wallpapers = value;
            OnPropertyChanged();
        }
    }

    public Skin SelectedSkin
    {
        get => _selectedSkin;
        set
        {
            if (_selectedSkin != value)
            {
                if (_selectedSkin != null)
                    _selectedSkin.IsActive = false;

                _selectedSkin = value;

                if (_selectedSkin != null)
                    _selectedSkin.IsActive = true;

                OnPropertyChanged();
            }
        }
    }

    public GameData()
    {
        // Инициализация скинов
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
                Price = 400
            },
            new Skin
            {
                Name = "Белый кот",
                ImagePath = "/CatGame;component/Views/котбели.png",
                Price = 100
            },
            new Skin
            {
                Name = "Сиамский кот",
                ImagePath = "/CatGame;component/Views/котсиам.png",
                Price = 300
            },
            new Skin
            {
                Name = "Черный кот",
                ImagePath = "/CatGame;component/Views/котчерни.png",
                Price = 200
            }
        };

        // Инициализация обоев
        Wallpapers = new ObservableCollection<Wallpaper>
        {
            new Wallpaper
            {
                Name = "Базовые обои",
                ImagePath = "/CatGame;component/Views/fonmenu.png",
                Price = 0,
                IsPurchased = true
              
            },
            new Wallpaper
            {
                Name = "Фигуры круглые",
                ImagePath = "/CatGame;component/Views/фигурыкруглые.jpg",
                Price = 2
            },
            new Wallpaper
            {
                Name = "Цветы большие",
                ImagePath = "/CatGame;component/Views/цветыбольшие.jpg",
                Price = 2
            },
            new Wallpaper
            {
                Name = "Клубнички",
                ImagePath = "/CatGame;component/Views/клубнички.jpg",
                Price = 2
            }
           
        };

        // Установка начальных значений
        SelectedSkin = Skins.First();
        SelectedWallpaper = Wallpapers.First();
    }

    // Остальные свойства остаются без изменений
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