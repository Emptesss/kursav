using System;

namespace EducationalEventGenerator
{
    public class TemporaryEffect
    {
        public string Name { get; }
        public int KnowledgeEffect { get; set; }
        public int AwarenessEffect { get; set; }
        public int MotivationEffect { get; set; }
        public int Duration { get; set; }
        public string Color { get; set; }
        public string Description => $"{Name} ({Duration} ходов)";

        public TemporaryEffect(string name, int knowledgeEffect, int awarenessEffect, int motivationEffect, int duration)
        {
            Name = name;
            KnowledgeEffect = knowledgeEffect;
            AwarenessEffect = awarenessEffect;
            MotivationEffect = motivationEffect;
            Duration = duration;

            // Определяем цвет эффекта
            if (knowledgeEffect + awarenessEffect + motivationEffect > 0)
                Color = "#4CAF50"; // Зеленый для положительных
            else
                Color = "#F44336"; // Красный для отрицательных
        }

        // Добавим метод для создания копии эффекта
        public TemporaryEffect Clone()
        {
            return new TemporaryEffect(Name, KnowledgeEffect, AwarenessEffect, MotivationEffect, Duration);
        }

    public void Apply(PlayerStats stats)
        {
            stats.Knowledge = Math.Max(0, Math.Min(100, stats.Knowledge + KnowledgeEffect));
            stats.Awareness = Math.Max(0, Math.Min(100, stats.Awareness + AwarenessEffect));
            stats.Motivation = Math.Max(0, Math.Min(100, stats.Motivation + MotivationEffect));
        }
    }
}