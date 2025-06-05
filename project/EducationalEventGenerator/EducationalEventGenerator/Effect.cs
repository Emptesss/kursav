using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationalEventGenerator
{
    public class Effect
    {
        public int KnowledgeEffect { get; set; }
        public int AwarenessEffect { get; set; }
        public int MotivationEffect { get; set; }
        public int ResilienceEffect { get; set; }
        public int CreativityEffect { get; set; }
        public int ExperienceGain { get; set; }
        public string RequiredSkill { get; set; }

        public bool IsBossEvent { get; set; }
        public List<string> RequiredSkills { get; set; }
        public string SkillDescription { get; set; }
        public int? RequiredCreativity { get; set; }
        public string ChainId { get; set; }
        public List<TemporaryEffect> TemporaryEffects { get; set; }
        public TemporaryEffect LongTermEffect { get; set; }

        public Effect()
        {
            // Инициализируем значения по умолчанию
            KnowledgeEffect = 0;
            AwarenessEffect = 0;
            MotivationEffect = 0;
            ResilienceEffect = 0;
            CreativityEffect = 0;
            ExperienceGain = 0;
            RequiredSkills = new List<string>();
            TemporaryEffects = new List<TemporaryEffect>();
        }

        public Effect(int knowledge, int awareness, int motivation) : this()
        {
            KnowledgeEffect = knowledge;
            AwarenessEffect = awareness;
            MotivationEffect = motivation;
            IsBossEvent = false;
        }

        // Метод для создания модифицированной копии эффекта
        public Effect CreateModified(double difficulty, int creativity, int resilience)
        {
            var modified = new Effect
            {
                KnowledgeEffect = (int)(KnowledgeEffect * (KnowledgeEffect < 0 ? difficulty * 1.2 : 1)),
                AwarenessEffect = (int)(AwarenessEffect * (AwarenessEffect < 0 ? difficulty * 1.2 : 1)),
                MotivationEffect = (int)(MotivationEffect * (MotivationEffect < 0 ? difficulty * 0.8 : 1)),
                ResilienceEffect = (int)(ResilienceEffect * (1 + resilience / 100.0)),
                CreativityEffect = (int)(CreativityEffect * (1 + creativity / 100.0)),
                ExperienceGain = ExperienceGain,
                RequiredSkill = RequiredSkill,
                RequiredSkills = RequiredSkills?.ToList(), // Создаем новый список
                RequiredCreativity = RequiredCreativity,
                ChainId = ChainId,
                TemporaryEffects = TemporaryEffects?.Select(te => te.Clone()).ToList(),
                LongTermEffect = LongTermEffect?.Clone()
            };

            return modified;
        }
    }
}