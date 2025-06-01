using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationalEventGenerator
{
    public class PlayerStats
    {
        private readonly SkillSystem _skillSystem;
        public List<TemporaryEffect> ActiveEffects { get; } = new List<TemporaryEffect>();
        public PlayerStats(SkillSystem skillSystem)
        {
            _skillSystem = skillSystem;
        }
        private const int BASE_RESILIENCE = 20;
        public int Level { get; set; } = 1;
        public int Experience { get; set; }
        public int ExperienceToNextLevel => Level * 10;

        public int Knowledge { get; set; } = 50;
        public int Awareness { get; set; } = 50;
        public int Motivation { get; set; } = 50;
        public int Resilience { get; set; } = 0;
        public int Creativity { get; set; } = 0;

        public void ApplyEffects(Effect effects)
        {
            // Базовый опыт за любое действие
            int experienceGain = 5;

            // Бонусный опыт за положительные эффекты
            if (effects.KnowledgeEffect > 0) experienceGain += effects.KnowledgeEffect;
            if (effects.AwarenessEffect > 0) experienceGain += effects.AwarenessEffect;
            if (effects.MotivationEffect > 0) experienceGain += effects.MotivationEffect;

            // Бонусный опыт за сложные решения (отрицательные эффекты)
            if (effects.KnowledgeEffect < 0) experienceGain += Math.Abs(effects.KnowledgeEffect) / 2;
            if (effects.AwarenessEffect < 0) experienceGain += Math.Abs(effects.AwarenessEffect) / 2;
            if (effects.MotivationEffect < 0) experienceGain += Math.Abs(effects.MotivationEffect) / 2;

            // Дополнительный опыт за временные эффекты
            if (effects.TemporaryEffects?.Any() == true)
            {
                experienceGain += 5 * effects.TemporaryEffects.Count;
            }

            // Бонус за долгосрочный эффект
            if (effects.LongTermEffect != null)
            {
                experienceGain += 10;
            }

            // Применяем модификаторы навыков
            var knowledgeModifier = _skillSystem.HasSkill("Критическое мышление") ? 1.2 : 1.0;
            var awarenessModifier = _skillSystem.HasSkill("Осознанность") ? 1.15 : 1.0;
            var motivationModifier = _skillSystem.HasSkill("Самомотивация") ? 1.1 : 1.0;

            // Применяем эффекты к характеристикам
            Knowledge = Math.Max(0, Math.Min(100, Knowledge + (int)(effects.KnowledgeEffect * knowledgeModifier)));
            Awareness = Math.Max(0, Math.Min(100, Awareness + (int)(effects.AwarenessEffect * awarenessModifier)));
            Motivation = Math.Max(0, Math.Min(100, Motivation + (int)(effects.MotivationEffect * motivationModifier)));

            // Дополнительные характеристики для высоких уровней
            if (Level >= 6)
            {
                if (effects.ResilienceEffect.HasValue)
                {
                    var resilienceModifier = _skillSystem.HasSkill("Стрессоустойчивость") ? 1.2 : 1.0;
                    Resilience = Math.Max(0, Math.Min(100, Resilience + (int)(effects.ResilienceEffect.Value * resilienceModifier)));
                }
                if (effects.CreativityEffect.HasValue)
                {
                    var creativityModifier = _skillSystem.HasSkill("Креативность") ? 1.15 : 1.0;
                    Creativity = Math.Max(0, Math.Min(100, Creativity + (int)(effects.CreativityEffect.Value * creativityModifier)));
                }

                // Бонус опыта за использование продвинутых характеристик
                if (effects.ResilienceEffect.HasValue || effects.CreativityEffect.HasValue)
                {
                    experienceGain += 5;
                }
            }

            // Применяем временные эффекты
            if (effects.TemporaryEffects != null)
            {
                foreach (var effect in effects.TemporaryEffects)
                {
                    ActiveEffects.Add(effect);
                }
            }

            // Обновляем активные эффекты
            foreach (var effect in ActiveEffects.ToList())
            {
                effect.Apply(this);
                effect.Duration--;
                if (effect.Duration <= 0)
                {
                    ActiveEffects.Remove(effect);
                }
            }

            // Добавляем бонусный опыт за высокие характеристики
            if (Knowledge >= 50) experienceGain += 2;
            if (Awareness >= 50) experienceGain += 2;
            if (Motivation >= 50) experienceGain += 2;

            // Добавляем опыт и проверяем повышение уровня
            Experience += experienceGain;

            // Логируем получение опыта
            Logger.Log($"Получено опыта: {experienceGain}. Текущий опыт: {Experience}/{ExperienceToNextLevel}");

            CheckLevelUp();
        }
        private void ApplyTemporaryEffect(TemporaryEffect effect)
        {
            Knowledge = Math.Max(0, Math.Min(100, Knowledge + effect.KnowledgeEffect));
            Awareness = Math.Max(0, Math.Min(100, Awareness + effect.AwarenessEffect));
            Motivation = Math.Max(0, Math.Min(100, Motivation + effect.MotivationEffect));
        }


        private void CheckLevelUp()
        {
            while (Experience >= ExperienceToNextLevel)
            {
                Experience -= ExperienceToNextLevel;
                Level++;

                // Бонусы при повышении уровня
                Knowledge = Math.Min(100, Knowledge + 5);
                Awareness = Math.Min(100, Awareness + 5);
                Motivation = Math.Min(100, Motivation + 5);

                // Разблокировка дополнительных характеристик
                if (Level == 6)
                {
                    Resilience = 20;
                    Logger.Log($"Разблокирована характеристика Устойчивость с базовым значением: {Resilience}");
                }

                Logger.Log($"Уровень повышен! Новый уровень: {Level}");
            }
        }
    }
}