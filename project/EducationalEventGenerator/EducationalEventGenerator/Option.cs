using System.Collections.Generic;
using System.Linq;

namespace EducationalEventGenerator
{
    public class Option
    {
        public string Text { get; }
        public Effect Effects { get; }
        public RequirementInfo Requirements { get; }

        public Option(string text, Effect effects)
        {
            Text = text;
            Effects = effects;
            Requirements = new RequirementInfo
            {
                RequiredCreativity = effects.RequiredCreativity ?? 0,
                RequiredSkills = effects.RequiredSkills ?? new List<string>()
            };
        }

        public bool CanChoose(PlayerStats playerStats, SkillSystem skillSystem)
        {
            // Проверяем требования к креативности
            if (Effects.RequiredCreativity > 0 && playerStats.Creativity < Effects.RequiredCreativity)
                return false;

            // Проверяем требования к устойчивости
            if (Effects.RequiredSkills?.Contains("Устойчивость") == true && playerStats.Resilience < 30)
                return false;

            // Проверяем другие требуемые навыки
            if (Effects.RequiredSkill != null && !skillSystem.HasSkill(Effects.RequiredSkill))
                return false;

            if (Effects.RequiredSkills != null)
            {
                foreach (var skill in Effects.RequiredSkills)
                {
                    if (skill != "Устойчивость" && !skillSystem.HasSkill(skill))
                        return false;
                }
            }

            return true;
        }

        public string GetRequirementText()
        {
            var requirements = new List<string>();

            if (Effects.RequiredCreativity > 0)
                requirements.Add($"Требуется креативность: {Effects.RequiredCreativity}");

            if (Effects.RequiredSkills?.Contains("Устойчивость") == true)
                requirements.Add($"Требуется устойчивость: 30");

            if (Effects.RequiredSkill != null)
                requirements.Add($"Требуется навык: {Effects.RequiredSkill}");

            if (Effects.RequiredSkills != null)
            {
                foreach (var skill in Effects.RequiredSkills.Where(s => s != "Устойчивость"))
                {
                    requirements.Add($"Требуется навык: {skill}");
                }
            }

            return string.Join("\n", requirements);
        }

        public string GetDisplayText(int playerCreativity, int playerResilience)
        {
            var requirementsText = Requirements.GetRequirementsDescription(playerCreativity, playerResilience);
            if (string.IsNullOrEmpty(requirementsText))
                return Text;

            return $"{Text}\n{requirementsText}";
        }

        public bool IsAvailable(int playerCreativity, int playerResilience)
        {
            if (Effects.RequiredCreativity > 0 && playerCreativity < Effects.RequiredCreativity)
                return false;

            if (Effects.RequiredSkills?.Contains("Устойчивость") == true && playerResilience < 30)
                return false;

            return true;
        }
    }
}