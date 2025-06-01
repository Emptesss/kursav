namespace EducationalEventGenerator
{
    public class TemporaryEffect
    {
        public string Name { get; }
        public int KnowledgeEffect { get; }
        public int AwarenessEffect { get; }
        public int MotivationEffect { get; }
        public int Duration { get; set; }

        public string Description =>
            $"{Name}: K{(KnowledgeEffect >= 0 ? "+" : "")}{KnowledgeEffect}, " +
            $"A{(AwarenessEffect >= 0 ? "+" : "")}{AwarenessEffect}, " +
            $"M{(MotivationEffect >= 0 ? "+" : "")}{MotivationEffect} " +
            $"({Duration} ходов)";

        public TemporaryEffect(string name, int knowledgeEffect, int awarenessEffect,
                             int motivationEffect, int duration)
        {
            Name = name;
            KnowledgeEffect = knowledgeEffect;
            AwarenessEffect = awarenessEffect;
            MotivationEffect = motivationEffect;
            Duration = duration;
        }
    }
}