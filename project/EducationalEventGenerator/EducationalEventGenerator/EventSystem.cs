using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationalEventGenerator
{
    public class EventSystem
    {
        private List<Event> baseEvents = new List<Event>();
        private List<Event> timedEvents = new List<Event>();
        private List<EventChain> eventChains = new List<EventChain>();
        private Random random = new Random();

        private const int HISTORY_SIZE = 10;

        // Трекер использованных событий
        private HashSet<int> usedBaseEventIndices = new HashSet<int>();
        private HashSet<int> usedTimedEventIndices = new HashSet<int>();
        private int lastBaseEventIndex = -1;
        private int lastTimedEventIndex = -1;

        private Queue<int> recentBaseEvents = new Queue<int>();
        private Queue<int> recentTimedEvents = new Queue<int>();
        private Queue<int> recentBossEvents = new Queue<int>();
        private Queue<int> recentCreativeEvents = new Queue<int>();

        private void AddToHistory(int eventIndex, Queue<int> history)
        {
            history.Enqueue(eventIndex);
            if (history.Count > HISTORY_SIZE)
            {
                history.Dequeue();
            }
        }
        private Event GetRandomTimedEvent(int playerLevel)
        {
            var availableEvents = timedEvents
                .Select((e, index) => new { Event = e, Index = index })
                .Where(item => !recentTimedEvents.Contains(item.Index) && item.Event.MinLevel <= playerLevel)
                .ToList();

            if (!availableEvents.Any())
            {
                recentTimedEvents.Clear();
                availableEvents = timedEvents
                    .Select((e, index) => new { Event = e, Index = index })
                    .Where(item => item.Event.MinLevel <= playerLevel)
                    .ToList();

                if (!availableEvents.Any())
                    return null;
            }

            var selectedEventInfo = availableEvents[random.Next(availableEvents.Count)];
            AddToHistory(selectedEventInfo.Index, recentTimedEvents);
            return selectedEventInfo.Event;
        }

        public void InitializeEvents()
        {
            baseEvents.Clear();
            timedEvents.Clear();
            eventChains.Clear();
            usedBaseEventIndices.Clear();
            usedTimedEventIndices.Clear();
            lastBaseEventIndex = -1;
            lastTimedEventIndex = -1;

            AddBasicEvents();
            AddTimedEvents();
            AddIntermediateEvents();
            AddBasicTrainingEvents();
            AddCombinedSkillEvents();
            AddAdvancedEvents();
            AddCreativityEvents();
            AddResilienceBossEvents();
            InitializeEventChains();
        }

        public Event GenerateEvent(int playerLevel, int creativity = 0, int resilience = 0)
        {
            // Сначала проверяем таймерные события (20% шанс)
            if (random.NextDouble() < 0.20)
            {
                var timedEvent = GetRandomTimedEvent(playerLevel);
                if (timedEvent != null)
                    return timedEvent;
            }

            // Проверяем босс-события
            double bossChance = Math.Min(0.3, 0.1 + (playerLevel - 6) * 0.02 + (creativity + resilience) / 400.0);
            if (playerLevel >= 6 && random.NextDouble() < bossChance)
            {
                var bossEvent = GetRandomBossEvent(playerLevel);
                if (bossEvent != null)
                    return bossEvent;
            }

            // Проверяем креативные события
            double creativeChance = Math.Min(0.4, 0.1 + (creativity / 100.0));
            if (playerLevel >= 6 && random.NextDouble() < creativeChance)
            {
                var creativeEvent = GetRandomCreativeEvent(playerLevel);
                if (creativeEvent != null)
                    return creativeEvent;
            }

            // Получаем доступные события, исключая недавно использованные
            var availableEvents = baseEvents
                .Select((e, index) => new { Event = e, Index = index })
                .Where(item => !recentBaseEvents.Contains(item.Index) && item.Event.MinLevel <= playerLevel)
                .ToList();

            // Если доступных событий мало, очищаем историю
            if (availableEvents.Count < 3)
            {
                recentBaseEvents.Clear();
                availableEvents = baseEvents
                    .Select((e, index) => new { Event = e, Index = index })
                    .Where(item => item.Event.MinLevel <= playerLevel)
                    .ToList();
            }

            if (!availableEvents.Any())
                return null;

            // Выбираем случайное событие
            var selectedEventInfo = availableEvents[random.Next(availableEvents.Count)];
            var selectedEvent = selectedEventInfo.Event;

            // Добавляем в историю
            AddToHistory(selectedEventInfo.Index, recentBaseEvents);

            // Фильтруем и модифицируем опции
            var modifiedOptions = selectedEvent.Options
                .Where(opt => IsOptionAvailable(opt, creativity, resilience))
                .Select(opt => ModifyOption(opt, playerLevel, selectedEvent.MinLevel, creativity, resilience))
                .ToList();

            if (!modifiedOptions.Any())
                return GenerateEvent(playerLevel, creativity, resilience);

            return new Event(
                selectedEvent.Category,
                selectedEvent.Description,
                modifiedOptions,
                selectedEvent.Explanation,
                selectedEvent.MinLevel,
                selectedEvent.IsBossEvent
            );
        }

        private Event GetRandomBossEvent(int playerLevel)
        {
            var availableEvents = baseEvents
                .Select((e, index) => new { Event = e, Index = index })
                .Where(item =>
                    !recentBossEvents.Contains(item.Index) &&
                    item.Event.IsBossEvent &&
                    item.Event.MinLevel <= playerLevel)
                .ToList();

            if (!availableEvents.Any())
            {
                recentBossEvents.Clear();
                availableEvents = baseEvents
                    .Select((e, index) => new { Event = e, Index = index })
                    .Where(item => item.Event.IsBossEvent && item.Event.MinLevel <= playerLevel)
                    .ToList();

                if (!availableEvents.Any())
                    return null;
            }

            var selectedEventInfo = availableEvents[random.Next(availableEvents.Count)];
            AddToHistory(selectedEventInfo.Index, recentBossEvents);
            return selectedEventInfo.Event;
        }

        private Event GetRandomCreativeEvent(int playerLevel)
        {
            var availableEvents = baseEvents
                .Select((e, index) => new { Event = e, Index = index })
                .Where(item =>
                    !recentCreativeEvents.Contains(item.Index) &&
                    item.Event.Options.Any(o => o.Effects.CreativityEffect > 0) &&
                    item.Event.MinLevel <= playerLevel)
                .ToList();

            if (!availableEvents.Any())
            {
                recentCreativeEvents.Clear();
                availableEvents = baseEvents
                    .Select((e, index) => new { Event = e, Index = index })
                    .Where(item =>
                        item.Event.Options.Any(o => o.Effects.CreativityEffect > 0) &&
                        item.Event.MinLevel <= playerLevel)
                    .ToList();

                if (!availableEvents.Any())
                    return null;
            }

            var selectedEventInfo = availableEvents[random.Next(availableEvents.Count)];
            AddToHistory(selectedEventInfo.Index, recentCreativeEvents);
            return selectedEventInfo.Event;
        }

        private bool IsOptionAvailable(Option option, int creativity, int resilience)
        {
            if (option.Effects.RequiredCreativity > 0 && creativity < option.Effects.RequiredCreativity)
                return false;

            if (option.Effects.RequiredSkills?.Contains("Устойчивость") == true && resilience < 30)
                return false;

            return true;
        }

        private Option ModifyOption(Option opt, int playerLevel, int eventMinLevel, int creativity, int resilience)
        {
            double difficulty = Math.Max(1.0, (playerLevel - eventMinLevel) * 0.15);

            // Создаем новый эффект с учетом характеристик игрока
            var newEffect = new Effect(
                (int)(opt.Effects.KnowledgeEffect * (opt.Effects.KnowledgeEffect < 0 ? difficulty * 1.2 : 1)),
                (int)(opt.Effects.AwarenessEffect * (opt.Effects.AwarenessEffect < 0 ? difficulty * 1.2 : 1)),
                (int)(opt.Effects.MotivationEffect * (opt.Effects.MotivationEffect < 0 ? difficulty * 0.8 : 1))
            )
            {
                // Усиливаем эффекты в зависимости от характеристик
                ResilienceEffect = (int)(opt.Effects.ResilienceEffect * (1 + resilience / 100.0)),
                CreativityEffect = (int)(opt.Effects.CreativityEffect * (1 + creativity / 100.0)),
                ExperienceGain = opt.Effects.ExperienceGain,
                RequiredSkill = opt.Effects.RequiredSkill,
                RequiredSkills = opt.Effects.RequiredSkills,
                RequiredCreativity = opt.Effects.RequiredCreativity,
                ChainId = opt.Effects.ChainId
            };

            // Обрабатываем временные эффекты
            if (opt.Effects.TemporaryEffects != null)
            {
                newEffect.TemporaryEffects = opt.Effects.TemporaryEffects
                    .Select(te => new TemporaryEffect(
                        te.Name,
                        (int)(te.KnowledgeEffect * difficulty * (1 + (creativity + resilience) / 200.0)),
                        (int)(te.AwarenessEffect * difficulty * (1 + (creativity + resilience) / 200.0)),
                        (int)(te.MotivationEffect * difficulty * (1 + (creativity + resilience) / 200.0)),
                        te.Duration
                    )).ToList();
            }

            // Обрабатываем долгосрочные эффекты
            if (opt.Effects.LongTermEffect != null)
            {
                newEffect.LongTermEffect = new TemporaryEffect(
                    opt.Effects.LongTermEffect.Name,
                    (int)(opt.Effects.LongTermEffect.KnowledgeEffect * difficulty * (1 + (creativity + resilience) / 200.0)),
                    (int)(opt.Effects.LongTermEffect.AwarenessEffect * difficulty * (1 + (creativity + resilience) / 200.0)),
                    (int)(opt.Effects.LongTermEffect.MotivationEffect * difficulty * (1 + (creativity + resilience) / 200.0)),
                    opt.Effects.LongTermEffect.Duration
                );
            }

            // Добавляем бонус за креативность
            if (opt.Effects.RequiredCreativity > 0)
            {
                newEffect.CreativityEffect += 2;

                if (newEffect.TemporaryEffects == null)
                    newEffect.TemporaryEffects = new List<TemporaryEffect>();

                newEffect.TemporaryEffects.Add(new TemporaryEffect(
                    "Вдохновение",
                    2 + (int)(creativity / 50.0),
                    2 + (int)(creativity / 50.0),
                    2 + (int)(creativity / 50.0),
                    3
                ));
            }

            // Создаем новый вариант ответа с обновленными эффектами и описанием требований
            string optionText = opt.Text;
            if (opt.Effects.RequiredCreativity > 0 || opt.Effects.RequiredSkills?.Contains("Устойчивость") == true)
            {
                optionText += "\n[Требования:";
                if (opt.Effects.RequiredCreativity > 0)
                    optionText += $"\nКреативность: {opt.Effects.RequiredCreativity}";
                if (opt.Effects.RequiredSkills?.Contains("Устойчивость") == true)
                    optionText += "\nУстойчивость: 30";
                optionText += "]";
            }

            return new Option(optionText, newEffect);
        }
        private void AddCombinedSkillEvents()
        {
            baseEvents.Add(new Event(
                "Креативная устойчивость",
                "Команда в стрессе, но нужно найти нестандартное решение. Как поступите?",
                new List<Option> {
            new Option("Использовать проверенные методы",
                new Effect(5, 5, 0) {
                    CreativityEffect = 5,
                    ResilienceEffect = 5
                }),
            new Option("Организовать мозговой штурм в спокойной обстановке",
                new Effect(15, 20, 10) {
                    CreativityEffect = 20,
                    ResilienceEffect = 15,
                    RequiredCreativity = 30,
                    RequiredSkills = new List<string> { "Устойчивость" }
                }),
            new Option("Придумать революционное решение под давлением",
                new Effect(25, 30, -10) {
                    CreativityEffect = 30,
                    ResilienceEffect = 25,
                    RequiredCreativity = 50,
                    RequiredSkills = new List<string> { "Устойчивость" },
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Креативная устойчивость", 5, 5, 5, 4)
                    }
                })
                },
                "Сочетание креативности и устойчивости открывает новые возможности",
                8,
                true
            ));
        }
        private void AddBasicEvents()
        {
            // Случайная встреча
            baseEvents.Add(new Event(
                "Неожиданная встреча",
                "В кафе вы встречаете бывшего одноклассника, который предлагает работу в крупной компании, но нужно переехать в другой город",
                new List<Option> {
        new Option("Отказаться, но поддерживать связь",
            new Effect(5, 10, -5) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Новые контакты", 2, 2, 0, 3)
                }
            }),
        new Option("Согласиться на переезд",
            new Effect(25, 30, -15) {
                CreativityEffect = 20,
                ResilienceEffect = 25,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Адаптация", -3, 15, -5, 4)
                }
            }),
        new Option("Предложить удалённое сотрудничество",
            new Effect(15, 20, 10) {
                CreativityEffect = 15,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Гибкий график", 5, 5, 10, 3)
                }
            })
                },
                "Иногда случайные встречи могут изменить всю жизнь",
                4
            ));

            // Семейная дилемма
            baseEvents.Add(new Event(
                "Семейный выбор",
                "Родители просят помочь с ремонтом на даче, но у вас важный проект и планы на выходные с друзьями",
                new List<Option> {
        new Option("Отложить всё и помочь родителям",
            new Effect(-10, 25, 15) {
                ResilienceEffect = 20,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Семейная поддержка", 0, 10, 5, 3)
                }
            }),
        new Option("Предложить нанять рабочих за свой счёт",
            new Effect(15, -5, -10) {
                ResilienceEffect = -5
            }),
        new Option("Распределить время между всеми делами",
            new Effect(5, 15, -15) {
                CreativityEffect = 25,
                ResilienceEffect = 15,
                RequiredCreativity = 35,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Многозадачность", 5, -5, -5, 4)
                }
            })
                },
                "Баланс между семьёй и личной жизнью - это искусство",
                3
            ));

            // Хобби или работа
            baseEvents.Add(new Event(
                "Творческий выбор",
                "Ваше хобби (музыка/рисование/фотография) начинает приносить первые деньги. Стоит ли превращать его в работу?",
                new List<Option> {
        new Option("Оставить как хобби",
            new Effect(5, 15, 20) {
                CreativityEffect = 15,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Творческая свобода", 5, 5, 10, 4)
                }
            }),
        new Option("Развивать как бизнес",
            new Effect(25, -10, -15) {
                CreativityEffect = 30,
                RequiredCreativity = 40,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Предпринимательский дух", 10, -5, -5, 5)
                }
            }),
        new Option("Совмещать работу и хобби",
            new Effect(15, 20, -20) {
                CreativityEffect = 35,
                ResilienceEffect = -15,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Усталость", -5, -5, -5, 3)
                }
            })
                },
                "Превращение хобби в работу может изменить отношение к любимому делу",
                5
            ));

            // Социальная ситуация
            baseEvents.Add(new Event(
     "Общественное мнение",
     "Ваш пост в социальной сети неожиданно стал вирусным, но мнения разделились",
     new List<Option> {
        new Option("Удалить пост",
            new Effect(-15, -10, 5) {
                ResilienceEffect = -10,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Сомнения", -5, -5, 0, 2)
                }
            }),
        new Option("Вступить в дискуссию",
            new Effect(20, 25, -15) {
                ResilienceEffect = 25,
                RequiredSkills = new List<string> { "Устойчивость" }, // Оставляем требование навыка
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Публичность", 5, 5, -5, 4)
                }
            }),
        new Option("Написать продолжение с юмором",
            new Effect(15, 15, 20) {
                CreativityEffect = 30,
                RequiredCreativity = 20, // Уменьшаем требование креативности с 35 до 20
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Популярность", 8, 8, 8, 3)
                }
            }),
        // Добавляем новый вариант без требований
        new Option("Игнорировать ситуацию",
            new Effect(5, 10, -5) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Спокойствие", 3, 3, -2, 2)
                }
            })
     },
     "В социальных сетях важно оставаться собой",
     4
 ));

            // Финансовое решение
            baseEvents.Add(new Event(
                "Неожиданный бонус",
                "Вы получили неожиданную премию. Как распорядиться деньгами?",
                new List<Option> {
        new Option("Купить новый гаджет",
            new Effect(15, -5, 20) {
                CreativityEffect = 10,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Технический энтузиазм", 5, 0, 5, 3)
                }
            }),
        new Option("Вложить в обучение",
            new Effect(25, 30, -10) {
                CreativityEffect = 25,
                ResilienceEffect = 15,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Новые знания", 8, 8, -3, 5)
                }
            }),
        new Option("Отложить на будущее",
            new Effect(5, 20, -15) {
                ResilienceEffect = 30,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Финансовая стабильность", 3, 5, -5, 4)
                }
            })
                },
                "Разумное распоряжение деньгами - путь к финансовой независимости",
                3
            ));

            // Здоровье и спорт
            baseEvents.Add(new Event(
                "Спортивный вызов",
                "Друзья зовут участвовать в любительском марафоне через месяц. Вы давно не занимались спортом",
                new List<Option> {
        new Option("Отказаться",
            new Effect(-5, -10, -5) {
                ResilienceEffect = -15,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Сожаление", -3, -3, -3, 2)
                }
            }),
        new Option("Начать интенсивные тренировки",
            new Effect(-10, 25, 20) {
                ResilienceEffect = 35,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Спортивный режим", -5, 10, 5, 5)
                }
            }),
        new Option("Предложить альтернативный вид спорта",
            new Effect(15, 15, 15) {
                CreativityEffect = 20,
                ResilienceEffect = 15,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Активный образ жизни", 3, 5, 5, 4)
                }
            })
                },
                "Спорт - это не только о физической форме, но и о преодолении себя",
                4
            ));
            // Неожиданное событие с парадоксальным выбором
            baseEvents.Add(new Event(
                "Парадокс выбора",
                "Вам предложили две задачи: простую с высокой оплатой или сложную с минимальной оплатой, но с перспективой роста",
                new List<Option> {
        new Option("Взять простую и высокооплачиваемую",
            new Effect(5, -10, 15) {
                CreativityEffect = -15,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Зона комфорта", -3, -3, 5, 4)
                }
            }),
        new Option("Выбрать сложную с минимальной оплатой",
            new Effect(25, 30, -20) {
                CreativityEffect = 35,
                ResilienceEffect = 20,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Профессиональный рост", 5, 5, -5, 5)
                }
            }),
        new Option("Попробовать совместить обе",
            new Effect(-15, 20, -25) {
                CreativityEffect = 40,
                ResilienceEffect = -30,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Перегрузка", -4, 8, -8, 3)
                }
            })
                },
                "Иногда правильный выбор не тот, что кажется очевидным",
                5
            ));

            // Событие с неожиданными последствиями
            baseEvents.Add(new Event(
                "Случайное открытие",
                "Вы случайно удалили важный файл, но в процессе восстановления нашли серьезную уязвимость в системе",
                new List<Option> {
        new Option("Восстановить файл и забыть про уязвимость",
            new Effect(5, -20, 0) {
                CreativityEffect = -10,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Упущенная возможность", -2, -5, -2, 3)
                }
            }),
        new Option("Изучить уязвимость, забыв про файл",
            new Effect(-10, 35, -5) {
                CreativityEffect = 30,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Исследовательский азарт", 5, 8, -3, 4)
                }
            }),
        new Option("Написать программу, которая и файл восстановит, и уязвимость обнаружит",
            new Effect(25, 25, -15) {
                CreativityEffect = 45,
                RequiredCreativity = 40,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Вдохновение", 10, 10, 5, 3)
                }
            })
                },
                "Иногда ошибки приводят к неожиданным открытиям",
                6
            ));

            // Этическая дилемма
            baseEvents.Add(new Event(
                "Дилемма",
                "Вы нашли способ автоматизировать работу целого отдела, но это приведет к увольнению коллег",
                new List<Option> {
        new Option("Внедрить автоматизацию",
            new Effect(30, -25, -20) {
                CreativityEffect = 20,
                ResilienceEffect = -15,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Моральная дилемма", -5, -5, -5, 5)
                }
            }),
        new Option("Поделиться идеей с коллегами и обучить их новым навыкам",
            new Effect(15, 25, 20) {
                CreativityEffect = 30,
                ResilienceEffect = 25,
                RequiredCreativity = 35,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Командный дух", 5, 5, 5, 4)
                }
            }),
        new Option("Скрыть своё открытие",
            new Effect(-20, -10, 10) {
                CreativityEffect = -25,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Внутренний конфликт", -3, -3, -3, 3)
                }
            })
                },
                "Технический прогресс не должен противоречить человечности",
                7
            ));

            // Событие с элементами абсурда
            baseEvents.Add(new Event(
                "Абсурдная ситуация",
                "Искусственный интеллект в вашем проекте начал писать стихи вместо кода",
                new List<Option> {
        new Option("Перепрограммировать ИИ",
            new Effect(10, -15, -5) {
                CreativityEffect = -20,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Разочарование в технологиях", -3, -3, -3, 2)
                }
            }),
        new Option("Создать платформу для поэзии ИИ",
            new Effect(-10, 30, 15) {
                CreativityEffect = 40,
                RequiredCreativity = 45,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Творческий подход", 8, 8, 8, 3)
                }
            }),
        new Option("Объединить код и поэзию в новый вид программирования",
            new Effect(25, 35, -10) {
                CreativityEffect = 50,
                RequiredCreativity = 60,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Инновационное мышление", 10, 10, 5, 4)
                }
            })
                },
                "Иногда самые странные ситуации приводят к революционным решениям",
                8,
                true
            ));

            // Экспериментальное событие
            baseEvents.Add(new Event(
                "Квантовый код",
                "Вы случайно написали код, который работает только по чётным числам месяца",
                new List<Option> {
        new Option("Переписать код правильно",
            new Effect(10, 5, -5) {
                CreativityEffect = -10
            }),
        new Option("Убедить всех, что это фича, а не баг",
            new Effect(-5, 25, 15) {
                CreativityEffect = 30,
                ResilienceEffect = 20,
                RequiredCreativity = 40,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Креативный маркетинг", 5, 5, 5, 3)
                }
            }),
        new Option("Создать новую методологию разработки на основе этого феномена",
            new Effect(30, 35, -15) {
                CreativityEffect = 45,
                RequiredCreativity = 55,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Революционное мышление", 8, 8, -3, 4)
                }
            })
                },
                "Иногда баги становятся особенностями, а особенности создают новые направления",
                7
            ));
            baseEvents.Add(new Event(
       "Стрессовая ситуация",
       "Вы столкнулись с серьезным вызовом. Как справитесь?",
       new List<Option> {
            new Option("Использовать техники релаксации",
                new Effect(5, 5, 5) {
                    ResilienceEffect = 10, // Увеличивает устойчивость
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Спокойствие", 2, 2, 2, 3)
                    }
                }),
            new Option("Продолжить работу, игнорируя стресс",
                new Effect(-5, -10, -15) {
                    ResilienceEffect = -5 // Уменьшает устойчивость
                }),
            new Option("Обратиться за поддержкой к команде",
                new Effect(10, 15, 10) {
                    ResilienceEffect = 15 // Значительно увеличивает устойчивость
                })
       },
       "Умение справляться со стрессом - важный навык",
       4
   ));

            // Добавить событие с влиянием устойчивости на характеристики
            baseEvents.Add(new Event(
                "Сложный проект",
                "Проект оказался сложнее, чем ожидалось. Что предпримете?",
                new List<Option> {
            new Option("Разбить на подзадачи",
                new Effect(15, 10, 5) {
                    ResilienceEffect = 8,
                    RequiredSkill = "Устойчивость", // Требует определенный уровень устойчивости
                    SkillDescription = "Высокая устойчивость поможет справиться с нагрузкой"
                }),
            new Option("Работать сверхурочно",
                new Effect(-10, -15, -20) {
                    ResilienceEffect = -10
                })
                },
                "Правильное планирование помогает справиться с любыми трудностями",
                5
            ));
            // Добавим сбалансированные обычные (basic) вопросы в EventSystem.cs → AddBasicEvents()
            baseEvents.Add(new Event(
                "Внимание",
                "Вы постоянно отвлекаетесь на уведомления. Как справитесь?",
                new List<Option> {
        new Option("Выключить уведомления", new Effect(10, 15, 5)),
        new Option("Игнорировать, надеясь на силу воли", new Effect(-5, -10, -5)),
        new Option("Планировать время на проверку сообщений", new Effect(5, 10, 10))
                },
                "План и осознанное поведение помогают управлять вниманием.",
                2
            ));

            baseEvents.Add(new Event(
                "Лень",
                "Вы постоянно откладываете задачу. Что предпримете?",
                new List<Option> {
        new Option("Сделать первый простой шаг", new Effect(5, 10, 10)),
        new Option("Снова отложить", new Effect(-5, -10, -10)),
        new Option("Заставить себя насильно", new Effect(0, -5, -5))
                },
                "Лучше сделать малое, чем продолжать прокрастинировать.",
                1
            ));

            baseEvents.Add(new Event(
                "Энергия",
                "Вы чувствуете упадок сил днем. Как поступите?",
                new List<Option> {
        new Option("Сделать перерыв на прогулку", new Effect(5, 10, 5)),
        new Option("Пить больше кофе", new Effect(0, -5, -5)),
        new Option("Игнорировать и работать дальше", new Effect(-10, -5, -10))
                },
                "Восстановление энергии — важная часть продуктивности.",
                3
            ));

            baseEvents.Add(new Event(
                "Приоритеты",
                "Слишком много задач. Что выберете?",
                new List<Option> {
        new Option("Составить список приоритетов", new Effect(10, 10, 10)),
        new Option("Делать первое попавшееся", new Effect(0, -5, 0)),
        new Option("Ничего не делать из-за перегруза", new Effect(-10, -10, -15))
                },
                "Четкие приоритеты снижают стресс и повышают эффективность.",
                2
            ));

            baseEvents.Add(new Event(
                "Фокус",
                "Вы замечаете, что многозадачность снижает продуктивность. Что делать?",
                new List<Option> {
        new Option("Перейти к одной задаче", new Effect(10, 15, 5)),
        new Option("Продолжать в том же духе", new Effect(-5, -10, -5)),
        new Option("Сменить задачу на более интересную", new Effect(5, 5, 10))
                },
                "Фокус на одном деле приводит к лучшему результату.",
                3
            ));

            baseEvents.Add(new Event(
    "Здоровье",
    "Вы проспали завтрак. Что будете делать?",
    new List<Option> {
        new Option("Пропустить завтрак",
            new Effect(-15, -5, -10) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Голод", -2, -2, -3, 3)
                }
            }),
        new Option("Перекусить быстро",
            new Effect(5, 5, 0) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Легкий голод", -1, -1, -1, 2)
                }
            }),
        new Option("Приготовить полезный завтрак",
            new Effect(10, 10, 10) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Энергичность", 2, 2, 2, 4)
                }
            })
    },
    "Пропуск завтрака снижает уровень энергии и концентрации. Полезный завтрак улучшает продуктивность.",
    1
));

            baseEvents.Add(new Event(
    "Саморазвитие",
    "Вы читаете сложную книгу, но понимаете лишь 30%. Как поступите?",
    new List<Option> {
        new Option("Сделать вид, что все поняли", new Effect(-10, -15, 0)), // Фальшивая экспертиза
        new Option("Бросить", new Effect(-5, -5, -10)), // Нежелание трудиться
        new Option("Читать медленно и пытаться понять все", new Effect(10, 15, -5)) // Трудно, но эффективно
    },
    "Настоящие знания требуют признания своего невежества. Готовы ли вы выглядеть глупее, чтобы стать умнее?",
    3
));
            baseEvents.Add(new Event(
    "Учёба",
    "Завтра экзамен, а вы почти не готовились. Что будете делать?",
    new List<Option> {
        new Option("Быстро пробежаться по конспектам",
            new Effect(5, 0, -5) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Поверхностные знания", -2, 0, -1, 3)
                }
            }),
        new Option("Усердно готовиться всю ночь",
            new Effect(10, 5, -10) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Сильная усталость", -5, -5, -5, 4)
                }
            }),
        new Option("Забить и надеяться на удачу",
            new Effect(-5, -10, 0) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Тревога", -3, -3, -4, 3)
                }
            })
    },
    "Последняя ночь перед экзаменом — это стресс. Лучше готовиться заранее.",
    2
));
            baseEvents.Add(new Event(
    "Стресс",
    "Сложный проект вызывает постоянное напряжение. Как справиться?",
    new List<Option> {
        new Option("Игнорировать стресс",
            new Effect(-10, -15, -10) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Выгорание", -5, -5, -5, 5)
                }
            }),
        new Option("Взять выходной",
            new Effect(5, 10, 15) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Отдых", 3, 3, 3, 2)
                }
            }),
        new Option("Заняться спортом",
            new Effect(10, 5, 10) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Бодрость", 2, 2, 4, 3)
                }
            })
    },
    "Стресс нужно контролировать, иначе он начнет контролировать вас.",
    4
));
            baseEvents.Add(new Event(
    "Здоровье",
    "Вы чувствуете первые признаки простуды. Как поступите?",
    new List<Option> {
        new Option("Продолжать работать",
            new Effect(-10, -5, -5) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Болезнь", -5, -5, -5, 5)
                }
            }),
        new Option("Взять больничный",
            new Effect(10, 10, -10) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Выздоровление", 2, 2, 2, 3)
                }
            }),
        new Option("Пить витамины и работать",
            new Effect(5, 10, -5) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Лёгкое недомогание", -2, -2, -2, 4)
                }
            })
    },
    "Здоровье важнее работы, но не все готовы это признать.",
    3
));

            baseEvents.Add(new Event(
                "Работа",
                "Начальник дал срочное задание, но вы не знаете, как его сделать. Ваши действия?",
                new List<Option> {
        new Option("Разобраться самому, даже если долго", new Effect(10, 10, 5)), // Лучший
        new Option("Спросить у коллег, но не вникать", new Effect(5, 0, 0)), // Нейтральный
        new Option("Сделать кое-как", new Effect(-5, -5, -5)) // Худший
                },
                "Лучше потратить время на обучение, чем делать некачественно.",
                2
            ));

            baseEvents.Add(new Event(
                "Деньги",
                "Вам неожиданно дали 5000 руб. Куда их потратите?",
                new List<Option> {
        new Option("Потратить на развлечения", new Effect(0, -5, 5)), // Худший
        new Option("Вложить в полезную книгу или курс", new Effect(10, 10, 5)), // Лучший
        new Option("Отложить, но без цели", new Effect(0, 5, 0)) // Нейтральный
                },
                "Деньги могут принести сиюминутную радость или долгосрочную пользу.",
                1
            ));

            baseEvents.Add(new Event(
                "Здоровье",
                "Вы устали, но друзья зовут на прогулку. Пойдёте?",
                new List<Option> {
        new Option("Отказаться и лежать дома", new Effect(0, 5, -5)), // Нейтральный
        new Option("Пойти, но злиться на усталость", new Effect(0, -5, -5)), // Худший
        new Option("Пойти, но уйти пораньше", new Effect(5, 10, 5)) // Лучший
                },
                "Баланс между отдыхом и общением важен для здоровья.",
                1
            ));
            baseEvents.Add(new Event(
    "Карьера",
    "Вам предлагают сложный долгосрочный проект. Возьметесь?",
    new List<Option> {
        new Option("Взяться с энтузиазмом",
            new Effect(10, 15, 10) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Воодушевление", 3, 3, 3, 4)
                },
                LongTermEffect = new TemporaryEffect("Профессиональный рост", 1, 1, 1, 10)
            }),
        new Option("Отказаться",
            new Effect(-5, -10, -5) {
                LongTermEffect = new TemporaryEffect("Упущенные возможности", -1, -1, -1, 8)
            }),
        new Option("Взять и делегировать",
            new Effect(5, 5, 0) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Меньше контроля", -2, 0, -1, 5)
                }
            })
    },
    "Большие возможности часто прячутся за большими вызовами.",
    5
));

            baseEvents.Add(new Event(
                "Отношения",
                "Друг просит в долг, но уже не отдавал прошлый. Дадите?",
                new List<Option> {
        new Option("Объяснить, почему не можете", new Effect(5, 10, 0)), // Лучший
        new Option("Резко отказать", new Effect(0, 0, -5)), // Нейтральный
        new Option("Дать, но потом злиться", new Effect(0, -10, -5)) // Худший
                },
                "Деньги могут испортить дружбу, если не говорить честно.",
                2
            ));

            baseEvents.Add(new Event(
                "Спорт",
                "Вы пропустили неделю тренировок. Как вернуться?",
                new List<Option> {
        new Option("Начать снова с той же нагрузкой", new Effect(0, 0, -5)), // Нейтральный
        new Option("Бросить спорт", new Effect(-5, -10, -10)), // Худший
        new Option("Вернуться с уменьшенной нагрузкой", new Effect(5, 10, 5)) // Лучший
                },
                "Главное — не резкость, а регулярность.",
                1
            ));

            baseEvents.Add(new Event(
                "Время",
                "У вас свободный вечер. Как его провести?",
                new List<Option> {
        new Option("Сделать что-то полезное, но без давления", new Effect(5, 10, 5)), // Лучший
        new Option("Смотреть сериалы", new Effect(0, -5, 0)), // Худший
        new Option("Заставить себя учиться", new Effect(5, 0, -5)) // Нейтральный
                },
                "Отдых — это не грех, но и бездействие не должно быть привычкой.",
                1
            ));

            baseEvents.Add(new Event(
                "Привычки",
                "Вы снова откладываете важное дело. Что делать?",
                new List<Option> {
        new Option("Ждать подходящего настроения", new Effect(0, -5, 0)), // Худший
        new Option("Начать с маленького шага", new Effect(5, 10, 10)), // Лучший
        new Option("Ругать себя", new Effect(-5, -5, -10)) // Очень плохой
                },
                "Лучше сделать немного, чем не сделать ничего.",
                2
            ));

            baseEvents.Add(new Event(
                "Соцсети",
                "Вы тратите слишком много времени на телефон. Как сократить?",
                new List<Option> {
        new Option("Оставить как есть", new Effect(0, -5, 0)), // Худший
        new Option("Поставить лимит времени", new Effect(5, 10, 5)), // Лучший
        new Option("Удалить все приложения", new Effect(0, 5, -5)) // Нейтральный
                },
                "Жёсткие запреты редко работают — лучше контролировать.",
                1
            ));

            baseEvents.Add(new Event(
                "Самооценка",
                "Кто-то сказал, что ваша работа плохая. Как вы отреагируете?",
                new List<Option> {
        new Option("Злиться и спорить", new Effect(0, -5, 0)), // Нейтральный
        new Option("Спросить, что можно улучшить", new Effect(10, 10, 5)), // Лучший
        new Option("Расстроиться и бросить", new Effect(-5, -10, -10)) // Худший
                },
                "Критика — это не всегда нападение, иногда это помощь.",
                2
            ));

            baseEvents.Add(new Event(
                "Сон",
                "Завтра рано вставать, но хочется досмотреть фильм. Ложиться?",
                new List<Option> {
        new Option("Лечь вовремя", new Effect(5, 10, 5)), // Лучший
        new Option("Посмотреть до конца", new Effect(0, -5, -5)), // Худший
        new Option("Лечь, но смотреть в кровати", new Effect(0, 0, 0)) // Нейтральный
                },
                "Недостаток сна снижает продуктивность на весь день.",
                1
            ));

            baseEvents.Add(new Event(
                "Еда",
                "Хочется вредного, но вы на диете. Съедите?",
                new List<Option> {
        new Option("Съесть немного без чувства вины", new Effect(5, 10, 5)), // Лучший
        new Option("Терпеть", new Effect(0, 5, -5)), // Нейтральный
        new Option("Сорваться и съесть много", new Effect(-5, -10, -5)) // Худший
                },
                "Жёсткие запреты часто приводят к срывам.",
                1
            ));

            baseEvents.Add(new Event(
                "Путешествия",
                "Есть возможность поехать в новое место, но страшно. Ехать?",
                new List<Option> {
        new Option("Поехать и наслаждаться", new Effect(10, 15, 10)), // Лучший
        new Option("Отказаться", new Effect(0, -5, -5)), // Худший
        new Option("Поехать, но волноваться", new Effect(5, 5, 0)) // Нейтральный
                },
                "Новые впечатления расширяют кругозор.",
                2
            ));

            baseEvents.Add(new Event(
                "Хобби",
                "Нравится рисовать, но нет времени. Как быть?",
                new List<Option> {
        new Option("Ждать вдохновения", new Effect(0, -5, 0)), // Худший
        new Option("Выделять 10 минут в день", new Effect(5, 10, 10)), // Лучший
        new Option("Забить", new Effect(-5, -10, -5)) // Очень плохо
                },
                "Даже маленькие шаги сохраняют интерес.",
                1
            ));

            baseEvents.Add(new Event(
                "Общение",
                "В компании говорят о чём-то скучном. Поддержите разговор?",
                new List<Option> {
        new Option("Говорить только о себе", new Effect(0, -5, 0)), // Худший
        new Option("Найти интересную тему", new Effect(5, 10, 5)), // Лучший
        new Option("Делать вид, что слушаете", new Effect(0, 0, -5)) // Нейтральный
                },
                "Искренний интерес к другим делает общение ценным.",
                1
            ));
        }
        private void AddBasicTrainingEvents()
        {
            // Событие для начальной прокачки креативности
            baseEvents.Add(new Event(
                "Первые шаги в креативности",
                "Вам поручили придумать название для нового проекта. Как подойдете к задаче?",
                new List<Option> {
            new Option("Взять первое пришедшее в голову название",
                new Effect(0, 0, 0) {
                    CreativityEffect = 5 // Небольшой бонус к креативности
                }),
            new Option("Записать несколько вариантов и выбрать лучший",
                new Effect(5, 5, 0) {
                    CreativityEffect = 10 // Средний бонус к креативности
                }),
            new Option("Посоветоваться с коллегами",
                new Effect(10, 10, 5) {
                    CreativityEffect = 15 // Хороший бонус к креативности
                })
                },
                "Креативность развивается через практику",
                1 // Доступно с первого уровня
            ));

            // Событие для начальной прокачки устойчивости
            baseEvents.Add(new Event(
                "Учимся справляться со стрессом",
                "День не задался с самого утра. Как справитесь с ситуацией?",
                new List<Option> {
            new Option("Продолжить работу как обычно",
                new Effect(0, 0, 0) {
                    ResilienceEffect = 5 // Небольшой бонус к устойчивости
                }),
            new Option("Сделать перерыв и подышать",
                new Effect(5, 5, 5) {
                    ResilienceEffect = 10, // Средний бонус к устойчивости
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Спокойствие", 1, 1, 1, 2)
                    }
                }),
            new Option("Использовать технику управления стрессом",
                new Effect(10, 10, 10) {
                    ResilienceEffect = 15, // Хороший бонус к устойчивости
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Внутренний баланс", 2, 2, 2, 3)
                    }
                })
                },
                "Устойчивость к стрессу можно развивать",
                1 // Доступно с первого уровня
            ));

            // Комбинированное событие для начального развития
            baseEvents.Add(new Event(
                "Новый вызов",
                "Вам нужно освоить новую технологию в сжатые сроки. Как поступите?",
                new List<Option> {
            new Option("Изучать по учебнику",
                new Effect(5, 5, 0) {
                    CreativityEffect = 5,
                    ResilienceEffect = 5
                }),
            new Option("Экспериментировать с кодом",
                new Effect(10, 5, -5) {
                    CreativityEffect = 15,
                    ResilienceEffect = 5,
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Вдохновение", 2, 2, 2, 2)
                    }
                }),
            new Option("Создать пробный проект",
                new Effect(15, 10, -5) {
                    CreativityEffect = 10,
                    ResilienceEffect = 15,
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Уверенность", 2, 2, 2, 3)
                    }
                })
                },
                "Развитие навыков требует времени и усилий",
                2
            ));

            // Событие для продвинутой прокачки (доступно раньше, чем события с требованиями)
            baseEvents.Add(new Event(
                "Сложная задача",
                "Необходимо оптимизировать важный процесс. Как подойдете к решению?",
                new List<Option> {
            new Option("Использовать проверенные методы",
                new Effect(5, 5, 5) {
                    CreativityEffect = 10,
                    ResilienceEffect = 10
                }),
            new Option("Придумать новый подход",
                new Effect(15, 10, -5) {
                    CreativityEffect = 25,
                    ResilienceEffect = 15,
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Креативный подход", 3, 3, 3, 3)
                    }
                }),
            new Option("Комбинировать несколько решений",
                new Effect(20, 15, -10) {
                    CreativityEffect = 20,
                    ResilienceEffect = 20,
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Системное мышление", 4, 4, 4, 4)
                    }
                })
                },
                "Сложные задачи - это возможность для роста",
                4
            ));
        }
        private void AddTimedEvents()
        {
            // Добавить в метод AddTimedEvents()

            // Опаздываем на встречу
            timedEvents.Add(new TimedEvent(
                "Пробки на дорогах",
                "⚡ До важной встречи 15 минут, а вы застряли в пробке! 8 секунд на решение!",
                new List<Option> {
        new Option("Заказать такси и поехать по выделенной полосе",
            new Effect(-5, 10, 15) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Пунктуальность", 0, 5, 5, 2)
                }
            }),
        new Option("Позвонить и предупредить об опоздании",
            new Effect(10, 15, -10) {
                ResilienceEffect = 10,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Честность", 5, 5, -5, 3)
                }
            }),
        new Option("Найти обходной путь через дворы",
            new Effect(15, -10, 20) {
                CreativityEffect = 15
            })
                },
                "Планирование времени с учётом пробок - важный навык",
                8,
                new Effect(-15, -10, -20), // Штраф за опоздание
                3
            ));

            // Срочный заказ еды
            timedEvents.Add(new TimedEvent(
                "Обеденный перерыв",
                "🍕 Через 5 минут важное совещание, а вы не успели поесть! 7 секунд на выбор!",
                new List<Option> {
        new Option("Быстро перекусить сэндвичем",
            new Effect(5, 5, 10) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Лёгкий голод", -2, -2, -2, 2)
                }
            }),
        new Option("Заказать доставку к концу совещания",
            new Effect(15, 10, -5) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Предвкушение обеда", 0, 0, 5, 3)
                }
            }),
        new Option("Перенести обед на потом",
            new Effect(-10, 15, -15))
                },
                "Правильное питание важно для продуктивной работы",
                7,
                new Effect(-5, -10, -15),
                2
            ));

            // Внезапный звонок
            timedEvents.Add(new TimedEvent(
                "Важный разговор",
                "📱 Звонит важный клиент, а вы в шумном кафе! 6 секунд на решение!",
                new List<Option> {
        new Option("Выйти на улицу для разговора",
            new Effect(20, 15, -5) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Профессионализм", 5, 5, 0, 2)
                }
            }),
        new Option("Попросить перезвонить позже",
            new Effect(-5, 10, -10)),
        new Option("Попытаться говорить в кафе",
            new Effect(-15, -10, 5))
                },
                "Не всегда условия идеальны для важных разговоров",
                6,
                new Effect(-20, -15, -10),
                4
            ));

            // Спонтанная вечеринка
            timedEvents.Add(new TimedEvent(
                "Внезапное приглашение",
                "🎉 Друзья зовут на спонтанную вечеринку, но завтра важный день! 8 секунд на решение!",
                new List<Option> {
        new Option("Пойти, но ненадолго",
            new Effect(-10, 5, 15) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Усталость", -3, -3, 5, 2)
                }
            }),
        new Option("Отказаться и выспаться",
            new Effect(15, 10, -10) {
                ResilienceEffect = 15,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Бодрость", 5, 5, -5, 2)
                }
            }),
        new Option("Пойти и остаться допоздна",
            new Effect(-20, -15, 25) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Недосып", -5, -5, 10, 3)
                }
            })
                },
                "Баланс между работой и отдыхом важен для успеха",
                8,
                new Effect(-10, -15, -5),
                3
            ));

            // Срочные покупки
            timedEvents.Add(new TimedEvent(
                "Закрытие магазина",
                "🏪 До закрытия магазина 10 минут, а вам нужно купить подарок! 9 секунд на выбор!",
                new List<Option> {
        new Option("Быстро купить что-нибудь",
            new Effect(-5, -10, 5) {
                CreativityEffect = -10
            }),
        new Option("Заказать онлайн с доставкой",
            new Effect(10, 15, -5) {
                CreativityEffect = 10,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Ожидание доставки", 0, 5, -3, 2)
                }
            }),
        new Option("Придумать креативный самодельный подарок",
            new Effect(20, 25, -10) {
                CreativityEffect = 25,
                RequiredCreativity = 30,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Вдохновение", 5, 5, 5, 3)
                }
            })
                },
                "Иногда лучшие решения приходят в последний момент",
                9,
                new Effect(-15, -20, -10),
                4
            ));
            timedEvents.Add(new TimedEvent(
                "Срочный баг",
                "Найдена критическая ошибка! 7 секунд на решение!",
                new List<Option> {
            new Option("Быстрый хотфикс",
                new Effect(10, -5, -5) {
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Технический долг", -2, -2, -2, 3)
                    }
                }),
            new Option("Тщательное исправление",
                new Effect(15, 10, -10))
                },
                "В критической ситуации важно сохранять спокойствие",
                7,
                new Effect(-15, -10, -10),
                3
            ));

            timedEvents.Add(new TimedEvent(
                "Дедлайн горит",
                "Клиент ждёт демонстрацию! 5 секунд!",
                new List<Option> {
            new Option("Показать текущую версию",
                new Effect(0, -5, -5)),
            new Option("Быстро доделать фичи",
                new Effect(15, 10, -10) {
                    RequiredSkill = "Критическое мышление",
                    SkillDescription = "Нужно критическое мышление для быстрого анализа ситуации"
                })
                },
                "Иногда лучше показать меньше, но качественнее",
                5,
                new Effect(-10, -15, -15),
                4
            ));

            timedEvents.Add(new TimedEvent(
                "Конфликт в команде",
                "Срочно требуется решение! 6 секунд!",
                new List<Option> {
            new Option("Разделить задачи",
                new Effect(5, 10, 5)),
            new Option("Провести медиацию",
                new Effect(15, 20, 10) {
                    RequiredSkill = "Осознанность",
                    SkillDescription = "Требуется осознанность для эффективного решения конфликта"
                })
                },
                "Конфликты нужно решать быстро",
                6,
                new Effect(-10, -15, -20),
                5
            ));

            timedEvents.Add(new TimedEvent(
                "Сервер не отвечает",
                "Критическая нагрузка! 8 секунд до отказа!",
                new List<Option> {
            new Option("Перезагрузить сервер",
                new Effect(-10, 5, -5)),
            new Option("Оптимизировать нагрузку",
                new Effect(20, 15, -10))
                },
                "Действуйте быстро, но обдуманно",
                8,
                new Effect(-20, -20, -15),
                6
            ));

            timedEvents.Add(new TimedEvent(
                "Утечка данных",
                "Обнаружена утечка данных! 7 секунд на реакцию!",
                new List<Option> {
            new Option("Отключить доступ всем",
                new Effect(-20, 5, -20)),
            new Option("Локализовать утечку",
                new Effect(20, 20, -5) {
                    RequiredSkill = "Стрессоустойчивость",
                    SkillDescription = "Необходима стрессоустойчивость для хладнокровной работы"
                })
                },
                "Безопасность данных - высший приоритет",
                7,
                new Effect(-25, -20, -15),
                7
            ));
        }
        private void AddIntermediateEvents()
        {
            baseEvents.Add(new Event(
                "Кризисная ситуация",
                "Ваш проект находится под угрозой срыва сроков!",
                new List<Option> {
                    new Option("Работать ночами",
                        new Effect(-20, -15, -10) {
                            TemporaryEffects = new List<TemporaryEffect> {
                                new TemporaryEffect("Усталость", -5, -5, -5, 3)
                            }
                        }),
                    new Option("Пересмотреть приоритеты", new Effect(10, 15, -5)),
                    new Option("Просить помощи", new Effect(-5, 10, 5))
                },
                "Кризисы требуют взвешенных решений",
                6
            ));
            
        {
            // 1. Кризисный менеджмент
            baseEvents.Add(new Event(
                "Кризис в проекте",
                "Сроки горят, а команда демотивирована. Что делать?",
                new List<Option> {
            new Option("Приказать работать сверхурочно",
                new Effect(-25, -20, -30)),

            new Option("Провести мотивационную встречу",
                new Effect(5, 15, 20)),

            new Option("Пересмотреть приоритеты",
                new Effect(10, 25, -10) {
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Фокус", 5, 10, 0, 4)
                    }
                })
                },
                "Кризисы требуют лидерских качеств",
                minLevel: 6
            ));

            // 2. Финансовый выбор
            baseEvents.Add(new Event(
                "Инвестиции",
                "Вам предлагают рискованные, но перспективные инвестиции",
                new List<Option> {
            new Option("Отказаться",
                new Effect(0, 10, -5)),

            new Option("Вложить 30% средств",
                new Effect(15, 20, 10)),

            new Option("Вложить всё",
                new Effect(-40, -20, -30) {
                    ChainId = "Финансовый_крах"
                })
                },
                "Риск должен быть осознанным",
                minLevel: 7
            ));

            // 3. Конфликт в команде
            baseEvents.Add(new Event(
                "Конфликт разработчиков",
                "Два ключевых разработчика не могут договориться",
                new List<Option> {
            new Option("Наказать обоих",
                new Effect(-10, -15, -25)),

            new Option("Провести медиацию",
                new Effect(5, 20, 15) {
                    RequiredSkill = "Коммуникация"
                }),

            new Option("Дать им разобраться самим",
                new Effect(-5, -10, 5) {
                    LongTermEffect = new TemporaryEffect("Напряжение", -2, -3, -1, 8)
                })
                },
                "Конфликты снижают продуктивность команды",
                minLevel: 8
            ));
        }
        }

        private void AddAdvancedEvents()
        {// Добавьте в метод AddAdvancedEvents() в EventSystem.cs
baseEvents.Add(new Event(
    "Стресс-менеджмент",
    "Вы столкнулись с серьезным конфликтом на работе. Как справитесь?",
    new List<Option> {
        new Option("Глубоко подышать и спокойно обсудить",
            new Effect(5, 10, 5) {
                ResilienceEffect = 15,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Спокойствие", 2, 2, 2, 3)
                }
            }),
        new Option("Немедленно эмоционально отреагировать",
            new Effect(-10, -15, -10) {
                ResilienceEffect = -5
            }),
        new Option("Взять паузу и обдумать ситуацию",
            new Effect(10, 15, 0) {
                ResilienceEffect = 10
            })
    },
    "Умение сохранять спокойствие в стрессовых ситуациях увеличивает вашу устойчивость",
    6
));

