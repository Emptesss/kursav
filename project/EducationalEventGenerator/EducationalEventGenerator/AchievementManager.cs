using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalEventGenerator
{
    public class AchievementManager
    {
        private List<Achievement> achievements;
        public event EventHandler<Achievement> AchievementUnlocked;

        public AchievementManager()
        {
            achievements = new List<Achievement>
        {
            new Achievement(
                "Мастер знаний",
                "Достигните 100 очков знаний",
                "icon.png",
                stats => stats.Knowledge >= 100
            ),
            new Achievement(
                "Мудрец",
                "Достигните 100 очков осознанности",
                "icon.png",
                stats => stats.Awareness >= 100
            ),
            new Achievement(
                "Мотиватор",
                "Достигните 100 очков мотивации",
                "icon.png",
                stats => stats.Motivation >= 100
            ),
            new Achievement(
                "Победитель",
                "Пройдите игру до конца",
                "icon.png",
                stats => stats.IsMaxLevel
            ),
            new Achievement(
                "Совершенство",
                "Закончите игру с показателями выше 80",
                "icon.png",
                stats => stats.IsMaxLevel &&
                        stats.Knowledge >= 80 &&
                        stats.Awareness >= 80 &&
                        stats.Motivation >= 80
            )
        };
        }

        public List<Achievement> GetAchievements() => achievements;

        public void CheckAchievements(PlayerStats stats)
        {
            foreach (var achievement in achievements)
            {
                if (!achievement.IsUnlocked && achievement.UnlockCondition(stats))
                {
                    achievement.IsUnlocked = true;
                    AchievementUnlocked?.Invoke(this, achievement);
                }
            }
        }
    }
}
