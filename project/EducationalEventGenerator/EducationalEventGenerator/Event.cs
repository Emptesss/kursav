using System.Collections.Generic;

namespace EducationalEventGenerator
{
    public class Event
    {
        public string Category { get; }
        public string Description { get; }
        public List<Option> Options { get; }
        public string Explanation { get; }
        public int MinLevel { get; }
        public bool IsBossEvent { get; }

        public Event(string category, string description, List<Option> options,
                    string explanation, int minLevel = 1, bool isBossEvent = false)
        {
            Category = category;
            Description = description;
            Options = options;
            Explanation = explanation;
            MinLevel = minLevel;
            IsBossEvent = isBossEvent;
        }
    }
}