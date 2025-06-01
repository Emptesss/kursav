using System.Collections.Generic;

namespace EducationalEventGenerator
{
    public class Effect
    {
        public int KnowledgeEffect { get; set; }
        public int AwarenessEffect { get; set; }
        public int MotivationEffect { get; set; }
        public int? ResilienceEffect { get; set; }
        public int? CreativityEffect { get; set; }
        public int ExperienceGain { get; set; } = 5;
        public List<TemporaryEffect> TemporaryEffects { get; set; }
        public TemporaryEffect LongTermEffect { get; set; }
        public string RequiredSkill { get; set; }
        public List<string> RequiredSkills { get; set; }
        public int? RequiredCreativity { get; set; }
        public string ChainId { get; set; }

        public Effect(int knowledge, int awareness, int motivation)
        {
            KnowledgeEffect = knowledge;
            AwarenessEffect = awareness;
            MotivationEffect = motivation;
        }
    }
}