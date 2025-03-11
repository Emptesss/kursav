using CatGame.Helpers;
using CatGame.Models;
using CatGame.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using CatGame.Views;

namespace CatGame.ViewModels
{
    public class MiniGame2ViewModel : ViewModelBase
    {
        private const int Columns = 17;
        private const int Rows = 6;
        public static double BubbleSize => 80;
        public const double FieldWidth = 1920;
        public const double FieldHeight = 1080;
        private const double CatYPosition = 690;
        private const double CenterOffset = 100;
        private const double BubbleYOffset = 950;
        private const int ColorsCount = 6;
        private const double ShootSpeed = 1800;
        private const int MaxBottomHits = 3;
        private const double HexOffset = 0.866;
        private const double VerticalSpacing = 0.75;
        private const int MaxMoves = 6;
        private const double DeathLineYPosition = 610;

        private const double BubbleCollisionOffset = 0.5;
        private int _missCount;
        private int _allowedMisses = 3;
        private int _movesLeft = 3;
        private int _nextColor;

        private readonly Random _rnd = new Random();
        private readonly NavigationService _navigation;

        private Point _shootDirection = new Point(0, -1);
        private int _score;
        private int _bottomHits;
        private bool _isShooting;
        private bool _isPaused;
        private int _currentColor;
        private object _currentView;
        private bool _isGameOver;
        private readonly ParticleEffectService _particleEffect;
        public ParticleEffectService ParticleEffect => _particleEffect;


        private Point _catPosition;

        public ObservableCollection<Bubble> Bubbles { get; } = new ObservableCollection<Bubble>();
        public GameData GameData { get; }

        public ICommand ShootCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand MouseShootCommand { get; }

