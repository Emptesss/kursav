using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalEventGenerator
{
    public class Achievement
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsUnlocked { get; set; }
        public string IconPath { get; set; }
        public Func<PlayerStats, bool> UnlockCondition { get; set; }

        public Achievement(string title, string description, string iconPath, Func<PlayerStats, bool> condition)
        {
            Title = title;
            Description = description;
            IconPath = iconPath;
            UnlockCondition = condition;
            IsUnlocked = false;
        }
    }
}
