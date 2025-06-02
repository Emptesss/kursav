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
                requirementTexts.Add($"Требуется уровень: {RequiredLevel}");

            foreach (var requirement in Requirements)
            {
                requirementTexts.Add($"{requirement.Key}: {requirement.Value}");
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