        public int Score
        {
            get => GameData.CurrentGameBalance;
            set
            {
                GameData.CurrentGameBalance = value;
                OnPropertyChanged();
            }
        }
        public int NextColor
        {
            get => _nextColor;
            private set
            {
                if (_nextColor != value)
                {
                    _nextColor = value;
                    OnPropertyChanged();
                }
            }
        }
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public bool IsGameOver
        {
            get => _isGameOver;
            set => SetProperty(ref _isGameOver, value);
        }
        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                if (_isPaused != value)
                {
                    _isPaused = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CurrentColor
        {
            get => _currentColor;
            set => SetProperty(ref _currentColor, value);
        }
        private void ShowPauseMenu()
        {
            IsPaused = true;
            CurrentView = new PauseMenuView
            {
                DataContext = new PauseMenuViewModel(this, _navigation)
            };
        }
        public void HidePauseMenu()
        {
            IsPaused = false;
            CurrentView = null;
        }

        public Point CurrentBubblePos { get; set; }

        public Point AimDirection => new Point(
         CurrentBubblePos.X + _shootDirection.X * 200,  // Начинаем от центрального шарика
         CurrentBubblePos.Y + _shootDirection.Y * 200
     );
        public Point CatPosition => _catPosition;
        public Visibility[] HeartVisibilities => new[]
        {
            _bottomHits < 1 ? Visibility.Visible : Visibility.Collapsed,
            _bottomHits < 2 ? Visibility.Visible : Visibility.Collapsed,
            _bottomHits < 3 ? Visibility.Visible : Visibility.Collapsed
        };

        public MiniGame2ViewModel(GameData gameData, NavigationService navigation)
        {
            GameData = gameData;
            _navigation = navigation;

            // Обнуляем текущий счет при старте новой игры
            GameData.CurrentGameBalance = 0;

            ShootCommand = new RelayCommand(Shoot, _ => !_isShooting);
            PauseCommand = new RelayCommand(_ => ShowPauseMenu());
            _particleEffect = new ParticleEffectService();
            MovesLeft = 3;
            NextColor = _rnd.Next(ColorsCount);

            _catPosition = new Point(
                (FieldWidth / 2) + CenterOffset,
                CatYPosition
            );
            InitializeField();
            ResetCurrentBubble();
            UpdateMoveIndicators();

            Debug.WriteLine($"Окно {FieldWidth}x{FieldHeight}");
            Debug.WriteLine($"Координаты кота: {CatPosition.X}, {CatPosition.Y}");
            Debug.WriteLine($"Прицел: {AimDirection.X}, {AimDirection.Y}");
        }

        private void InitializeField()
        {
            Bubbles.Clear();

            for (int row = 0; row < Rows; row++)
            {
                int cols = Columns - (row % 2);
                for (int col = 0; col < cols; col++)
                {
                    Bubbles.Add(new Bubble
                    {
                        Position = CalculateBubblePosition(row, col),
                        ColorIndex = _rnd.Next(ColorsCount),
                        Row = row,
                        Column = col
                    });
                }
            }
            OnPropertyChanged(nameof(Bubbles));
            Debug.WriteLine($"Field initialized with {Bubbles.Count} bubbles");
        }
        public void UpdateAimDirection(Point mousePosition)
        {
            if (_isShooting || IsPaused) return;

            // Вычисляем вектор направления от текущего шарика к позиции мыши
            double dx = mousePosition.X - CurrentBubblePos.X;
            double dy = mousePosition.Y - CurrentBubblePos.Y;

            // Нормализуем вектор
            double length = Math.Sqrt(dx * dx + dy * dy);
            if (length > 0)
            {
                _shootDirection = new Point(dx / length, dy / length);
                OnPropertyChanged(nameof(AimDirection));
            }
        }
        public void MouseShoot()
        {
            if (!_isShooting && !IsPaused)
            {
                Shoot(null);
            }
        }

        private Point CalculateBubblePosition(int row, int col)
        {
            double hexWidth = BubbleSize * HexOffset;
            double verticalSpacing = BubbleSize * VerticalSpacing;

            bool isEvenRow = row % 2 == 0;
            double xOffset = isEvenRow ? 0 : hexWidth / 2; // Смещение нечётных рядов для шестиугольной сетки

            double x = col * hexWidth + xOffset + (FieldWidth - (Columns * hexWidth)) / 2;
            double y = row * verticalSpacing;

            return new Point(x, y);
        }

        private async void Shoot(object _)
        {
            if (_isShooting || IsPaused) return;
            _isShooting = true;

            var velocity = new Point(
                _shootDirection.X * ShootSpeed,
                _shootDirection.Y * ShootSpeed
            );

            var currentPos = CurrentBubblePos;
            const double collisionCheckStep = 10.0; // Увеличиваем количество проверок
            double collisionRadius = BubbleSize * 0.7; // Убрали const, так как это вычисляемое значение
            Bubble newBubble = null; // Перемещаем объявление переменной в начало метода

            while (true)
            {
                var nextPos = new Point(
                    currentPos.X + velocity.X * 0.016,
                    currentPos.Y + velocity.Y * 0.016
                );

                // Проверяем путь между текущей и следующей позицией
                var stepX = (nextPos.X - currentPos.X) / collisionCheckStep;
                var stepY = (nextPos.Y - currentPos.Y) / collisionCheckStep;

                bool collision = false;
                Point collisionPoint = nextPos;
                Bubble collidedBubble = null;

                // Проверяем каждую точку на пути движения
                for (int i = 0; i < collisionCheckStep; i++)
                {
                    var checkPos = new Point(
                        currentPos.X + stepX * i,
                        currentPos.Y + stepY * i
                    );

                    // Проверяем все пузырьки на возможную коллизию
                    foreach (var bubble in Bubbles)
                    {
                        if (DistanceBetween(bubble.Position, checkPos) < collisionRadius)
                        {
                            collision = true;
                            collisionPoint = checkPos;
                            collidedBubble = bubble;
                            break;
                        }
                    }

                    if (collision) break;
                }

                // Обработка столкновения со стенами
                if (nextPos.X < 0 || nextPos.X > FieldWidth)
                {
                    velocity = new Point(-velocity.X, velocity.Y);
                    continue;
                }

                // Обработка столкновения с верхней границей или другими пузырьками
                if (nextPos.Y < 0 || collision)
                {
                    Debug.WriteLine($"Collision detected at ({collisionPoint.X:F1}, {collisionPoint.Y:F1})");
                    newBubble = collision ? SnapBubble(collisionPoint) : null;
                    break;
                }

                currentPos = nextPos;
                CurrentBubblePos = currentPos;
                OnPropertyChanged(nameof(CurrentBubblePos));
                await Task.Delay(16);
            }

            int removed = 0;

            if (newBubble != null)
            {
                removed = await CheckAndRemoveMatches(newBubble);
                if (removed > 0)
                {
                    MovesLeft = Math.Min(MovesLeft + 1, MaxMoves);
                }
                else
                {
                    MovesLeft--;
                }
            }
            else
            {
                MovesLeft--;
            }

            await HandleMoveAndCheckRows();
            ResetCurrentBubble();
            _isShooting = false;
        }
        // В метод SnapBubble добавьте проверку
        private Bubble SnapBubble(Point pos)
        {
            if (pos.X < 0 || pos.X > FieldWidth || pos.Y < 0)
            {
                Debug.WriteLine("❌ Invalid collision position");
                return null;
            }
            var (row, col) = CalculateGridPosition(pos);

            if (Bubbles.Any(b => b.Row == row && b.Column == col))
            {
                Debug.WriteLine($"⚠️ Position [{row},{col}] is occupied, searching for nearest free spot...");
                var newBubble = FindNearestFreePosition(pos, row, col);

                if (newBubble != null)
                {
                    Debug.WriteLine($"✅ Bubble placed at [{newBubble.Row}, {newBubble.Column}]");
                    return newBubble;
                }
                else
                {
                    Debug.WriteLine("❌ No valid position found. Bubble lost.");
                    return null; // Или можно удалить пузырь
                }
            }

            var bubble = new Bubble
            {
                Row = row,
                Column = col,
                ColorIndex = CurrentColor,
                Position = CalculateBubblePosition(row, col)
            };

            Bubbles.Add(bubble);
            OnPropertyChanged(nameof(Bubbles));

            Debug.WriteLine($"✅ Bubble placed at [{row}, {col}]");
            return bubble;
        }



        private Bubble FindNearestFreePosition(Point originalPos, int startRow, int startCol)
        {
            Debug.WriteLine($"\n=== FindNearestFreePosition from [{startRow}, {startCol}] ===");
            Debug.WriteLine($"Original position: ({originalPos.X:F1}, {originalPos.Y:F1})");

            int maxAttempts = 10; // Ограничение на количество проверок
            int attempt = 0;

            for (int distance = 1; distance < 5; distance++)
            {
                Debug.WriteLine($"Checking distance {distance}");
                for (int dr = -distance; dr <= distance; dr++)
                {
                    for (int dc = -distance; dc <= distance; dc++)
                    {
                        if (Math.Abs(dr) + Math.Abs(dc) != distance) continue;

                        int newRow = startRow + dr;
                        int newCol = startCol + dc;
                        Debug.WriteLine($"Checking position [{newRow}, {newCol}]");

                        if (!Bubbles.Any(b => b.Row == newRow && b.Column == newCol))
                        {
                            var pos = CalculateBubblePosition(newRow, newCol);
                            Debug.WriteLine($"✅ Found free position at [{newRow}, {newCol}]");

                            var bubble = new Bubble
                            {
                                Row = newRow,
                                Column = newCol,
                                Position = pos,
                                ColorIndex = CurrentColor
                            };

                            Bubbles.Add(bubble);
                            OnPropertyChanged(nameof(Bubbles));

                            Debug.WriteLine($"📌 Bubble successfully placed at [{newRow}, {newCol}]");
                            return bubble;
                        }
                    }
                }

                attempt++;
                if (attempt >= maxAttempts)
                {
                    Debug.WriteLine("⚠️ Too many attempts to find a free position. Exiting.");
                    return null;
                }
            }

            Debug.WriteLine("❌ No free position found!");
            return null;
        }



        private Bubble AddBubbleAtPosition(Point position)
        {
            var (row, col) = CalculateGridPosition(position);
            var bubble = new Bubble
            {
                ColorIndex = CurrentColor,
                Position = position,
                Row = row,
                Column = col
            };
            Bubbles.Add(bubble);
            return bubble;
        }
        private (int row, int col) CalculateGridPosition(Point pos)
        {
            double verticalSpacing = BubbleSize * VerticalSpacing;

            // Получаем актуальное количество рядов
            int maxRow = Bubbles.Any() ? Bubbles.Max(b => b.Row) + 1 : Rows;

            // Вычисляем строку
            int row = (int)Math.Round(pos.Y / verticalSpacing);
            // Ограничиваем строку диапазоном от 0 до максимальной строки
            row = Math.Max(0, Math.Min(row, maxRow));

            double hexWidth = BubbleSize * HexOffset;
            bool isEvenRow = row % 2 == 0;
            int cols = isEvenRow ? Columns : Columns - 1;

            // Вычисляем смещение для текущей строки
            double startX = (FieldWidth - cols * hexWidth) / 2;
            double relX = pos.X - startX - (isEvenRow ? 0 : hexWidth / 2);

            // Вычисляем и ограничиваем колонку
            int col = (int)Math.Floor(relX / hexWidth + 0.5);
            col = Math.Clamp(col, 0, cols - 1);

            Debug.WriteLine($"🔹 Calculated grid position: ({pos.X:F1}, {pos.Y:F1}) -> [{row}, {col}]");
            Debug.WriteLine($"🔹 Max row: {maxRow}, Is even row: {isEvenRow}, Columns in row: {cols}");

            return (row, col);
        }

        private void AddNewRow()
        {
            int newRow = -1; // Добавляем новый ряд над всеми остальными

            bool isEvenRow = newRow % 2 == 0;
            int columns = isEvenRow ? Columns : Columns - 1;

            // Опускаем все существующие ряды на один вниз
            foreach (var bubble in Bubbles)
            {
                bubble.Row += 1;
                bubble.Position = CalculateBubblePosition(bubble.Row, bubble.Column);
            }

            // Добавляем новый ряд сверху
            for (int col = 0; col < columns; col++)
            {
                var pos = CalculateBubblePosition(newRow, col);
                Bubbles.Add(new Bubble
                {
                    Row = newRow,
                    Column = col,
                    ColorIndex = _rnd.Next(ColorsCount),
                    Position = pos
                });
            }

            OnPropertyChanged(nameof(Bubbles));
        }



        private async Task<int> CheckAndRemoveMatches(Bubble triggerBubble)
        {
            var visited = new HashSet<Bubble>();
            var group = FindStrictConnectedGroup(triggerBubble, visited);

            Debug.WriteLine($"🎯 Found a group of {group.Count} bubbles");

            if (group.Count >= 3)
            {
                // Начисляем по одной монете за каждый удаленный шарик
                Score += group.Count;
                Debug.WriteLine($"Added {group.Count} coins. Current balance: {Score}");

                // Удаляем пузырьки
                foreach (var bubble in group.ToList())
                {
                    Color bubbleColor = GetBubbleColor(bubble.ColorIndex);

                    var effectPosition = new Point(
                        bubble.Position.X + BubbleSize / 2,
                        bubble.Position.Y + BubbleSize / 2 - 70
                    );

                    await _particleEffect.CreateBubblePopEffect(effectPosition, bubbleColor, BubbleSize);
                    Bubbles.Remove(bubble);
                }

                await Task.Delay(50);

                var floatingBubbles = await RemoveFloatingBubbles();

                if (floatingBubbles > 0)
                {
                    Score += floatingBubbles;
                    Debug.WriteLine($"Added {floatingBubbles} coins for floating bubbles. Current balance: {Score}");
                }

                // Добавляем ход, но проверяем ограничение
                Debug.WriteLine($"Moves left after match: {_movesLeft}");

                return group.Count + floatingBubbles;
            }

            Debug.WriteLine($"⚠ Group too small to remove ({group.Count} bubbles)");
            return 0;
        }
        private Color GetBubbleColor(int colorIndex)
        {
            return colorIndex switch
            {
                0 => Color.FromRgb(163, 51, 78),
                1 => Color.FromRgb(247, 136, 163),
                2 => Color.FromRgb(208, 133, 151),
                3 => Color.FromRgb(100, 59, 69),
                4 => Color.FromRgb(155, 81, 99),
                5 => Color.FromRgb(249, 92, 130),
                _ => Colors.White
            };
        }

        private async Task<int> RemoveFloatingBubbles()
        {
            var connectedBubbles = new HashSet<Bubble>();
            var queue = new Queue<Bubble>();

            // Сначала добавляем все шарики из верхнего ряда
            foreach (var bubble in Bubbles.Where(b => b.Row == 0))
            {
                queue.Enqueue(bubble);
                connectedBubbles.Add(bubble);
            }

            // Проходим по всем связанным шарикам
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                // Получаем всех возможных соседей
                foreach (var neighbor in GetAllPossibleNeighbors(current))
                {
                    // Если этот шарик еще не проверяли
                    if (!connectedBubbles.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        connectedBubbles.Add(neighbor);
                    }
                }
            }

            // Находим все "висящие" шарики
            var floatingBubbles = Bubbles.Where(b => !connectedBubbles.Contains(b)).ToList();

            if (floatingBubbles.Any())
            {
                foreach (var bubble in floatingBubbles)
                {
                    Color bubbleColor = GetBubbleColor(bubble.ColorIndex);
                    var effectPosition = new Point(
                        bubble.Position.X + BubbleSize / 2,
                        bubble.Position.Y + BubbleSize / 2 - 70
                    );

                    await _particleEffect.CreateBubblePopEffect(effectPosition, bubbleColor, BubbleSize);
                    Bubbles.Remove(bubble);
                }

                await Task.Delay(50);
                OnPropertyChanged(nameof(Bubbles));
                return floatingBubbles.Count;
            }

            return 0;
        }
        private IEnumerable<Bubble> GetAllPossibleNeighbors(Bubble bubble)
        {
            bool isEvenRow = bubble.Row % 2 == 0;

            // Обновляем смещения для более точного определения соседей
            var offsets = isEvenRow
                ? new[] {
            (-1, -1), (-1, 0),   // Верхние соседи
            (0, -1), (0, 1),     // Боковые соседи
            (1, -1), (1, 0)      // Нижние соседи
                }
                : new[] {
            (-1, 0), (-1, 1),    // Верхние соседи
            (0, -1), (0, 1),     // Боковые соседи
            (1, 0), (1, 1)       // Нижние соседи
                };

            foreach (var (dr, dc) in offsets)
            {
                int newRow = bubble.Row + dr;
                int newCol = bubble.Column + dc;

                // Важно: проверяем только наличие соседей сверху для определения "висящих" шариков
                if (newRow >= 0 && newCol >= 0 && newCol < Columns)
                {
                    var neighbor = Bubbles.FirstOrDefault(b =>
                        b.Row == newRow &&
                        b.Column == newCol);

                    if (neighbor != null)
                    {
                        yield return neighbor;
                    }
                }
            }
        }