baseEvents.Add(new Event(
    "Самоконтроль",
    "Проект близок к провалу, все нервничают. Ваши действия?",
    new List<Option> {
        new Option("Сохранять хладнокровие и методично работать",
            new Effect(15, 10, 10) {
                ResilienceEffect = 20,
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Концентрация", 3, 3, 3, 4)
                }
            }),
        new Option("Паниковать вместе со всеми",
            new Effect(-15, -10, -15) {
                ResilienceEffect = -10
            }),
        new Option("Отстраниться от ситуации",
            new Effect(0, -5, -5) {
                ResilienceEffect = 5
            })
    },
    "Ваша устойчивость растет, когда вы справляетесь со стрессом",
    7
));
            baseEvents.Add(new Event(
                "Технический долг",
                "Накопились серьезные проблемы в архитектуре проекта. Как поступите?",
                new List<Option> {
                    new Option("Игнорировать",
                        new Effect(-30, -20, -25) {
                            LongTermEffect = new TemporaryEffect("Растущий долг", -2, -1, -1, 10)
                        }),
                    new Option("Частично исправить",
                        new Effect(10, 5, -15) {
                            RequiredSkill = "Рефакторинг"
                        }),
                    new Option("Полный рефакторинг",
                        new Effect(-10, 20, 5) {
                            ChainId = "Рефакторинг_Последствия"
                        })
                },
                "Технический долг подобен финансовому - чем дольше игнорируете, тем дороже исправление",
                11,
                true
            ));
            
        {
            // 1. Кризис лидерства (босс-событие)
            baseEvents.Add(new Event(
                "Бунт в команде",
                "Команда отказывается выполнять ваши решения из-за спорного проекта",
                new List<Option> {
            // Жесткие меры (негативные последствия)
            new Option("Уволить зачинщиков",
                new Effect(-30, -40, -50) {
                    LongTermEffect = new TemporaryEffect("Страх в команде", -10, -15, -20, 15)
                }),
            
          
            new Option("Провести переговоры",
                new Effect(15, 25, 10) {
                    RequiredSkill = "Эмоциональный интеллект",
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Доверие", 5, 10, 5, 10)
                    }
                }),
            
            new Option("Пересмотреть стратегию",
                new Effect(-15, 30, -20) {
                    ChainId = "Реформа_команды",
                    RequiredCreativity = 35
                })
                },
                "Лидерство проверяется в кризисах",
                minLevel: 11,
                isBossEvent: true
            ));

            
            baseEvents.Add(new Event(
                "Внедрение AI",
                "Нужно рискнуть и перевести систему на искусственный интеллект?",
                new List<Option> {
            new Option("Постепенная интеграция",
                new Effect(20, 15, 10)),

            new Option("Полный переход (риск!)",
                new Effect(-40, 50, -30) {
                    ChainId = "AI_Революция",
                    RequiredSkills = new List<string> { "Машинное обучение", "Управление рисками" }
                }),

            new Option("Отказаться от инноваций",
                new Effect(-20, -25, -15) {
                    LongTermEffect = new TemporaryEffect("Техническое отставание", -7, -10, -5, 20)
                })
                },
                "Инновации определяют будущее компании",
                minLevel: 12
            ));

            
            baseEvents.Add(new Event(
                "Данные клиентов",
                "Можно продать данные для быстрой прибыли, но это неэтично",
                new List<Option> {
            new Option("Продать анонимизированные данные",
                new Effect(40, -50, -60) {
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Вина", 0, -15, -20, 25)
                    }
                }),

            new Option("Отказаться от сделки",
                new Effect(-20, 40, 30)),

            new Option("Найти компромисс",
                new Effect(10, 25, 15) {
                    RequiredSkills = new List<string> { "Этика", "Креативность" },
                    ChainId = "Этичный_бизнес"
                })
                },
                "Репутация строится годами, а разрушается за день",
                minLevel: 13,
                isBossEvent: true
            ));
        }
        }
        private void AddResilienceBossEvents()
        {
            baseEvents.Add(new Event(
                "Критический кризис",
                "Проект на грани провала, команда выгорает. Как справитесь с ситуацией?",
                new List<Option> {
            new Option("Взять ответственность на себя",
                new Effect(30, 25, -15) {
                    ResilienceEffect = 40,
                    RequiredSkills = new List<string> { "Устойчивость" },
                    RequiredCreativity = 40,
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Лидерская стойкость", 8, 8, 8, 5)
                    }
                }),
            new Option("Разделить нагрузку по команде",
                new Effect(20, 15, 10) {
                    ResilienceEffect = 20,
                    RequiredSkills = new List<string> { "Устойчивость" }
                }),
            new Option("Запросить перенос сроков",
                new Effect(-10, -20, -5) {
                    ResilienceEffect = -15
                })
                },
                "Настоящий лидер проявляется в кризисных ситуациях",
                12,
                true
            ));

            baseEvents.Add(new Event(
                "Экстремальный вызов",
                "Одновременно произошло несколько критических сбоев. Ваши действия?",
                new List<Option> {
            new Option("Сохранять хладнокровие и методично решать проблемы",
                new Effect(35, 30, -10) {
                    ResilienceEffect = 50,
                    RequiredSkills = new List<string> { "Устойчивость" },
                    CreativityEffect = 20,
                    RequiredCreativity = 50,
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Антикризисный режим", 10, 10, 10, 6)
                    }
                }),
            new Option("Привлечь внешних специалистов",
                new Effect(15, 10, -5) {
                    ResilienceEffect = 10
                }),
            new Option("Действовать по стандартным процедурам",
                new Effect(-5, -10, 0) {
                    ResilienceEffect = -10
                })
                },
                "Экстремальные ситуации требуют исключительной устойчивости",
                14,
                true
            ));
        }
        private void AddCreativityEvents()
        {
            baseEvents.Add(new Event(
        "Креативное мышление",
        "На совещании нужно предложить новый подход к проекту. Как поступите?",
        new List<Option> {
            new Option("Предложить стандартное решение",
                new Effect(5, 5, 0) {
                    CreativityEffect = 5
                }),
            new Option("Комбинировать существующие идеи",
                new Effect(10, 10, -5) {
                    CreativityEffect = 15
                }),
            new Option("Придумать что-то совершенно новое",
                new Effect(15, 15, -10) {
                    CreativityEffect = 25,
                    RequiredCreativity = 20
                })
        },
        "Креативность развивается через практику новых идей",
        4
    ));
            baseEvents.Add(new Event(
        "Творческий вызов",
        "Нужно оптимизировать процесс разработки. Ваш подход?",
        new List<Option> {
            new Option("Использовать готовые методологии",
                new Effect(10, 5, 0) {
                    CreativityEffect = 10,
                    ResilienceEffect = 5
                }),
            new Option("Создать гибридный подход",
                new Effect(20, 15, -10) {
                    CreativityEffect = 30,
                    ResilienceEffect = 15,
                    RequiredCreativity = 30
                }),
            new Option("Разработать свою методологию",
                new Effect(30, 25, -15) {
                    CreativityEffect = 40,
                    ResilienceEffect = 20,
                    RequiredCreativity = 40,
                    RequiredSkills = new List<string> { "Устойчивость" }
                })
        },
        "Инновации требуют баланса между креативностью и стабильностью",
        7,
        true
    ));
            baseEvents.Add(new Event(
        "Инновационный прорыв",
        "Появилась возможность полностью изменить архитектуру проекта",
        new List<Option> {
            new Option("Постепенные улучшения",
                new Effect(15, 10, 5) {
                    CreativityEffect = 20,
                    ResilienceEffect = 10
                }),
            new Option("Экспериментальный подход",
                new Effect(20, 20, -10) {
                    CreativityEffect = 35,
                    ResilienceEffect = 25,
                    RequiredCreativity = 50,
                    RequiredSkills = new List<string> { "Устойчивость" }
                }),
            new Option("Революционное решение",
                new Effect(25, 20, -20) {
                    CreativityEffect = 50,
                    ResilienceEffect = 40,
                    RequiredCreativity = 70,
                    RequiredSkills = new List<string> { "Устойчивость" },
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Прорывное мышление", 10, 10, 10, 5)
                    }
                })
        },
        "Большие изменения требуют как креативности, так и устойчивости",
        10,
        true
    ));
            baseEvents.Add(new Event(
                "Творческий подход",
                "Вы столкнулись с нестандартной проблемой в проекте. Как поступите?",
                new List<Option> {
            new Option("Использовать проверенное решение",
                new Effect(5, 0, 0) {
                    CreativityEffect = 0
                }),
            new Option("Придумать новый подход",
                new Effect(10, 5, -5) {
                    CreativityEffect = 15,
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Вдохновение", 3, 3, 3, 3)
                    }
                }),
            new Option("Комбинировать разные методы",
                new Effect(15, 10, -10) {
                    CreativityEffect = 20,
                    RequiredSkill = "Креативность",
                    SkillDescription = "Требуется базовый уровень креативности"
                })
                },
                "Креативность растет, когда вы пробуете новые подходы",
                3
            ));

            
            baseEvents.Add(new Event(
                "Инновационное решение",
                "Команда застряла на сложной задаче. Ваши действия?",
                new List<Option> {
            new Option("Провести мозговой штурм",
                new Effect(20, 15, 10) {
                    CreativityEffect = 25,
                    RequiredCreativity = 30,
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Креативный поток", 5, 5, 5, 4)
                    }
                }),
            new Option("Исследовать существующие решения",
                new Effect(10, 5, 0) {
                    CreativityEffect = 10
                }),
            new Option("Предложить революционный подход",
                new Effect(30, 20, -15) {
                    CreativityEffect = 40,
                    RequiredCreativity = 50,
                    ChainId = "Инновационный_прорыв"
                })
                },
                "Иногда лучшее решение - это совершенно новый подход",
                8
            ));

            // Босс-событие на креативность
            baseEvents.Add(new Event(
                "Креативный прорыв",
                "Вы видите возможность полностью переосмыслить проект. Рискнете?",
                new List<Option> {
            new Option("Полностью переработать архитектуру",
                new Effect(40, 30, -20) {
                    CreativityEffect = 50,
                    RequiredCreativity = 70,
                    ResilienceEffect = 20,
                    TemporaryEffects = new List<TemporaryEffect> {
                        new TemporaryEffect("Творческий гений", 10, 10, 10, 5)
                    }
                }),
            new Option("Внести частичные улучшения",
                new Effect(20, 15, -10) {
                    CreativityEffect = 25,
                    RequiredCreativity = 40
                }),
            new Option("Остаться с текущим решением",
                new Effect(-10, -15, 0) {
                    CreativityEffect = -10
                })
                },
                "Великие прорывы требуют смелости и креативности",
                10,
                true // это босс-событие
            ));
        }
        private void InitializeEventChains()
        {
            eventChains.Add(new EventChain(
                "Рефакторинг_Последствия",
                new List<Event> {
                    new Event(
                        "Последствия рефакторинга",
                        "Ваш рефакторинг вызвал новые ошибки в системе",
                        new List<Option> {
                            new Option("Откатить изменения", new Effect(-5, -10, -15)),
                            new Option("Исправлять дальше", new Effect(15, 10, 5))
                        },
                        "Рефакторинг требует тщательного тестирования",
                        11
                    )
                },
                11
            ));
        }
        public EventChain GetEventChain(string chainId)
        {
            return eventChains.FirstOrDefault(c => c.Id == chainId);
        }
    }
}