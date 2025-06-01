using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationalEventGenerator
{
    public class EventSystem
    {
        private List<Event> baseEvents = new List<Event>();
        private List<Event> timedEvents = new List<Event>(); // Новый список для таймер-событий
        private List<EventChain> eventChains = new List<EventChain>();
        private Random random = new Random();
        private List<int> usedEventIndices = new List<int>();
        private int lastEventIndex = -1;

        public Event GetRandomTimedEvent(int playerLevel)
        {
            var availableTimedEvents = timedEvents.Where(e => e.MinLevel <= playerLevel).ToList();
            if (!availableTimedEvents.Any())
                return GenerateEvent(playerLevel); // Если нет доступных таймер-событий, возвращаем обычное

            return availableTimedEvents[random.Next(availableTimedEvents.Count)];
        }
        public void InitializeEvents()
        {
            baseEvents.Clear();
            timedEvents.Clear(); // Очищаем таймер-события
            eventChains.Clear();
            AddBasicEvents();
            AddTimedEvents();
            AddIntermediateEvents();
            AddAdvancedEvents();
            InitializeEventChains();
        }

        public Event GenerateEvent(int playerLevel)
        {
            // Очищаем историю, если использовано 70% событий
            if (usedEventIndices.Count >= baseEvents.Count * 0.7)
                usedEventIndices.Clear();

            // Выбираем доступные события
            var availableEvents = baseEvents
                .Where((e, index) => e.MinLevel <= playerLevel &&
                                    !usedEventIndices.Contains(index) &&
                                    index != lastEventIndex)
                .ToList();
            if (random.NextDouble() < 0.35) // 35% шанс
            {
                var availableTimedEvents = timedEvents
                    .Where(e => e.MinLevel <= playerLevel)
                    .ToList();

                if (availableTimedEvents.Any())
                    return availableTimedEvents[random.Next(availableTimedEvents.Count)];
            }
            // Если все использованы — сбрасываем
            if (!availableEvents.Any())
                availableEvents = baseEvents.Where(e => e.MinLevel <= playerLevel).ToList();

            // Выбираем случайное
            var randomIndex = random.Next(availableEvents.Count);
            var selectedEvent = availableEvents[randomIndex];
            lastEventIndex = baseEvents.IndexOf(selectedEvent);
            usedEventIndices.Add(lastEventIndex);

            // Создаем копию события с модифицированными эффектами
            var modifiedOptions = selectedEvent.Options.Select(opt =>
            {
                // Увеличиваем сложность на основе уровня игрока
                var difficulty = Math.Max(1.0, (playerLevel - selectedEvent.MinLevel) * 0.15); // 15% за каждый уровень выше минимального

                // Создаем новый эффект с измененными значениями
                var newEffect = new Effect(
                    (int)(opt.Effects.KnowledgeEffect * (opt.Effects.KnowledgeEffect < 0 ? difficulty : 1)),
                    (int)(opt.Effects.AwarenessEffect * (opt.Effects.AwarenessEffect < 0 ? difficulty : 1)),
                    (int)(opt.Effects.MotivationEffect * (opt.Effects.MotivationEffect < 0 ? difficulty : 1))
                )
                {
                    ResilienceEffect = opt.Effects.ResilienceEffect,
                    CreativityEffect = opt.Effects.CreativityEffect,
                    ExperienceGain = opt.Effects.ExperienceGain,
                    RequiredSkill = opt.Effects.RequiredSkill,
                    RequiredSkills = opt.Effects.RequiredSkills,
                    RequiredCreativity = opt.Effects.RequiredCreativity,
                    ChainId = opt.Effects.ChainId
                };

                // Добавляем временные эффекты, если они есть
                if (opt.Effects.TemporaryEffects != null)
                {
                    newEffect.TemporaryEffects = opt.Effects.TemporaryEffects.Select(te =>
                        new TemporaryEffect(
                            te.Name,
                            (int)(te.KnowledgeEffect * difficulty),
                            (int)(te.AwarenessEffect * difficulty),
                            (int)(te.MotivationEffect * difficulty),
                            te.Duration
                        )).ToList();
                }

                // Добавляем долгосрочный эффект, если он есть
                if (opt.Effects.LongTermEffect != null)
                {
                    newEffect.LongTermEffect = new TemporaryEffect(
                        opt.Effects.LongTermEffect.Name,
                        (int)(opt.Effects.LongTermEffect.KnowledgeEffect * difficulty),
                        (int)(opt.Effects.LongTermEffect.AwarenessEffect * difficulty),
                        (int)(opt.Effects.LongTermEffect.MotivationEffect * difficulty),
                        opt.Effects.LongTermEffect.Duration
                    );
                }

                return new Option(opt.Text, newEffect);
            }).ToList();

            // Создаем новое событие с модифицированными опциями
            return new Event(
                selectedEvent.Category,
                selectedEvent.Description,
                modifiedOptions,
                selectedEvent.Explanation,
                selectedEvent.MinLevel,
                selectedEvent.IsBossEvent
            );
        }

        private void AddBasicEvents()
        {
            baseEvents.Add(new Event(
    "Здоровье",
    "Вы проспали завтрак. Что будете делать?",
    new List<Option> {
        new Option("Пропустить завтрак",
            new Effect(-15, 0, -5) {
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
            new Effect(5, 5, -5) {
                TemporaryEffects = new List<TemporaryEffect> {
                    new TemporaryEffect("Выздоровление", 2, 2, 2, 3)
                }
            }),
        new Option("Пить витамины и работать",
            new Effect(0, 0, -5) {
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
        new Option("Сделать кое-как", new Effect(-5, -5, 0)) // Худший
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

            // Остальные 5 вопросов (с рандомным порядком лучших вариантов)

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

        private void AddTimedEvents()
        {
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
                "Обнаружена утечка! 7 секунд на реакцию!",
                new List<Option> {
            new Option("Отключить доступ всем",
                new Effect(-15, 10, -10)),
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
        {
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

        

        private Event GetRandomBossEvent(int playerLevel)
        {
            var bossEvents = baseEvents.Where(e => e.IsBossEvent && e.MinLevel <= playerLevel).ToList();
            return bossEvents.Count > 0 ? bossEvents[random.Next(bossEvents.Count)] : GenerateEvent(playerLevel);
        }

        public EventChain GetEventChain(string chainId)
        {
            return eventChains.FirstOrDefault(c => c.Id == chainId);
        }
    }
}