using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EducationalEventGenerator
{
    public class AchievementManager
    {
        private List<Achievement> achievements;
        private const string SaveFilePath = "achievements.txt";
        public event EventHandler<Achievement> AchievementUnlocked;

        public AchievementManager()
        {
            LoadAchievements();
            if (achievements == null || achievements.Count == 0)
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
        }

        public void SaveAchievements()
        {
            try
            {
                var lines = achievements
                    .Select(a => $"{a.Title}|{a.Description}|{a.IconPath}|{a.IsUnlocked}")
                    .ToList();
                File.WriteAllLines(SaveFilePath, lines);
            }
            catch (Exception ex)
            {
                // Обработка ошибок сохранения
            }
        }

        public void LoadAchievements()
        {
            achievements = new List<Achievement>();
            if (File.Exists(SaveFilePath))
            {
                try
                {
                    var lines = File.ReadAllLines(SaveFilePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length == 4)
                        {
                            var achievement = CreateAchievementFromTitle(parts[0]);
                            if (achievement != null)
                            {
                                achievement.IsUnlocked = bool.Parse(parts[3]);
                                achievements.Add(achievement);
                            }
                        }
                    }
                }
                catch
                {
                    achievements = new List<Achievement>();
                }
            }
        }

        private Achievement CreateAchievementFromTitle(string title)
        {
            switch (title)
            {
                case "Мастер знаний":
                    return new Achievement(
                        "Мастер знаний",
                        "Достигните 100 очков знаний",
                        "icon.png",
                        stats => stats.Knowledge >= 100
                    );
                case "Мудрец":
                    return new Achievement(
                        "Мудрец",
                        "Достигните 100 очков осознанности",
                        "icon.png",
                        stats => stats.Awareness >= 100
                    );
                case "Мотиватор":
                    return new Achievement(
                        "Мотиватор",
                        "Достигните 100 очков мотивации",
                        "icon.png",
                        stats => stats.Motivation >= 100
                    );
                case "Победитель":
                    return new Achievement(
                        "Победитель",
                        "Пройдите игру до конца",
                        "icon.png",
                        stats => stats.IsMaxLevel
                    );
                case "Совершенство":
                    return new Achievement(
                        "Совершенство",
                        "Закончите игру с показателями выше 80",
                        "icon.png",
                        stats => stats.IsMaxLevel &&
                                stats.Knowledge >= 80 &&
                                stats.Awareness >= 80 &&
                                stats.Motivation >= 80
                    );
                default:
                    return null;
            }
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