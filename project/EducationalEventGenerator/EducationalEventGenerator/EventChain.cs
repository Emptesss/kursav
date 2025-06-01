using System.Collections.Generic;

namespace EducationalEventGenerator
{
    public class EventChain
    {
        public string Id { get; }
        public List<Event> Events { get; }
        public int MinLevel { get; }

        public EventChain(string id, List<Event> events, int minLevel)
        {
            Id = id;
            Events = events;
            MinLevel = minLevel;
        }
    }
}