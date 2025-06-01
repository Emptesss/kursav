using System;

namespace EducationalEventGenerator
{
    public class PlayerStats
    {
        public int Level { get; set; } = 1;
        public int Experience { get; set; }
        public int ExperienceToNextLevel => Level * 1;

        public int Knowledge { get; set; } = 50;
        public int Awareness { get; set; } = 50;
        public int Motivation { get; set; } = 50;
        public int Resilience { get; set; } = 0;
        public int Creativity { get; set; } = 0;

        public void ApplyEffects(Effect effects)
        {
            Knowledge = Math.Max(0, Math.Min(100, Knowledge + effects.KnowledgeEffect));
            Awareness = Math.Max(0, Math.Min(100, Awareness + effects.AwarenessEffect));
            Motivation = Math.Max(0, Math.Min(100, Motivation + effects.MotivationEffect));

            if (Level >= 6)
            {
                Resilience = Math.Max(0, Math.Min(100, Resilience + (effects.ResilienceEffect ?? 0)));
                Creativity = Math.Max(0, Math.Min(100, Creativity + (effects.CreativityEffect ?? 0)));
            }

            Experience += effects.ExperienceGain;
            CheckLevelUp();
        }

        private void CheckLevelUp()
        {
            while (Experience >= ExperienceToNextLevel && Level < 15)
            {
                Experience -= ExperienceToNextLevel;
                Level++;

                if (Level % 3 == 0)
                {
                    Resilience = Math.Max(0, Math.Min(100, Resilience + 5));
                    Creativity = Math.Max(0, Math.Min(100, Creativity + 5));
                }
            }
        }
    }
}