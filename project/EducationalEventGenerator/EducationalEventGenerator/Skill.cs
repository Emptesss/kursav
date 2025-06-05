using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EducationalEventGenerator
{
    public class Skill : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; }
        public string Description { get; }
        private bool isAcquired;
        public bool IsAcquired
        {
            get => isAcquired;
            set
            {
                if (isAcquired != value)
                {
                    isAcquired = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StatusColor));
                }
            }
        }
        public bool CanAcquire { get; set; }
        public int RequiredLevel { get; }
        public Dictionary<string, int> Requirements { get; }

        public string StatusColor => IsAcquired ? "#4CAF50" : "#F44336";

        public string RequirementsText => GetRequirementsText();

        public Skill(string name, string description, int requiredLevel, Dictionary<string, int> requirements)
        {
            Name = name;
            Description = description;
            RequiredLevel = requiredLevel;
            Requirements = requirements ?? new Dictionary<string, int>();
        }

        public string GetRequirementsText()
        {
            var requirementTexts = new List<string>();

            if (RequiredLevel > 0)
                requirementTexts.Add($"Требуемый уровень: {RequiredLevel}");

            foreach (var requirement in Requirements)
            {
                switch (requirement.Key)
                {
                    case "Knowledge":
                        requirementTexts.Add($"Требуемые знания: {requirement.Value}");
                        break;
                    case "Awareness":
                        requirementTexts.Add($"Требуемая осознанность: {requirement.Value}");
                        break;
                    case "Motivation":
                        requirementTexts.Add($"Требуемая мотивация: {requirement.Value}");
                        break;
                    case "Resilience":
                        requirementTexts.Add($"Требуемая устойчивость: {requirement.Value}");
                        break;
                    case "Creativity":
                        requirementTexts.Add($"Требуемая креативность: {requirement.Value}");
                        break;
                    default:
                        requirementTexts.Add($"{requirement.Key}: {requirement.Value}");
                        break;
                }
            }

            return requirementTexts.Count > 0 ?
                "Требования:\n" + string.Join("\n", requirementTexts) :
                "Нет требований";
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}