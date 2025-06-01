using System.Collections.Generic;
using System.Linq;

namespace EducationalEventGenerator
{
    public class SkillSystem
    {
        public List<Skill> AvailableSkills { get; } = new List<Skill>();

        // Новый метод для получения навыков по уровню
        public IEnumerable<Skill> GetAvailableSkills(int playerLevel)
        {
            return AvailableSkills.Where(skill => skill.RequiredLevel <= playerLevel);
        }

        public void InitializeSkills()
        {
            AvailableSkills.Clear();

            AvailableSkills.Add(new Skill(
                "Критическое мышление",
                "+20% к эффектам знаний",
                3  // Указываем требуемый уровень
            ));

            AvailableSkills.Add(new Skill(
                "Рефакторинг",
                "Позволяет оптимизировать код без негативных последствий",
                5  // Указываем требуемый уровень
            ));

            // Добавляем другие навыки с указанием требуемого уровня
        }

        public bool HasSkill(string skillName)
        {
            return AvailableSkills.Any(s => s.Name == skillName && s.IsAcquired);
        }
    }
}