        private List<Bubble> FindStrictConnectedGroup(Bubble start, HashSet<Bubble> visited)
        {
            var group = new List<Bubble>();
            var stack = new Stack<Bubble>();
            int targetColor = start.ColorIndex;

            // Добавляем начальный пузырь
            stack.Push(start);
            visited.Add(start);
            group.Add(start);

            Debug.WriteLine($"🔍 Start finding group from [{start.Row}, {start.Column}] (Color {targetColor})");

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                // Проверяем всех соседей текущего пузыря
                var neighbors = GetStrictNeighbors(current);
                foreach (var neighbor in neighbors)
                {
                    // Проверяем только не посещенные пузыри того же цвета
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        if (neighbor.ColorIndex == targetColor)
                        {
                            group.Add(neighbor);
                            stack.Push(neighbor);
                            Debug.WriteLine($"➡️ Found connected bubble [{neighbor.Row}, {neighbor.Column}]");
                        }
                    }
                }
            }

            Debug.WriteLine($"🎯 Total bubbles in group: {group.Count}");
            return group;
        }
        private IEnumerable<Bubble> GetStrictNeighbors(Bubble bubble)
        {
            var neighbors = new List<Bubble>();
            bool isEvenRow = bubble.Row % 2 == 0;

            // Определяем смещения для соседей в зависимости от четности ряда
            var offsets = isEvenRow
                ? new[] {
            (-1, -1), (-1, 0),   // Верхние соседи
            (0, -1), (0, 1),     // Боковые соседи
            (1, -1), (1, 0)      // Нижние соседи
                  }
                : new[] {
            (-1, 0), (-1, 1),    // Верхние соседи
            (0, -1), (0, 1),     // Боковые соседи
            (1, 0), (1, 1)       // Нижние соседи
                  };

            Debug.WriteLine($"🔎 Checking neighbors for bubble [{bubble.Row}, {bubble.Column}]");

            foreach (var (dr, dc) in offsets)
            {
                int newRow = bubble.Row + dr;
                int newCol = bubble.Column + dc;

                // Проверяем, что позиция находится в пределах поля
                if (newRow >= 0 && newRow < Rows + 5 && newCol >= 0 && newCol < Columns)
                {
                    var neighbor = Bubbles.FirstOrDefault(b =>
                        b.Row == newRow &&
                        b.Column == newCol);

                    if (neighbor != null)
                    {
                        neighbors.Add(neighbor);
                        Debug.WriteLine($"Found neighbor at [{neighbor.Row}, {neighbor.Column}] with color {neighbor.ColorIndex}");
                    }
                }
            }

            Debug.WriteLine($"📌 Total neighbors: {neighbors.Count}");
            return neighbors;
        }




        private double DistanceBetween(Point a, Point b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }



        private void ResetCurrentBubble()
        {
            CurrentColor = _nextColor; // Используем сохраненный следующий цвет
            NextColor = _rnd.Next(ColorsCount); // Генерируем новый следующий цвет
            CurrentBubblePos = new Point(
                FieldWidth / 2,    // По центру по горизонтали
                BubbleYOffset      // Используем константу для позиции по вертикали
            );
            OnPropertyChanged(nameof(CurrentBubblePos));
        }


        private async Task HandleMoveAndCheckRows()
        {
            if (MovesLeft > 0) return;

            MovesLeft = 3;
            AddNewRow();
            await ApplyBubbleGravity();

            var lowestBubble = Bubbles.MaxBy(b => b.Position.Y);
            if (lowestBubble != null && lowestBubble.Position.Y >= DeathLineYPosition)
            {
                GameOver();
            }

            OnPropertyChanged(nameof(Bubbles));
        }
        public int MovesLeft
        {
            get => _movesLeft;
            private set
            {
                if (_movesLeft != value)
                {
                    _movesLeft = Math.Min(value, MaxMoves);
                    OnPropertyChanged(nameof(MovesLeft));
                    UpdateMoveIndicators();
                }
            }
        }


        private async Task ApplyBubbleGravity()
        {
            // Сохраняем текущее расположение шариков относительно друг друга
            var bubbleRows = Bubbles.GroupBy(b => b.Row)
                .OrderBy(g => g.Key)
                .ToList();

            // Начинаем с самого нижнего ряда
            int currentRow = 0;
            foreach (var row in bubbleRows)
            {
                foreach (var bubble in row)
                {
                    // Просто смещаем каждый ряд вниз, сохраняя колонки
                    bubble.Row = currentRow;
                    bubble.Position = CalculateBubblePosition(bubble.Row, bubble.Column);
                }
                currentRow++;
            }

            await Task.Delay(50);
            OnPropertyChanged(nameof(Bubbles));
        }
        public ObservableCollection<int> MoveIndicators { get; } = new ObservableCollection<int>();

        private void UpdateMoveIndicators()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MoveIndicators.Clear();
                for (int i = 0; i < MovesLeft; i++) // Используем свойство вместо поля
                {
                    MoveIndicators.Add(i);
                }
            });
        }


        public static ImageSource DefaultCatImage =>
    new BitmapImage(new Uri("pack://application:,,,/CatGame;component/Views/котправо.png"));


        private void GameOver() 
        {
            IsGameOver = true;
            IsPaused = true;
            Debug.WriteLine($"Game Over! Final score: {Score}");
            _navigation.NavigateTo(new GameOverViewModel(GameData, _navigation, "MiniGame2"));
        }
    }
}