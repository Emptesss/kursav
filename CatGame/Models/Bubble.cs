// Models/Bubble.cs
using System.Windows;
using System.Windows.Media;

namespace CatGame.Models
{
    public class Bubble : DependencyObject
    {
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Point), typeof(Bubble));

        public static readonly DependencyProperty ColorIndexProperty =
            DependencyProperty.Register("ColorIndex", typeof(int), typeof(Bubble));

        public int Row { get; set; }
        public int Column { get; set; }

        public Point Position
        {
            get => (Point)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public int ColorIndex
        {
            get => (int)GetValue(ColorIndexProperty);
            set => SetValue(ColorIndexProperty, value);
        }
    }
}