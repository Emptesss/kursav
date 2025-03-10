// SoundBehaviorAttacher.cs
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace CatGame.Behaviors
{
    public static class SoundBehaviorAttacher
    {
        public static bool GetEnableSound(DependencyObject obj) =>
            (bool)obj.GetValue(EnableSoundProperty);

        public static void SetEnableSound(DependencyObject obj, bool value) =>
            obj.SetValue(EnableSoundProperty, value);

        public static readonly DependencyProperty EnableSoundProperty =
            DependencyProperty.RegisterAttached(
                "EnableSound",
                typeof(bool),
                typeof(SoundBehaviorAttacher),
                new PropertyMetadata(false, OnEnableSoundChanged));

        private static void OnEnableSoundChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is Button button)
            {
                var behaviors = Interaction.GetBehaviors(button);
                behaviors.Clear();

                if (e.NewValue is true)
                {
                    behaviors.Add(new PlaySoundBehavior());
                }
            }
        }
    }
}