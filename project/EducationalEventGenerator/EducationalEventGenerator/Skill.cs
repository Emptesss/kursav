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
        public bool IsAcquired { get; set; }
        public bool CanAcquire { get; set; }
        public int RequiredLevel { get; }
        public Dictionary<string, int> Requirements { get; }

        // Добавляем свойство для привязки в XAML
        public string RequirementsText => GetRequirementsText();

        public Skill(string name, string description, int requiredLevel, Dictionary<string, int> requirements)
        {
            Name = name;
            Description = description;
            RequiredLevel = requiredLevel;
            Requirements = requirements ?? new Dictionary<string, int>();
            Logger.Log($"Created skill: {name} with level requirement: {requiredLevel}");
        }

        public string GetRequirementsText()
        {
            var requirementTexts = new List<string>();

            if (RequiredLevel > 0)
                requirementTexts.Add($"Уровень: {RequiredLevel}");

            foreach (var requirement in Requirements)
            {
                requirementTexts.Add($"{requirement.Key}: {requirement.Value}");
            }

            var result = requirementTexts.Count > 0 ?
                "Требования:\n" + string.Join("\n", requirementTexts) :
                "Нет требований";

            Logger.Log($"GetRequirementsText for {Name}: {result}");
            return result;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}