namespace EducationalEventGenerator
{
    public class Skill
    {
        public string Name { get; }
        public string Description { get; }
        public bool IsAcquired { get; set; }
        public bool CanAcquire { get; set; } = true;
        public int RequiredLevel { get; }  // Добавляем новое свойство

        // Обновляем конструктор
        public Skill(string name, string description, int requiredLevel = 1)
        {
            Name = name;
            Description = description;
            RequiredLevel = requiredLevel;  // Инициализируем новое свойство
        }
    }
}