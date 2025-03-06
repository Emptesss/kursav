using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;

namespace CatGame.Behaviors
{
    public class PlaySoundBehavior : Behavior<Button>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += OnButtonClick;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Click -= OnButtonClick;
            base.OnDetaching();
        }

        private void OnButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            App.SoundService.PlayClickSound();
        }
    }
}