using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalEventGenerator
{
    public class RequirementInfo
    {
        public int RequiredCreativity { get; set; }
        public int RequiredResilience { get; set; }
        public List<string> RequiredSkills { get; set; }
        public string Description { get; set; }

        public RequirementInfo()
        {
            RequiredSkills = new List<string>();
        }

        public string GetRequirementsDescription(int playerCreativity, int playerResilience)
        {
            var requirements = new List<string>();

            if (RequiredCreativity > 0)
            {
                string status = playerCreativity >= RequiredCreativity ? "✓" : "✗";
                requirements.Add($"Требуемая креативность: {RequiredCreativity} {status}");
            }

            if (RequiredSkills.Contains("Устойчивость"))
            {
                string status = playerResilience >= 30 ? "✓" : "✗";
                requirements.Add($"Требуемая устойчивость: 30 {status}");
            }

            if (requirements.Count == 0)
                return "";

            return "Требования:\n" + string.Join("\n", requirements);
        }
    }
}
