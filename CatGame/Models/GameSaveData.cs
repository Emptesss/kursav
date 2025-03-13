using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatGame.Models
{
    public class GameSaveData
    {
        public int Balance { get; set; }
        public int CurrentGameBalance { get; set; }
        public string SelectedSkinName { get; set; }
        public string SelectedWallpaperName { get; set; }
        public string SelectedLockerName { get; set; } // Добавляем это
        public List<string> PurchasedSkins { get; set; } = new List<string>();
        public List<string> PurchasedWallpapers { get; set; } = new List<string>();
        public List<string> PurchasedLockers { get; set; } = new List<string>(); // Добавляем это
    }
}