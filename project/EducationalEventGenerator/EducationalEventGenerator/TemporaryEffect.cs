using System;

namespace EducationalEventGenerator
{
    public class TemporaryEffect
    {
        public string Name { get; set; }
        public int KnowledgeEffect { get; set; }
        public int AwarenessEffect { get; set; }
        public int MotivationEffect { get; set; }
        public int ResilienceEffect { get; }
        public int CreativityEffect { get; }

        public int Duration { get; set; }

        public TemporaryEffect(string name, int knowledge, int awareness, int motivation, int duration,
                       int resilience = 0, int creativity = 0)
        {
            Name = name;
            KnowledgeEffect = knowledge;
            AwarenessEffect = awareness;
            MotivationEffect = motivation;
            ResilienceEffect = resilience;
            CreativityEffect = creativity;
            Duration = duration;
        }

        public TemporaryEffect Clone() => new TemporaryEffect(Name, KnowledgeEffect, AwarenessEffect, MotivationEffect, Duration, ResilienceEffect, CreativityEffect);
    }
}