using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace CatGame.Helpers
{
    public class MousePositionBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty MousePositionProperty =
            DependencyProperty.Register(
                "MousePosition",
                typeof(Point),
                typeof(MousePositionBehavior)
            );

        public Point MousePosition
        {
            get => (Point)GetValue(MousePositionProperty);
            set => SetValue(MousePositionProperty, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.MouseMove += (s, e) =>
                MousePosition = e.GetPosition(AssociatedObject);
        }
    }
}