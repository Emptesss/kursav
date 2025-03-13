using CatGame.Helpers;
using CatGame.Models;
using CatGame.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace CatGame.ViewModels
{
    public class MiniGame1ViewModel : ViewModelBase
    {
        private const double CatSpeed = 20;
        private const double FoodSize = 50.0;
        private const double InitialFoodSpawnInterval = 1.5;
        private const double InitialFoodSpeed = 250;
        private const int InitialMaxFoodCount = 5;
        private const int MaxMissedFood = 3;
        private const double MinDistanceBetweenFood = 200.0;
        private Dictionary<Key, bool> _pressedKeys = new Dictionary<Key, bool>();
        private DispatcherTimer _moveTimer;
        private double _currentSpeed = 0;
        private const double MaxSpeed = 20;
        private const double Acceleration = 2.0;
        private const double Deceleration = 1.5;

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
        private bool IsTooCloseToOtherFood(Point position)
        {
            // Проверяем расстояние до хорошей еды
            foreach (var food in Foods)
            {
                double distance = Math.Sqrt(
                    Math.Pow(food.Position.X - position.X, 2) +
                    Math.Pow(food.Position.Y - position.Y, 2));
                if (distance < MinDistanceBetweenFood)
                    return true;
            }

            // Проверяем расстояние до плохой еды
            foreach (var badFood in BadFoods)
            {
                double distance = Math.Sqrt(
                    Math.Pow(badFood.Position.X - position.X, 2) +
                    Math.Pow(badFood.Position.Y - position.Y, 2));
                if (distance < MinDistanceBetweenFood)
                    return true;
            }

            return false;
        }
        private void IncreaseDifficulty(double time)
        {
            _foodSpawnInterval = Math.Max(0.5, InitialFoodSpawnInterval - time / 35);

            _foodSpeed = InitialFoodSpeed + (time / 10) * 5;

            _maxFoodCount = 6;
        }

        private void UpdateFoodPositions(double delta)
        {
            // Обновляем позиции хорошей еды
            foreach (var food in Foods.ToArray())
            {
                var newPosition = new Point(food.Position.X, food.Position.Y + food.Speed * delta);

                // Проверяем, не слишком ли близко к плохой еде
                bool isTooClose = BadFoods.Any(badFood =>
                {
                    double distance = Math.Sqrt(
                        Math.Pow(badFood.Position.X - newPosition.X, 2) +
                        Math.Pow(badFood.Position.Y - newPosition.Y, 2));
                    return distance < MinDistanceBetweenFood;
                });

                // Если еда слишком близко к плохой, немного корректируем скорость
                if (isTooClose)
                {
                    food.Speed *= 0.8; // Замедляем еду
                }

                food.Position = newPosition;

                if (CheckCollision(food))
                {
                    _gameData.CurrentGameBalance += food.Reward;
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

            // Обновляем позиции плохой еды
            foreach (var badFood in BadFoods.ToArray())
            {
                var newPosition = new Point(badFood.Position.X, badFood.Position.Y + badFood.Speed * delta);

                // Проверяем, не слишком ли близко к хорошей еде
                bool isTooClose = Foods.Any(food =>
                {
                    double distance = Math.Sqrt(
                        Math.Pow(food.Position.X - newPosition.X, 2) +
                        Math.Pow(food.Position.Y - newPosition.Y, 2));
                    return distance < MinDistanceBetweenFood;
                });

                // Если плохая еда слишком близко к хорошей, ускоряем её
                if (isTooClose)
                {
                    badFood.Speed *= 1.2; // Ускоряем плохую еду
                }

                badFood.Position = newPosition;

                if (CheckCollision(badFood))
                {
                    MissedFoodCount += badFood.Penalty;
                    OnPropertyChanged(nameof(HeartVisibilities));
                    PlaySound("Views/roblox-death-sound-effect.mp3");
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
            const int maxAttempts = 10; // Максимальное количество попыток найти подходящую позицию
            int attempts = 0;
            Point position;

            // Пытаемся найти подходящую позицию
            do
            {
                position = new Point(_rnd.Next(500, 1420 - (int)FoodSize), -FoodSize);
                attempts++;

                // Если не удалось найти подходящую позицию после maxAttempts попыток,
                // пропускаем создание еды
                if (attempts >= maxAttempts)
                    return;
            }
            while (IsTooCloseToOtherFood(position));

            // Теперь создаем еду только если нашли подходящую позицию
            if (_rnd.NextDouble() < 0.15) // 15% вероятность создания плохой еды
            {
                var badFood = new BadFood
                {
                    Position = position,
                    Speed = _foodSpeed * (0.8 + _rnd.NextDouble() * 0.4),
                    Reward = 0,
                    Penalty = 1,
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
                    Position = position,
                    Speed = _foodSpeed * (0.8 + _rnd.NextDouble() * 0.4),
                    Reward = 2,
                    ImagePath = _rnd.Next(2) == 0
                        ? "/CatGame;component/Views/рыба.png"
                        : "/CatGame;component/Views/мясо.png"
                };
                Foods.Add(food);
            }
        }

        public void MoveCat(Key key, double speed = 30.0)
        {
            if (IsPaused || IsGameOver || _isGamePaused)
                return;

            double newX = CatPosition.X;

            if (key == Key.Left)
            {
                newX -= speed;
                IsFacingRight = false;
            }
            else if (key == Key.Right)
            {
                newX += speed;
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
