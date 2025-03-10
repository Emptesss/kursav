using System.Windows;
using System.Windows.Media;

namespace CatGame.Models
{
    public class BubbleParticle
    {
        public Point Position { get; set; }
        public Vector Velocity { get; set; }
        public double Size { get; set; }
        public double Opacity { get; set; }
        public Color Color { get; set; }
        public double LifeTime { get; set; }
        public double Rotation { get; set; }
        public bool IsShard { get; set; } // Признак, является ли частица осколком
    }
}