using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationalEventGenerator
{
    public class PlayerStats
    {
        public readonly SkillSystem _skillSystem;
        public List<TemporaryEffect> ActiveEffects { get; } = new List<TemporaryEffect>();

        private const int BASE_RESILIENCE = 20;
        public int Level { get; set; } = 1;
        public int Experience { get; set; }

        private const int MAX_LEVEL = 15;
        public bool IsMaxLevel => Level >= MAX_LEVEL;

    
        public int MaxLevel => MAX_LEVEL;
        public int ExperienceToNextLevel => Level * 10;

        public event EventHandler<int> LevelChanged;

        public int Knowledge { get; set; } = 50;
        public int Awareness { get; set; } = 50;
        public int Motivation { get; set; } = 50;
        public int Resilience { get; set; } = 0;
        public int Creativity { get; set; } = 0;

        private int _highScore;
        public int HighScore
        {
            get => _highScore;
            private set
            {
                if (value > _highScore)
                {
                    _highScore = value;
                    HighScoreManager.SaveHighScore(value);
                    HighScoreChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler HighScoreChanged;

        public PlayerStats(SkillSystem skillSystem)
        {
            _skillSystem = skillSystem;
            HighScore = HighScoreManager.LoadHighScore();
        }

        public void ApplyEffects(Effect effects)
        {
            int startKnowledge = Knowledge;
            int startAwareness = Awareness;
            int startMotivation = Motivation;

            // Определяем базовый опыт сразу
            int expGain = effects.IsBossEvent ? 20 : 10;

            Logger.Log("\nНачало применения эффектов:");

            double damageReduction = Math.Min(0.5, Resilience / 200.0);
            Logger.Log($"Текущая устойчивость: {Resilience}, снижение урона: {damageReduction:P0}");

            if (ActiveEffects.Any())
            {
                foreach (var effect in ActiveEffects)
                {
                    Logger.Log($"Активный эффект: {effect.Name} (осталось ходов: {effect.Duration})");
                    if (effect.KnowledgeEffect != 0)
                        Logger.Log($"  Знания: {(effect.KnowledgeEffect > 0 ? "+" : "")}{effect.KnowledgeEffect}");
                    if (effect.AwarenessEffect != 0)
                        Logger.Log($"  Осознанность: {(effect.AwarenessEffect > 0 ? "+" : "")}{effect.AwarenessEffect}");
                    if (effect.MotivationEffect != 0)
                        Logger.Log($"  Мотивация: {(effect.MotivationEffect > 0 ? "+" : "")}{effect.MotivationEffect}");
                }
            }
            else
            {
                Logger.Log("Нет активных временных эффектов");
            }

            int ModifyEffect(int effect, string skillName, double bonus)
            {
                if (_skillSystem.HasSkill(skillName) && effect > 0)
                {
                    return (int)(effect * (1 + bonus));
                }
                return effect;
            }

            // Применяем базовые эффекты с учетом навыков
            int finalKnowledgeEffect = ModifyEffect(effects.KnowledgeEffect, "Критическое мышление", 0.2);
            int finalAwarenessEffect = ModifyEffect(effects.AwarenessEffect, "Осознанность", 0.15);
            int finalMotivationEffect = ModifyEffect(effects.MotivationEffect, "Самомотивация", 0.1);

            // Применяем стрессоустойчивость к негативным эффектам
            if (_skillSystem.HasSkill("Стрессоустойчивость"))
            {
                double reduction = 0.3; // 30% снижение негативных эффектов
                if (finalKnowledgeEffect < 0) finalKnowledgeEffect = (int)(finalKnowledgeEffect * (1 - reduction));
                if (finalAwarenessEffect < 0) finalAwarenessEffect = (int)(finalAwarenessEffect * (1 - reduction));
                if (finalMotivationEffect < 0) finalMotivationEffect = (int)(finalMotivationEffect * (1 - reduction));
            }

            Knowledge = ApplyStatEffect("Знания", Knowledge, finalKnowledgeEffect, damageReduction);
            Awareness = ApplyStatEffect("Осознанность", Awareness, finalAwarenessEffect, damageReduction);
            Motivation = ApplyStatEffect("Мотивация", Motivation, finalMotivationEffect, damageReduction);
            Resilience = ApplyStatEffect("Устойчивость", Resilience, effects.ResilienceEffect);
            Creativity = ApplyStatEffect("Креативность", Creativity, effects.CreativityEffect);

            if (effects.TemporaryEffects?.Any() == true)
            {
                foreach (var tempEffect in effects.TemporaryEffects)
                    ActiveEffects.Add(tempEffect.Clone());
            }

            foreach (var effect in ActiveEffects.ToList())
            {
                Knowledge = ApplyStatEffect($"[Временный] {effect.Name} → Знания", Knowledge, effect.KnowledgeEffect);
                Awareness = ApplyStatEffect($"[Временный] {effect.Name} → Осознанность", Awareness, effect.AwarenessEffect);
                Motivation = ApplyStatEffect($"[Временный] {effect.Name} → Мотивация", Motivation, effect.MotivationEffect);
                Resilience = ApplyStatEffect($"[Временный] {effect.Name} → Устойчивость", Resilience, effect.ResilienceEffect);
                Creativity = ApplyStatEffect($"[Временный] {effect.Name} → Креативность", Creativity, effect.CreativityEffect);

                effect.Duration--;
                if (effect.Duration <= 0)
                {
                    ActiveEffects.Remove(effect);
                    Logger.Log($"Эффект '{effect.Name}' завершился");
                }
                else
                {
                    Logger.Log($"Эффект '{effect.Name}': осталось ходов {effect.Duration}");
                }
            }

            // Начисляем опыт после всех эффектов
            Experience += expGain;
            Logger.Log($"Текущий опыт: {Experience}/{ExperienceToNextLevel}");

            int preLevel = Level;
            CheckLevelUp();

            // Вычисляем изменения характеристик
            int knowledgeChange = Knowledge - startKnowledge;
            int awarenessChange = Awareness - startAwareness;
            int motivationChange = Motivation - startMotivation;

            // Если произошло повышение уровня, учитываем это в изменениях
            if (Level > preLevel)
            {
                knowledgeChange -= 5;
                awarenessChange -= 5;
                motivationChange -= 5;
            }

            // Выводим изменения с правильными знаками
            if (knowledgeChange != 0)
                Logger.Log($"Итоговое изменение знаний: {(knowledgeChange > 0 ? "+" : "")}{knowledgeChange}");
            if (awarenessChange != 0)
                Logger.Log($"Итоговое изменение осознанности: {(awarenessChange > 0 ? "+" : "")}{awarenessChange}");
            if (motivationChange != 0)
                Logger.Log($"Итоговое изменение мотивации: {(motivationChange > 0 ? "+" : "")}{motivationChange}");

            Logger.Log($"Получено опыта: +{expGain}");
        }

        private int ApplyStatEffect(string label, int current, int effect, double damageReduction = 0)
        {
            if (effect == 0) return current;

            int final = effect < 0 ? (int)(effect * (1 - damageReduction)) : effect;
            int newStat = Math.Max(0, Math.Min(100, current + final));
            Logger.Log($"{label}: {current} -> {newStat} (дельта: {newStat - current})");
            return newStat;
        }

        private void CheckLevelUp()
        {
            while (Experience >= ExperienceToNextLevel && Level < MAX_LEVEL) // Добавляем проверку максимального уровня
            {
                Experience -= ExperienceToNextLevel;
                int oldLevel = Level;
                Level++;

                if (Level == 6)
                {
                    Resilience = BASE_RESILIENCE;
                    Logger.Log($"Разблокирована характеристика Устойчивость с базовым значением: {Resilience}");
                }

                // Обновляем рекорд при достижении нового уровня
                HighScore = Level;

                Logger.Log($"Уровень повышен! Новый уровень: {Level}");
                LevelChanged?.Invoke(this, Level);

                // Если достигнут максимальный уровень
                if (Level >= MAX_LEVEL)
                {
                    Experience = 0; // Обнуляем опыт при достижении максимума
                    Logger.Log("Достигнут максимальный уровень!");
                    break;
                }
            }
        }
        public void Reset()
        {
            Level = 1;
            Experience = 0;
            Knowledge = 50;
            Awareness = 50;
            Motivation = 50;
            Resilience = 0;
            Creativity = 0;
            ActiveEffects.Clear();
        }
    }
}