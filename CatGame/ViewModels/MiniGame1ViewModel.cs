using CatGame.Helpers;
using CatGame.Models;
using CatGame.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CatGame.ViewModels
{
    public class MiniGame1ViewModel : ViewModelBase
    {
        private const double CatSpeed = 30;
        private const double FoodSize = 50.0;
        private const double InitialFoodSpawnInterval = 1.5;
        private const double InitialFoodSpeed = 250;
        private const int InitialMaxFoodCount = 5;
        private const int MaxMissedFood = 3;

        private double _foodSpawnInterval = InitialFoodSpawnInterval;
        private double _foodSpeed = InitialFoodSpeed;
        private int _maxFoodCount = InitialMaxFoodCount;
        private int _missedFoodCount;
        private double _foodSpawnTimer = 0;
        private double _gameTime = 0;
        private bool _isFacingRight = true;
        private double _lastTime;
        private bool _isGamePaused;

        public static readonly double CatWidth = 260.0;
        public static readonly double CatHeight = 260.0;

        private readonly GameData _gameData;
        private readonly NavigationService _navigation;
        private GameOverViewModel _gameOverViewModel;
        public GameOverViewModel GameOverViewModel => _gameOverViewModel ??= new GameOverViewModel(_gameData, _navigation);
        private bool _isPaused;
        private Random _rnd = new Random();
        private Point _catPosition = new Point(900, 720);
        private bool _isGameOver;

        public ObservableCollection<Food> Foods { get; } = new ObservableCollection<Food>();
        public ObservableCollection<BadFood> BadFoods { get; } = new ObservableCollection<BadFood>();


        public Point CatPosition
        {
            get => _catPosition;
            set => SetProperty(ref _catPosition, value);
        }

        public MiniGame1ViewModel(GameData gameData, NavigationService navigation)
        {
            _gameData = gameData;
            _navigation = navigation;

            InitializeCommands();
            InitializeGame();
            CompositionTarget.Rendering += GameLoop;

            PauseViewModel = new PauseMenuViewModel(this, navigation);
            CompositionTarget.Rendering += (s, e) =>
            {
                if (e is RenderingEventArgs renderArgs)
                    GameLoop(s, renderArgs);
            };
        }

        public ICommand PauseCommand { get; private set; }
        public ICommand RestartCommand { get; private set; }
        public ICommand MoveCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public PauseMenuViewModel PauseViewModel { get; }

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                if (SetProperty(ref _isPaused, value))
                {
                    OnPropertyChanged(nameof(IsPausedAndNotGameOver));
                }
            }
        }

        public GameData GameData => _gameData;

        private void InitializeCommands()
        {
            PauseCommand = new RelayCommand(_ => IsPaused = !IsPaused);
            RestartCommand = new RelayCommand(_ => InitializeGame());
            ExitCommand = new RelayCommand(_ =>
                _navigation.NavigateTo(new MainGameScreenViewModel(_gameData, _navigation)));

            MoveCommand = new RelayCommand(param =>
            {
                if (param is string direction)
                {
                    Key key = direction == "Left" ? Key.Left : Key.Right;
                    MoveCat(key);
                }
            });
        }
        public bool IsFacingRight
        {
            get => _isFacingRight;
            set => SetProperty(ref _isFacingRight, value);
        }
        public int MissedFoodCount
        {
            get => _missedFoodCount;
            set
            {
                if (SetProperty(ref _missedFoodCount, value))
                {
                    OnPropertyChanged(nameof(HeartVisibilities));
                }
            }
        }
        public void InitializeGame()
        {
            Foods.Clear();
            BadFoods.Clear(); // Добавляем очистку плохой еды
            _gameData.CurrentGameBalance = 0;
            _foodSpawnTimer = 0;
            _gameTime = 0;
            _foodSpawnInterval = InitialFoodSpawnInterval;
            _foodSpeed = InitialFoodSpeed;
            _maxFoodCount = InitialMaxFoodCount;
            MissedFoodCount = 0;
            _isGamePaused = false; // Сбрасываем флаг паузы
            IsGameOver = false;
            OnPropertyChanged(nameof(HeartVisibilities));
        }

        private void GameLoop(object sender, EventArgs e)
        {
            // Проверяем и паузу, и окончание игры
            if (_isGamePaused || IsPaused || IsGameOver || !(e is RenderingEventArgs args))
                return;

            double currentTime = args.RenderingTime.TotalSeconds;
            double delta = currentTime - _lastTime;
            _lastTime = currentTime;

            IncreaseDifficulty(_gameTime);
            UpdateFoodPositions(delta);
            SpawnFood(delta);

            _gameTime += delta;
        }
        private void PlaySound(string soundFilePath)
        {
            try
            {
                var player = new MediaPlayer();
                Uri soundUri = new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, soundFilePath), UriKind.Absolute);
                player.Open(soundUri);
                player.Volume = 0.5; // Настройка громкости
                player.Play();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при воспроизведении звука: {ex.Message}");
            }
        }

        private void IncreaseDifficulty(double time)
        {
            _foodSpawnInterval = Math.Max(0.5, InitialFoodSpawnInterval - time / 35);

            _foodSpeed = InitialFoodSpeed + (time / 10) * 5;

            _maxFoodCount = 6;
        }

        private void UpdateFoodPositions(double delta)
        {
            foreach (var food in Foods.ToArray())
            {
                food.Position = new Point(food.Position.X, food.Position.Y + food.Speed * delta);

                if (CheckCollision(food))
                {
                    _gameData.CurrentGameBalance += food.Reward; // Обновляем CurrentGameBalance
                    Debug.WriteLine($"Монетка собрана! Текущий баланс: {_gameData.CurrentGameBalance}");
                    Foods.Remove(food);
                }
                else if (food.Position.Y > 1080)
                {
                    Foods.Remove(food);
                    MissedFoodCount++;
                    PlaySound("Views/roblox-death-sound-effect.mp3");
                    if (MissedFoodCount >= MaxMissedFood)
                    {
                        Debug.WriteLine($"Игра окончена! Текущий баланс: {_gameData.CurrentGameBalance}");
                        GameOver();
                    }
                }
            }
            foreach (var badFood in BadFoods.ToArray())
            {
                badFood.Position = new Point(badFood.Position.X, badFood.Position.Y + badFood.Speed * delta);

                if (CheckCollision(badFood))
                {
                    MissedFoodCount += badFood.Penalty;
                    OnPropertyChanged(nameof(HeartVisibilities));
                    PlaySound("Views/roblox-death-sound-effect.mp3");// Отнимаем жизнь
                    BadFoods.Remove(badFood);
                    if (MissedFoodCount >= MaxMissedFood)
                    {
                        GameOver();
                    }
                }
                else if (badFood.Position.Y > 1080)
                {
                    BadFoods.Remove(badFood);
                }
            }
        }

        private bool CheckCollision(Food food)
        {
            bool collisionX = food.Position.X + FoodSize > CatPosition.X &&
                              food.Position.X < CatPosition.X + CatWidth;
            bool collisionY = food.Position.Y + FoodSize > CatPosition.Y &&
                              food.Position.Y < CatPosition.Y + CatHeight;
            return collisionX && collisionY;
        }

        private void SpawnFood(double delta)
        {
            _foodSpawnTimer += delta;

            if (_foodSpawnTimer >= _foodSpawnInterval && Foods.Count < _maxFoodCount)
            {
                if (_rnd.NextDouble() < 0.5)
                {
                    AddNewFood();
                }
                _foodSpawnTimer = 0;
            }
        }

        private void AddNewFood()
        {
            if (_rnd.NextDouble() < 0.15) // 15% вероятность создания плохой еды
            {
                var badFood = new BadFood
                {
                    Position = new Point(_rnd.Next(500, 1420 - (int)FoodSize), -FoodSize),
                    Speed = _foodSpeed * (0.8 + _rnd.NextDouble() * 0.4), // Скорость от 80% до 120% от базовой
                    Reward = 0, // Плохая еда не дает награды
                    Penalty = 1, // Штраф за сбор
                    ImagePath = _rnd.Next(2) == 0
                ? "/CatGame;component/Views/рыбьякость.png"
                : "/CatGame;component/Views/яблокоогрызок.png"
                };
                BadFoods.Add(badFood);
            }
            else
            {
                var food = new Food
                {
                    Position = new Point(_rnd.Next(500, 1420 - (int)FoodSize), -FoodSize),
                    Speed = _foodSpeed * (0.8 + _rnd.NextDouble() * 0.4), // Скорость от 80% до 120% от базовой
                    Reward = 2,
                    ImagePath = _rnd.Next(2) == 0
                ? "/CatGame;component/Views/рыба.png"
                : "/CatGame;component/Views/мясо.png"
                };
                Foods.Add(food);
            }
        }

        public void MoveCat(Key key)
        {
            if (IsPaused || IsGameOver || _isGamePaused)
                return;

            double newX = CatPosition.X;
            if (key == Key.Left)
            {
                newX -= CatSpeed;
                IsFacingRight = false;
            }
            else if (key == Key.Right)
            {
                newX += CatSpeed;
                IsFacingRight = true;
            }

            newX = Math.Clamp(newX, 0, 1920 - CatWidth);
            CatPosition = new Point(newX, CatPosition.Y);
        }
        public int Lives
        {
            get => MaxMissedFood - _missedFoodCount;
        }
        public bool[] HeartVisibilities
        {
            get
            {
                var result = new[]
                {
            MissedFoodCount < 1,
        MissedFoodCount < 2,
        MissedFoodCount < 3
        };
                Debug.WriteLine($"Hearts: {result[0]} | {result[1]} | {result[2]}");
                return result;
            }
        }

        public bool IsGameOver
        {
            get => _isGameOver;
            set
            {
                if (SetProperty(ref _isGameOver, value))
                {
                    OnPropertyChanged(nameof(IsPausedAndNotGameOver));
                }
            }
        }
        public bool IsPausedAndNotGameOver
        {
            get => IsPaused && !IsGameOver;
        }
        private void GameOver()
        {
            _isGamePaused = true; // Останавливаем игровой цикл
            IsGameOver = true;
            _gameOverViewModel = new GameOverViewModel(_gameData, _navigation, "MiniGame1");
            Debug.WriteLine($"Создано меню проигрыша. Текущий баланс: {_gameData.CurrentGameBalance}");
        }
    }
}
