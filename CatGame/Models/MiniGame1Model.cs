using System.Collections.ObjectModel;
using System.Windows;

namespace CatGame.Models
{
    public class MiniGame1Model
    {
        public ObservableCollection<Bubble> Bubbles { get; } = new();
        public string CurrentColor { get; set; } = "Red";
        public int Score { get; set; }
        public bool IsPaused { get; set; }
    }
}