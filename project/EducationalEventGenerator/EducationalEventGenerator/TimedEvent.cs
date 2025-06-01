using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalEventGenerator
{
    public class TimedEvent : Event
    {
        public int TimeLimit { get; } // Время в секундах
        public Effect TimeoutEffect { get; } // Эффект при истечении времени

        public TimedEvent(string category, string description, List<Option> options,
                         string explanation, int timeLimit, Effect timeoutEffect,
                         int minLevel = 1, bool isBossEvent = false)
            : base(category, description, options, explanation, minLevel, isBossEvent)
        {
            TimeLimit = timeLimit;
            TimeoutEffect = timeoutEffect;
        }
    }
}
