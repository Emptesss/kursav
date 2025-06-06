using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EducationalEventGenerator
{
    public class SkillSystem
    {
        private readonly List<Skill> _skills;

        public SkillSystem()
        {
            _skills = new List<Skill>();
        }
        public int GetAcquiredSkillsCount()
        {
            return _skills.Count(s => s.IsAcquired);
        }

        public void InitializeSkills()
        {
            _skills.Clear();

            AddSkill("Критическое мышление",
                "Улучшает эффективность обучения (+20% к получению знаний)",
                5,  // уровень 5
                new Dictionary<string, int> { { "Knowledge", 60 } });

            AddSkill("Осознанность",
                "Помогает лучше понимать ситуацию (+15% к получению осознанности)",
                5,  // уровень 5
                new Dictionary<string, int> { { "Awareness", 60 } });

            AddSkill("Самомотивация",
                "Повышает эффективность действий (+10% к получению мотивации)",
                5,  // уровень 5
                new Dictionary<string, int> { { "Motivation", 60 } });

            AddSkill("Стрессоустойчивость",
                "Уменьшает негативные эффекты на 30%",
                6,  // уровень 6
                new Dictionary<string, int> { { "Resilience", 40 } });

            AddSkill("Креативность",
                "Открывает новые возможности и решения",
                7,  // уровень 7
                new Dictionary<string, int> { { "Creativity", 25 } });
        }

        private void AddSkill(string name, string description, int requiredLevel, Dictionary<string, int> requirements)
        {
            var skill = new Skill(name, description, requiredLevel, requirements);
            _skills.Add(skill);
            Logger.Log($"Created skill: {name} with level requirement: {requiredLevel}");
        }

        public bool HasSkill(string skillName)
        {
            return _skills.Any(s => s.Name == skillName && s.IsAcquired);
        }

        private int GetPlayerStatValue(string statName, PlayerStats playerStats)
        {
            switch (statName)
            {
                case "Knowledge":
                    return playerStats.Knowledge;
                case "Awareness":
                    return playerStats.Awareness;
                case "Motivation":
                    return playerStats.Motivation;
                case "Resilience":
                    return playerStats.Resilience;
                case "Creativity":
                    return playerStats.Creativity;
                default:
                    return 0;
            }
        }

        public bool TryAcquireSkill(string skillName, PlayerStats playerStats)
        {
            var skill = _skills.FirstOrDefault(s => s.Name == skillName);
            if (skill == null) return false;

            // Проверка уровня
            if (playerStats.Level < skill.RequiredLevel)
            {
                MessageBox.Show($"Требуется уровень {skill.RequiredLevel} для изучения этого навыка.");
                return false;
            }

            // Проверка характеристик
            foreach (var requirement in skill.Requirements)
            {
                int playerValue = GetPlayerStatValue(requirement.Key, playerStats);
                if (playerValue < requirement.Value)
                {
                    MessageBox.Show($"Требуется {requirement.Key} {requirement.Value} для изучения этого навыка.");
                    return false;
                }
            }

            skill.IsAcquired = true;
            return true;
        }

        public List<Skill> GetAvailableSkills(PlayerStats playerStats)
        {
            return _skills
                .Select(skill =>
                {
                    if (!skill.IsAcquired)
                    {
                        bool canAcquire = playerStats.Level >= skill.RequiredLevel;
                        if (canAcquire)
                        {
                            foreach (var requirement in skill.Requirements)
                            {
                                int playerValue = GetPlayerStatValue(requirement.Key, playerStats);
                                if (playerValue < requirement.Value)
                                {
                                    canAcquire = false;
                                    break;
                                }
                            }
                        }
                        skill.CanAcquire = canAcquire;
                    }
                    return skill;
                })
                .ToList();
        }
    }
}