using CatGame.Models;
using CatGame.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.IO;

public class GameData : INotifyPropertyChanged
{
    private readonly GameDataService _gameDataService;
    private int _balance;
    private int _currentGameBalance;
    private ObservableCollection<Skin> _skins;
    private Skin _selectedSkin;
    private ObservableCollection<Wallpaper> _wallpapers;
    private Wallpaper _selectedWallpaper;
    private bool _isLoading = false;
    private ObservableCollection<Locker> _lockers;
    private Locker _selectedLocker;
    public ObservableCollection<Locker> Lockers
    {
        get => _lockers;
        set
        {
            _lockers = value;
            OnPropertyChanged();
        }
    }
    public Locker SelectedLocker
    {
        get => _selectedLocker;
        set
        {
            if (_selectedLocker != value)
            {
                if (_selectedLocker != null)
                    _selectedLocker.IsActive = false;

                _selectedLocker = value;

                if (_selectedLocker != null)
                    _selectedLocker.IsActive = true;

                OnPropertyChanged();
                if (!_isLoading) SaveGame();
            }
        }
    }

    public Wallpaper SelectedWallpaper
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
                if (!_isLoading) SaveGame();
            }
        }
    }

    public ObservableCollection<Wallpaper> Wallpapers
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
                if (!_isLoading) SaveGame();
            }
        }
    }

    public int Balance
    {
        get => _balance;
        set
        {
            if (_balance != value)
            {
                _balance = value;
                OnPropertyChanged();
                if (!_isLoading) SaveGame();
                Debug.WriteLine($"Баланс изменен на: {_balance}");
            }
        }
    }

    public int CurrentGameBalance
    {
        get => _currentGameBalance;
        set
        {
            if (_currentGameBalance != value)
            {
                _currentGameBalance = value;
                OnPropertyChanged();
                if (!_isLoading) SaveGame();
                Debug.WriteLine($"CurrentGameBalance изменен: {_currentGameBalance}");
            }
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

    public GameData()
    {
        _gameDataService = new GameDataService();
        InitializeDefaultData();
        LoadSavedData();
        if (!File.Exists(GetSaveFilePath()))
        {
            Balance = 100;
            SelectedLocker = Lockers.First(); // Добавьте эту строку
        }
    }
    private string GetSaveFilePath()
    {
        // Получаем путь к директории, где находится исполняемый файл приложения
        string basePath = AppDomain.CurrentDomain.BaseDirectory;

        // Создаем директорию Saves если её нет
        string savesDir = Path.Combine(basePath, "Saves");
        if (!Directory.Exists(savesDir))
        {
            Directory.CreateDirectory(savesDir);
        }

        // Возвращаем полный путь к файлу сохранения
        return Path.Combine(savesDir, "gamedata.json");
    }

    private void InitializeDefaultData()
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
                Price = 50
            },
            new Wallpaper
            {
                Name = "Цветы большие",
                ImagePath = "/CatGame;component/Views/цветыбольшие.jpg",
                Price = 50
            },
            new Wallpaper
            {
                Name = "Клубнички",
                ImagePath = "/CatGame;component/Views/клубнички.jpg",
                Price = 50
            },
            new Wallpaper
            {
                Name = "В горошек",
                ImagePath = "/CatGame;component/Views/вгорошек.jpg",
                Price = 50
            },
            new Wallpaper
            {
                Name = "Обои радуга",
                ImagePath = "/CatGame;component/Views/обоигея.jpg",
                Price = 50
            },
            new Wallpaper
            {
                Name = "Обои яичница",
                ImagePath = "/CatGame;component/Views/обоияички.png",
                Price = 50
            }
        };
        Lockers = new ObservableCollection<Locker>
    {
        new Locker
        {
            Name = "Базовый шкафчик",
            ImagePath = "/CatGame;component/Views/базовыйшкаф.png",
            Price = 0,
            IsPurchased = true,
            Size = 1
        },
        new Locker
        {
            Name = "С лампой",
            ImagePath = "/CatGame;component/Views/шкафчик.png",
            Price = 50,
            Size = 0
        },
        new Locker
        {
            Name = "Милый шкафчик",
            ImagePath = "/CatGame;component/Views/милыйшкаф.PNG",
            Price = 50,
            Size = 1
        }
        
    };

        // Установка начальных значений для скина и обоев
        _selectedSkin = Skins.First();
        _selectedWallpaper = Wallpapers.First();
        _selectedLocker = Lockers.First();
    }

    private void LoadSavedData()
    {
        try
        {
            _isLoading = true;
            if (File.Exists(GetSaveFilePath()))
            {
                _gameDataService.LoadGame(this);
                Debug.WriteLine($"Загружен баланс: {Balance}");
                Debug.WriteLine($"Загружены обои: {SelectedWallpaper?.Name}");
                Debug.WriteLine($"Загружен шкафчик: {SelectedLocker?.Name}"); // Добавляем это
                foreach (var wallpaper in Wallpapers)
                {
                    Debug.WriteLine($"Обои {wallpaper.Name}: {(wallpaper.IsPurchased ? "куплены" : "не куплены")}");
                }
                foreach (var locker in Lockers) // Добавляем это
                {
                    Debug.WriteLine($"Шкафчик {locker.Name}: {(locker.IsPurchased ? "куплен" : "не куплен")}");
                }
            }
            else
            {
                Debug.WriteLine("Файл сохранения не найден, используются начальные значения");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
        }
        finally
        {
            _isLoading = false;
        }
    }


    public void SaveGame()
    {
        if (_isLoading) return; // Не сохраняем во время загрузки

        try
        {
            _gameDataService.SaveGame(this);
            Debug.WriteLine("Игра сохранена успешно");
            Debug.WriteLine($"Сохранённый баланс: {Balance}");
            Debug.WriteLine($"Сохранённые обои: {SelectedWallpaper?.Name}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка при сохранении: {ex.Message}");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}