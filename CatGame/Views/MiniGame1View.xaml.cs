using CatGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CatGame.Views
{
    /// <summary>
    /// Логика взаимодействия для MiniGame1View.xaml
    /// </summary>
    public partial class MiniGame1View : UserControl
    {
        private Dictionary<Key, bool> _pressedKeys = new Dictionary<Key, bool>();
        private DispatcherTimer _moveTimer;
        private double _currentSpeed = 0;
        private const double MaxSpeed = 30;
        private const double Acceleration = 2.0;
        private const double Deceleration = 1.5;
        public MiniGame1View()
        {
            InitializeComponent();
            this.Loaded += (s, e) => GameCanvas.Focus();
            this.IsVisibleChanged += (s, e) =>
            {
                if ((bool)e.NewValue)
                {
                    GameCanvas.Focus();
                }
            };

            // Инициализация таймера для плавного движения
            _moveTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
            };
            _moveTimer.Tick += MoveTimer_Tick;

            // Инициализация словаря клавиш
            _pressedKeys[Key.Left] = false;
            _pressedKeys[Key.Right] = false;

            // Добавляем обработчик для возвращения фокуса при клике
            this.MouseDown += (s, e) => GameCanvas.Focus();
        }
        private void GameCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameCanvas.Focus();
            e.Handled = true;
        }
        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            GameCanvas.Focus(); // Устанавливаем фокус на Canvas при загрузке
        }
        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                _pressedKeys[e.Key] = true;
                if (!_moveTimer.IsEnabled)
                {
                    _moveTimer.Start();
                }
                e.Handled = true; // Предотвращаем дальнейшую обработку события
            }
        }


        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                _pressedKeys[e.Key] = false;

                // Останавливаем таймер, если ни одна клавиша не нажата
                if (!_pressedKeys.Values.Any(pressed => pressed))
                {
                    _moveTimer.Stop();
                }
                e.Handled = true; // Предотвращаем дальнейшую обработку события
            }
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            var viewModel = DataContext as MiniGame1ViewModel;
            if (viewModel == null) return;

            bool isMoving = _pressedKeys[Key.Left] || _pressedKeys[Key.Right];

            if (isMoving)
            {
                _currentSpeed = Math.Min(_currentSpeed + Acceleration, MaxSpeed);
            }
            else
            {
                _currentSpeed = Math.Max(_currentSpeed - Deceleration, 0);
                if (_currentSpeed == 0)
                {
                    _moveTimer.Stop();
                    return;
                }
            }

            if (_pressedKeys[Key.Left])
                viewModel.MoveCat(Key.Left, _currentSpeed);
            if (_pressedKeys[Key.Right])
                viewModel.MoveCat(Key.Right, _currentSpeed);
        }
    }
}