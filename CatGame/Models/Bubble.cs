using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CatGame.Models
{
    public class Bubble : INotifyPropertyChanged
    {
        private Point _position;
        public Point Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }

        public string Color { get; set; }
        public int Size { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}