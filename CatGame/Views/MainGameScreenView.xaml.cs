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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatGame.Views
{
    /// <summary>
    /// Логика взаимодействия для MainGameScreenView.xaml
    /// </summary>
    public partial class MainGameScreenView : UserControl
    {
        private bool _canJump = true;
        private readonly Storyboard _jumpStoryboard;
        public MainGameScreenView()
        {
            InitializeComponent();

            // Создаем анимацию прыжка
            _jumpStoryboard = new Storyboard();

            // Анимация движения вверх-вниз
            var jumpAnimation = new DoubleAnimationUsingKeyFrames();
            jumpAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.Zero)));
            jumpAnimation.KeyFrames.Add(new SplineDoubleKeyFrame(-50, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(250)), new KeySpline(0.1, 0.5, 0.2, 1.0)));
            jumpAnimation.KeyFrames.Add(new SplineDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500)), new KeySpline(0.8, 0, 0.9, 0.5)));

            Storyboard.SetTargetName(jumpAnimation, "CatTranslate");
            Storyboard.SetTargetProperty(jumpAnimation, new PropertyPath(TranslateTransform.YProperty));

            _jumpStoryboard.Children.Add(jumpAnimation);
            _jumpStoryboard.Completed += JumpStoryboard_Completed;
        }
        private async void CatImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_canJump) return;

            _canJump = false;
            _jumpStoryboard.Begin(this);

            // Анимация тени
            var transformGroup = (TransformGroup)CatShadow.RenderTransform;
            var shadowScale = (ScaleTransform)transformGroup.Children[0];

            var shadowAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.7,
                Duration = TimeSpan.FromMilliseconds(250),
                AutoReverse = true
            };

            shadowScale.BeginAnimation(ScaleTransform.ScaleXProperty, shadowAnimation);
            shadowScale.BeginAnimation(ScaleTransform.ScaleYProperty, shadowAnimation);
        }
        private void JumpStoryboard_Completed(object sender, EventArgs e)
        {
            // Добавляем небольшую задержку перед следующим возможным прыжком
            Task.Delay(500).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() => _canJump = true);
            });
        }
    }
}
