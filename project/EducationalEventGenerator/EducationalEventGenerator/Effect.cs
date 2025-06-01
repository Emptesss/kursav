using System;
using System.Collections.Generic;

namespace EducationalEventGenerator
{
    public class Effect
    {
        // Существующие поля
        public int KnowledgeEffect { get; }
        public int AwarenessEffect { get; }
        public int MotivationEffect { get; }

        // Добавляем новое свойство для описания требования навыка
        public string SkillDescription { get; set; }

        public Effect(int knowledge, int awareness, int motivation)
        {
            KnowledgeEffect = knowledge;
            AwarenessEffect = awareness;
            MotivationEffect = motivation;
            ExperienceGain = Math.Abs(knowledge) + Math.Abs(awareness) + Math.Abs(motivation);
        }

        // Остальные существующие свойства и методы класса
        public int? ResilienceEffect { get; set; }
        public int? CreativityEffect { get; set; }
        public int ExperienceGain { get; set; }
        public string RequiredSkill { get; set; }
        public List<string> RequiredSkills { get; set; }
        public int? RequiredCreativity { get; set; }
        public string ChainId { get; set; }
        public List<TemporaryEffect> TemporaryEffects { get; set; }
        public TemporaryEffect LongTermEffect { get; set; }
    }
}