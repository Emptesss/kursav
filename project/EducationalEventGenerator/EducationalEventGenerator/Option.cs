using System.Collections.Generic;
using System.Linq;

namespace EducationalEventGenerator
{
    public class Option
    {
        public string Text { get; }
        public Effect Effects { get; }

        public Option(string text, Effect effects)
        {
            Text = text;
            Effects = effects;
        }

        public string GetRequirementText()
        {
            var requirements = new List<string>();

            if (Effects.RequiredCreativity != null)
                requirements.Add($"Требуется креативность: {Effects.RequiredCreativity}+");

            if (!string.IsNullOrEmpty(Effects.RequiredSkill))
                requirements.Add($"Требуется навык: {Effects.RequiredSkill}");

            if (Effects.RequiredSkills != null && Effects.RequiredSkills.Any())
                requirements.Add($"Требуются навыки: {string.Join(", ", Effects.RequiredSkills)}");

            return requirements.Any() ? string.Join("\n", requirements) : "Доступно всем";
        }

        public bool CanChoose(PlayerStats stats, SkillSystem skillSystem)
        {
            if (Effects.RequiredCreativity != null && stats.Creativity < Effects.RequiredCreativity)
                return false;

            if (!string.IsNullOrEmpty(Effects.RequiredSkill) && !skillSystem.HasSkill(Effects.RequiredSkill))
                return false;

            if (Effects.RequiredSkills != null && Effects.RequiredSkills.Any(skill => !skillSystem.HasSkill(skill)))
                return false;

            return true;
        }
    }
}