using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CatGame.Models
{
    public class Food : INotifyPropertyChanged
    {
        private Point _position;
        public string ImagePath { get; set; }
        public Point Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }

        public double Speed { get; set; } = 200;
        public int Reward { get; set; } = 1;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
