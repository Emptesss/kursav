using CatGame.Helpers;
using CatGame.Models;
using CatGame.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CatGame.ViewModels
{
    public class MiniGame1ViewModel : ViewModelBase
    {
        private readonly GameData _gameData;
        private readonly NavigationService _navigation;
        private DateTime _lastUpdate;
        private bool _isPaused;
        private string _currentColor;
        private int _score;
        private Random _rnd = new Random();
        private Point _mousePosition;
        public Point MousePosition
        {
            get => _mousePosition;
            set => SetProperty(ref _mousePosition, value);
        }

        public ObservableCollection<Bubble> Bubbles { get; } = new();
        public string CurrentColor
        {
            get => _currentColor;
            set => SetProperty(ref _currentColor, value);
        }
        public int Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }

        public MiniGame1ViewModel(GameData gameData, NavigationService navigation)
        {
            _gameData = gameData;
            _navigation = navigation;

            InitializeCommands();
            InitializeGame();
            CompositionTarget.Rendering += GameLoop;
            PauseViewModel = new PauseMenuViewModel(this, navigation);
        }

        public ICommand ShootCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand ResumeCommand { get; private set; }
        public PauseMenuViewModel PauseViewModel { get; }
        public ICommand RestartCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }


        public bool IsPaused
        {
            get => _isPaused;
            set => SetProperty(ref _isPaused, value);
        }

        private void InitializeCommands()
        {
            ShootCommand = new RelayCommand(Shoot);
            PauseCommand = new RelayCommand(_ => IsPaused = true);
            ResumeCommand = new RelayCommand(_ => IsPaused = false);
            RestartCommand = new RelayCommand(_ => InitializeGame());
            ExitCommand = new RelayCommand(_ => _navigation.NavigateTo(new MainGameScreenViewModel(_gameData, _navigation)));
        }

        public void InitializeGame()
        {
            Bubbles.Clear();
            Score = 0;
            CurrentColor = GetRandomColor();

            for (int i = 0; i < 15; i++)
            {
                AddNewBubble();
            }
        }
        public GameData GameData => _gameData;

        private void GameLoop(object sender, EventArgs e)
        {
            if (IsPaused) return;

            var now = DateTime.Now;
            var delta = (now - _lastUpdate).TotalSeconds;
            _lastUpdate = now;

            foreach (var bubble in Bubbles.ToArray())
            {
                var newX = bubble.Position.X + 50 * delta;
                var newY = bubble.Position.Y + Math.Sin(now.Ticks * 0.0000001) * 30 * delta;

                if (newX > 1920) newX = 100;
                if (newY > 1080) newY = 100;

                bubble.Position = new Point(newX, newY);
            }
        }
        private Bubble FindBubbleAtPosition(Point position)
        {
            return Bubbles.FirstOrDefault(b =>
                Math.Sqrt(
                    Math.Pow(b.Position.X - position.X, 2) +
                    Math.Pow(b.Position.Y - position.Y, 2)
                ) < b.Size
            );
        }
        private List<Bubble> FindConnectedBubbles(Bubble startBubble)
        {
            var visited = new List<Bubble>();
            var queue = new Queue<Bubble>();
            queue.Enqueue(startBubble);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                visited.Add(current);

                foreach (var bubble in Bubbles)
                {
                    if (!visited.Contains(bubble) &&
                        IsAdjacent(current, bubble) &&
                        bubble.Color == current.Color)
                    {
                        queue.Enqueue(bubble);
                    }
                }
            }
            return visited;
        }
        private bool IsAdjacent(Bubble a, Bubble b)
        {
            return Math.Sqrt(
                Math.Pow(a.Position.X - b.Position.X, 2) +
                Math.Pow(a.Position.Y - b.Position.Y, 2)
            ) < a.Size + b.Size;
        }

        private void Shoot(object parameter)
        {
            if (parameter is Point position)
            {
                var targetBubble = FindBubbleAtPosition(position);
                if (targetBubble != null && targetBubble.Color == CurrentColor)
                {
                    var connectedBubbles = FindConnectedBubbles(targetBubble);
                    if (connectedBubbles.Count >= 3)
                    {
                        foreach (var bubble in connectedBubbles)
                        {
                            Bubbles.Remove(bubble);
                            Score += connectedBubbles.Count * 10; // Бонус за группу
                        }
                        _gameData.Balance += Score / 100; // Начисление монет
                    }
                    CurrentColor = GetRandomColor();
                }
            }
        }


        private void AddNewBubble()
        {
            Bubbles.Add(new Bubble
            {
                Position = new Point(_rnd.Next(100, 1700), _rnd.Next(100, 800)),
                Color = GetRandomColor(),
                Size = _rnd.Next(30, 60)
            });
        }

        private string GetRandomColor() =>
            new[] { "#FF0000", "#0000FF", "#00FF00", "#FFFF00" }[_rnd.Next(0, 4)];
    }
}