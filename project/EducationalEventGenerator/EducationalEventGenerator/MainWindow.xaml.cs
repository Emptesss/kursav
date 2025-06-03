using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace EducationalEventGenerator
{
    public partial class MainWindow : Window
    {
        private PlayerStats playerStats;
        private Stopwatch stopwatch;
        private EventSystem eventSystem = new EventSystem();
        private SkillSystem skillSystem;
        private Random random = new Random();
        private System.Windows.Threading.DispatcherTimer timer;
        private int remainingTime;

        private Event currentEvent;
        private List<TemporaryEffect> activeEffects = new List<TemporaryEffect>();
        private List<Event> eventHistory = new List<Event>();
        private EventChain currentChain = null;
        private int chainProgress = 0;

        private int oldKnowledge;
        private int oldAwareness;
        private int oldMotivation;
        private int oldResilience;
        private int oldCreativity;
        
      

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            InitializeSystems();

            // Установка начальных значений
            oldKnowledge = playerStats.Knowledge;
            oldAwareness = playerStats.Awareness;
            oldMotivation = playerStats.Motivation;
            oldResilience = playerStats.Resilience;
            oldCreativity = playerStats.Creativity;

            // Делаем панель навыков и продвинутых характеристик скрытыми при старте
            SkillsPanel.Visibility = Visibility.Collapsed;
            AdvancedStatsPanel.Visibility = Visibility.Collapsed;

            UpdateUI();
        }

        private void InitializeSystems()
        {
            // Правильный порядок инициализации
            skillSystem = new SkillSystem();
            skillSystem.InitializeSkills();

            playerStats = new PlayerStats(skillSystem);

            eventSystem = new EventSystem();
            eventSystem.InitializeEvents();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = false;
            NextButton.IsEnabled = true;
            RestartButton.IsEnabled = true;
            ShowRandomEvent();
        }
        private void InitializeTimer()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100); // Уменьшаем интервал для более плавной анимации
            timer.Tick += Timer_Tick;
            stopwatch = new Stopwatch();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (currentEvent is TimedEvent timedEvent)
            {
                double elapsedSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
                remainingTime = Math.Max(0, timedEvent.TimeLimit - (int)Math.Ceiling(elapsedSeconds));

                // Обновляем прогресс бар плавно
                double progress = Math.Max(0, timedEvent.TimeLimit - elapsedSeconds);
                TimeProgressBar.Value = progress;
                TimeProgressBar.Maximum = timedEvent.TimeLimit;
                TimerText.Text = $"Осталось: {remainingTime} сек";

                // Изменение цвета в зависимости от оставшегося времени
                if (remainingTime <= 3)
                {
                    TimeProgressBar.Foreground = new SolidColorBrush(Colors.Red);
                    TimerText.Foreground = new SolidColorBrush(Colors.Red);
                }
                else if (remainingTime <= 5)
                {
                    TimeProgressBar.Foreground = new SolidColorBrush(Colors.Orange);
                    TimerText.Foreground = new SolidColorBrush(Colors.Orange);
                }

                // Проверка окончания времени
                if (remainingTime <= 0)
                {
                    timer.Stop();
                    stopwatch.Stop();
                    TimerPanel.Visibility = Visibility.Collapsed;

                    // Применяем штрафные эффекты
                    playerStats.ApplyEffects(timedEvent.TimeoutEffect);
                    MessageBox.Show("Время истекло! Применены штрафные эффекты.",
                                  "Время вышло!", MessageBoxButton.OK, MessageBoxImage.Warning);

                    // Деактивируем все кнопки
                    foreach (var child in OptionsPanel.Children)
                    {
                        if (child is Button btn)
                            btn.IsEnabled = false;
                    }

                    ShowRandomEvent();
                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExplanationBox.Visibility == Visibility.Visible)
            {
                ShowRandomEvent();
            }
            else
            {
                MessageBox.Show("Сначала сделайте выбор и прочитайте объяснение!", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновляем создание PlayerStats
            skillSystem = new SkillSystem();
            skillSystem.InitializeSkills();
            playerStats = new PlayerStats(skillSystem);
            timer.Stop();
            TimerPanel.Visibility = Visibility.Collapsed;

            activeEffects.Clear();
            eventHistory.Clear();
            currentChain = null;
            chainProgress = 0;

            InitializeSystems();
            UpdateUI();

            EventText.Text = "Нажмите 'Начать игру' для старта";
            EventCategory.Text = "";
            OptionsPanel.Children.Clear();
            ExplanationBox.Visibility = Visibility.Collapsed;
            ActiveEffectsDisplay.ItemsSource = null;

            StartButton.IsEnabled = true;
            NextButton.IsEnabled = false;
            RestartButton.IsEnabled = false;
        }

        private void ShowRandomEvent()
        {
            if (CheckGameOver()) return;

            if (currentChain != null && chainProgress < currentChain.Events.Count)
            {
                currentEvent = currentChain.Events[chainProgress];
                chainProgress++;
            }
            else
            {
                currentEvent = eventSystem.GenerateEvent(playerStats.Level);
                currentChain = null;
                chainProgress = 0;
            }

            eventHistory.Add(currentEvent);
            DisplayEvent(currentEvent);
        }

        private void DisplayEvent(Event eventToShow)
        {
            timer.Stop();
            stopwatch.Stop();
            TimerPanel.Visibility = Visibility.Collapsed;
            EventCategory.Text = eventToShow.Category.ToUpper();
            EventText.Text = eventToShow.Description;
            OptionsPanel.Children.Clear();
            ExplanationBox.Visibility = Visibility.Collapsed;
            NextButton.IsEnabled = false;

            // Добавляем обработку TimedEvent
            if (eventToShow is TimedEvent timedEvent)
            {
                remainingTime = timedEvent.TimeLimit;
                TimerPanel.Visibility = Visibility.Visible;

                // Устанавливаем начальные значения
                TimeProgressBar.Maximum = timedEvent.TimeLimit;
                TimeProgressBar.Value = timedEvent.TimeLimit;
                TimeProgressBar.Foreground = new SolidColorBrush(Colors.Green);
                TimerText.Foreground = new SolidColorBrush(Colors.Green);
                TimerText.Text = $"Осталось: {remainingTime} сек";

                // Перезапускаем таймер
                stopwatch.Reset();
                stopwatch.Start();
                timer.Start();
            }
            else
            {
                TimerPanel.Visibility = Visibility.Collapsed;
                timer.Stop();
                stopwatch.Stop();
            }

            foreach (var option in eventToShow.Options)
            {
                var button = new Button
                {
                    Content = option.Text,
                    Tag = option,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    IsEnabled = option.CanChoose(playerStats, skillSystem)
                };

                if (!button.IsEnabled)
                {
                    button.ToolTip = option.GetRequirementText();
                }

                button.Click += (s, e) => ProcessChoice((Option)((Button)s).Tag);
                OptionsPanel.Children.Add(button);
            }
        }
        private Option ModifyOption(Option opt, int playerLevel, int eventMinLevel, int creativity, int resilience)
        {
            if (opt == null) return null;
            if (opt.Effects == null) return opt; // Возвращаем оригинальный вариант если нет эффектов

            double difficulty = Math.Max(1.0, (playerLevel - eventMinLevel) * 0.15);

            // Используем новый метод CreateModified
            var newEffect = opt.Effects.CreateModified(difficulty, creativity, resilience);

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

            // Создаем текст с требованиями
            string optionText = opt.Text;
            var requirements = new List<string>();

            if (opt.Effects.RequiredCreativity > 0)
                requirements.Add($"Требуется креативность: {opt.Effects.RequiredCreativity}");

            if (opt.Effects.RequiredSkills?.Contains("Устойчивость") == true)
                requirements.Add("Требуется устойчивость: 30");

            if (requirements.Any())
                optionText += $"\n[{string.Join(", ", requirements)}]";

            return new Option(optionText, newEffect);
        }
        private void ProcessChoice(Option selectedOption)
        {
            timer.Stop();
            stopwatch.Stop();
            TimerPanel.Visibility = Visibility.Collapsed;

            // Сохраняем начальные значения ДО применения эффектов
            oldKnowledge = playerStats.Knowledge;
            oldAwareness = playerStats.Awareness;
            oldMotivation = playerStats.Motivation;
            oldResilience = playerStats.Resilience;
            oldCreativity = playerStats.Creativity;
            int oldExp = playerStats.Experience;

            var report = new StringBuilder();
            report.AppendLine("\nВыбранное действие:");
            report.AppendLine(selectedOption.Text);

            double damageReduction = Math.Min(0.5, playerStats.Resilience / 200.0);

            // Описываем базовые эффекты
            report.AppendLine("\n1. Базовые эффекты от выбора:");
            Logger.Log($"Базовые эффекты выбора '{selectedOption.Text}':");

            // Показываем базовые эффекты для всех характеристик до их применения
            DescribeEffects(selectedOption.Effects, report, damageReduction, playerStats);

            // Показываем текущую защиту от урона
            if (damageReduction > 0 && HasNegativeEffects(selectedOption.Effects))
            {
                report.AppendLine($"\nТекущая защита от урона:");
                report.AppendLine($"Устойчивость: {playerStats.Resilience}");
                report.AppendLine($"Снижение урона: {damageReduction:P0}");
                Logger.Log($"Защита от урона: Устойчивость {playerStats.Resilience}, снижение {damageReduction:P0}");
            }

            // Показываем активные эффекты до применения новых
            report.AppendLine("\n2. Активные временные эффекты:");
            DescribeActiveEffects(activeEffects, report);

            // Показываем новые временные эффекты
            if (selectedOption.Effects.TemporaryEffects?.Any() == true)
            {
                report.AppendLine("\n3. Новые временные эффекты:");
                DescribeTemporaryEffects(selectedOption.Effects.TemporaryEffects, report);
            }

            // Применяем эффекты ОДИН раз
            playerStats.ApplyEffects(selectedOption.Effects);

            if (playerStats.Level >= 5)
            {
                SkillsPanel.Visibility = Visibility.Visible;
                var availableSkills = skillSystem.GetAvailableSkills(playerStats);
                SkillsList.ItemsSource = availableSkills;
            }

            if (selectedOption.Effects.TemporaryEffects?.Any() == true)
            {
                foreach (var tempEffect in selectedOption.Effects.TemporaryEffects)
                {
                    
                }
            }


            // Анимируем изменения
            AnimateProgressBar(KnowledgeProgress, KnowledgeText, KnowledgeChangeText,
                oldKnowledge, playerStats.Knowledge);
            AnimateProgressBar(AwarenessProgress, AwarenessText, AwarenessChangeText,
                oldAwareness, playerStats.Awareness);
            AnimateProgressBar(MotivationProgress, MotivationText, MotivationChangeText,
                oldMotivation, playerStats.Motivation);

            if (playerStats.Level >= 6)
            {
                AdvancedStatsPanel.Visibility = Visibility.Visible;
                AnimateProgressBar(ResilienceProgress, ResilienceText, ResilienceChangeText,
                    oldResilience, playerStats.Resilience);
                AnimateProgressBar(CreativityProgress, CreativityText, CreativityChangeText,
                    oldCreativity, playerStats.Creativity);
            }

            // Обновляем прогресс уровня
            LevelProgress.Value = playerStats.Experience % playerStats.ExperienceToNextLevel;
            LevelProgress.Maximum = playerStats.ExperienceToNextLevel;
            LevelText.Text = $"Уровень {playerStats.Level} ({playerStats.Experience}/{playerStats.ExperienceToNextLevel})";

            // Показываем итоговые изменения
            // Показываем итоговые изменения
            report.AppendLine("\nИтоговые изменения характеристик:");

            // Просто показываем чистую разницу между конечным и начальным значением
            int knowledgeChange = playerStats.Knowledge - oldKnowledge;
            int awarenessChange = playerStats.Awareness - oldAwareness;
            int motivationChange = playerStats.Motivation - oldMotivation;
            int expChange = playerStats.Experience - oldExp;

            if (knowledgeChange != 0)
                report.AppendLine($"Знания: {(knowledgeChange > 0 ? "+" : "")}{knowledgeChange}");
            if (awarenessChange != 0)
                report.AppendLine($"Осознанность: {(awarenessChange > 0 ? "+" : "")}{awarenessChange}");
            if (motivationChange != 0)
                report.AppendLine($"Мотивация: {(motivationChange > 0 ? "+" : "")}{motivationChange}");

            report.AppendLine($"\nПолучено опыта: {(expChange > 0 ? "+" : "")}{expChange}");

            // Обновляем отображение активных эффектов
            UpdateActiveEffectsDisplay();

            // Показываем объяснение
            ExplanationText.Text = currentEvent.Explanation + "\n" + report.ToString();
            ExplanationBox.Visibility = Visibility.Visible;
            NextButton.IsEnabled = true;

            // Деактивируем кнопки выбора
            foreach (var child in OptionsPanel.Children)
            {
                if (child is Button btn) btn.IsEnabled = false;
            }
        }

        // Вспомогательные методы
        private void DescribeEffects(Effect effects, StringBuilder report, double damageReduction, PlayerStats stats)
        {
            if (effects.KnowledgeEffect != 0)
                DescribeEffect(report, "Знания", effects.KnowledgeEffect, damageReduction,
                    stats._skillSystem.HasSkill("Критическое мышление"), 0.2, "Критическое мышление");

            if (effects.AwarenessEffect != 0)
                DescribeEffect(report, "Осознанность", effects.AwarenessEffect, damageReduction,
                    stats._skillSystem.HasSkill("Осознанность"), 0.15, "Осознанность");

            if (effects.MotivationEffect != 0)
                DescribeEffect(report, "Мотивация", effects.MotivationEffect, damageReduction,
                    stats._skillSystem.HasSkill("Самомотивация"), 0.1, "Самомотивация");
        }
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var infoWindow = new InfoWindow();
            infoWindow.Owner = this; // Делаем главное окно владельцем

            // Применяем эффект размытия к главному окну
            var blurEffect = new System.Windows.Media.Effects.BlurEffect
            {
                Radius = 5
            };
            this.Effect = blurEffect;

            // Показываем окно и ждем его закрытия
            infoWindow.ShowDialog();

            // Убираем эффект размытия
            this.Effect = null;
        }
        private void DescribeEffect(StringBuilder report, string statName, int effect, double damageReduction,
            bool hasSkill, double skillBonus, string skillName)
        {
            bool isPositive = effect > 0;
            string baseEffect = $"{statName}: {(isPositive ? "+" : "")}{effect}";

            if (isPositive && hasSkill)
            {
                baseEffect += $" (Навык '{skillName}' увеличивает на {skillBonus:P0})";
                Logger.Log($"Базовый эффект {statName.ToLower()}: {baseEffect}");
            }
            else if (!isPositive && damageReduction > 0)
            {
                int reducedEffect = (int)(effect * (1 - damageReduction));
                baseEffect += $" (Снижено устойчивостью до {reducedEffect})";
                Logger.Log($"Базовый эффект {statName.ToLower()} с уменьшением урона: {baseEffect}");
            }
            report.AppendLine(baseEffect);
        }

        private bool HasNegativeEffects(Effect effects)
        {
            return effects.KnowledgeEffect < 0 || effects.AwarenessEffect < 0 || effects.MotivationEffect < 0;
        }
        private void DescribeActiveEffects(List<TemporaryEffect> effects, StringBuilder report)
        {
            if (effects.Any(e => e.Duration > 0))
            {
                foreach (var effect in effects.Where(e => e.Duration > 0))
                {
                    string effectInfo = $"- {effect.Name} (осталось ходов: {effect.Duration})";
                    report.AppendLine(effectInfo);

                    if (effect.KnowledgeEffect != 0)
                        report.AppendLine($"  Знания {(effect.KnowledgeEffect > 0 ? "+" : "")}{effect.KnowledgeEffect}");
                    if (effect.AwarenessEffect != 0)
                        report.AppendLine($"  Осознанность {(effect.AwarenessEffect > 0 ? "+" : "")}{effect.AwarenessEffect}");
                    if (effect.MotivationEffect != 0)
                        report.AppendLine($"  Мотивация {(effect.MotivationEffect > 0 ? "+" : "")}{effect.MotivationEffect}");

                    Logger.Log($"Активный эффект: {effectInfo}");
                }
            }
            else
            {
                report.AppendLine("Нет активных эффектов");
                Logger.Log("Нет активных временных эффектов");
            }
        }

        private void DescribeTemporaryEffects(List<TemporaryEffect> effects, StringBuilder report)
        {
            foreach (var effect in effects)
            {
                string newEffect = $"- {effect.Name} ({effect.Duration} ходов)";
                report.AppendLine(newEffect);

                if (effect.KnowledgeEffect != 0)
                    report.AppendLine($"  Знания {(effect.KnowledgeEffect > 0 ? "+" : "")}{effect.KnowledgeEffect}");
                if (effect.AwarenessEffect != 0)
                    report.AppendLine($"  Осознанность {(effect.AwarenessEffect > 0 ? "+" : "")}{effect.AwarenessEffect}");
                if (effect.MotivationEffect != 0)
                    report.AppendLine($"  Мотивация {(effect.MotivationEffect > 0 ? "+" : "")}{effect.MotivationEffect}");

                Logger.Log($"Добавлен новый временный эффект: {newEffect}");
            }
        }

        private void ShowFinalChanges(StringBuilder report, int oldKnowledge, int oldAwareness,
            int oldMotivation, int oldResilience, int oldCreativity, int oldExp)
        {
            var changes = new[]
            {
        ("Знания", playerStats.Knowledge - oldKnowledge),
        ("Осознанность", playerStats.Awareness - oldAwareness),
        ("Мотивация", playerStats.Motivation - oldMotivation),
        ("Устойчивость", playerStats.Resilience - oldResilience),
        ("Креативность", playerStats.Creativity - oldCreativity)
    };

            bool hasChanges = false;
            foreach (var (stat, change) in changes)
            {
                if (change != 0)
                {
                    report.AppendLine($"{stat}: {(change > 0 ? "+" : "")}{change}");
                    Logger.Log($"Итоговое изменение {stat.ToLower()}: {change}");
                    hasChanges = true;
                }
            }

            if (!hasChanges)
                report.AppendLine("Характеристики не изменились");

            // Опыт
            int expGained = playerStats.Experience - oldExp;
            report.AppendLine($"\n5. Получено опыта: {(expGained > 0 ? "+" : "")}{expGained}");
            Logger.Log($"Получено опыта: {expGained}");
        }

        private void UpdateActiveEffectsDisplay()
        {
            var effectsToDisplay = playerStats.ActiveEffects

                 .Where(e => e.Duration > 0)
                .Select(e => new
                {
                    Name = e.Name,
                    RemainingTurns = $"Осталось ходов: {e.Duration}",
                    Effects = new List<string>
                    {
                e.KnowledgeEffect != 0 ? $"Знания: {(e.KnowledgeEffect > 0 ? "+" : "")}{e.KnowledgeEffect}" : null,
                e.AwarenessEffect != 0 ? $"Осознанность: {(e.AwarenessEffect > 0 ? "+" : "")}{e.AwarenessEffect}" : null,
                e.MotivationEffect != 0 ? $"Мотивация: {(e.MotivationEffect > 0 ? "+" : "")}{e.MotivationEffect}" : null
                    }.Where(s => s != null),
                    TextColor = IsPositiveEffect(e) ? new SolidColorBrush(Colors.DarkGreen) : new SolidColorBrush(Colors.DarkRed),
                    BackgroundColor = IsPositiveEffect(e) ? new SolidColorBrush(Color.FromRgb(220, 255, 220)) : new SolidColorBrush(Color.FromRgb(255, 220, 220)),
                    BorderColor = IsPositiveEffect(e) ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red)
                })
                .ToList();

            ActiveEffectsDisplay.ItemsSource = effectsToDisplay;
        }

        private bool IsPositiveEffect(TemporaryEffect effect)
        {
            return effect.KnowledgeEffect >= 0 &&
                   effect.AwarenessEffect >= 0 &&
                   effect.MotivationEffect >= 0;
        }



        private bool CheckGameOver()
        {
            if (playerStats.Knowledge <= 0 || playerStats.Awareness <= 0 || playerStats.Motivation <= 0)
            {
                EventText.Text = "ИГРА ОКОНЧЕНА\nВаши показатели упали слишком низко";
                EventCategory.Text = "Поражение";
                OptionsPanel.Children.Clear();
                ExplanationBox.Visibility = Visibility.Collapsed;
                NextButton.IsEnabled = false;
                return true;
            }
            return false;
        }

        
        private void MotivationProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Логика при необходимости (можно оставить пустым)
        }

        private void SkillCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Skill skill)
            {
                if (checkBox.IsChecked == true)
                {
                    if (!skillSystem.TryAcquireSkill(skill.Name, playerStats))
                    {
                        checkBox.IsChecked = false;
                    }
                    else
                    {
                        // Обновляем UI после приобретения навыка
                        UpdateUI();
                        MessageBox.Show($"Вы изучили навык: {skill.Name}!", "Успех",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
        private void AnimateProgressBar(ProgressBar progressBar, TextBlock valueText, TextBlock changeText,
    int oldValue, int newValue, string format = "0")
        {
            var animation = new DoubleAnimation
            {
                From = oldValue,
                To = newValue,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            progressBar.Value = newValue;
            valueText.Text = newValue.ToString(format);

            int change = newValue - oldValue;
            if (change != 0)
            {
                changeText.Text = (change > 0 ? " +" : " ") + change.ToString();
                changeText.Foreground = change > 0 ?
                    new SolidColorBrush(Colors.Green) :
                    new SolidColorBrush(Colors.Red);

                // Анимация исчезновения текста изменения
                var fadeOut = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(2),
                    BeginTime = TimeSpan.FromSeconds(1)
                };
                changeText.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            }
            else
            {
                changeText.Text = "";
            }
        }

        private void UpdateUI()
        {
            // Сначала обновляем базовые характеристики
            KnowledgeProgress.Value = playerStats.Knowledge;
            AwarenessProgress.Value = playerStats.Awareness;
            MotivationProgress.Value = playerStats.Motivation;

            KnowledgeText.Text = $"{playerStats.Knowledge}/100";
            AwarenessText.Text = $"{playerStats.Awareness}/100";
            MotivationText.Text = $"{playerStats.Motivation}/100";

            // Сбрасываем тексты изменений
            KnowledgeChangeText.Text = "";
            AwarenessChangeText.Text = "";
            MotivationChangeText.Text = "";

            // Обновляем прогресс уровня
            LevelProgress.Value = playerStats.Experience;
            LevelProgress.Maximum = playerStats.ExperienceToNextLevel;
            LevelText.Text = $"Уровень {playerStats.Level} ({playerStats.Experience}/{playerStats.ExperienceToNextLevel})";

            // Проверяем уровень для показа панели навыков
            if (playerStats.Level >= 5)
            {
                // Показываем панель навыков
                SkillsPanel.Visibility = Visibility.Visible;
                var availableSkills = skillSystem.GetAvailableSkills(playerStats);
                SkillsList.ItemsSource = availableSkills;
            }
            else
            {
                SkillsPanel.Visibility = Visibility.Collapsed;
            }

            // Отдельно проверяем продвинутые характеристики
            if (playerStats.Level >= 6)
            {
                AdvancedStatsPanel.Visibility = Visibility.Visible;
                ResilienceProgress.Value = playerStats.Resilience;
                ResilienceText.Text = $"{playerStats.Resilience}/100";
                CreativityProgress.Value = playerStats.Creativity;
                CreativityText.Text = $"{playerStats.Creativity}/100";
            }
            else
            {
                AdvancedStatsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private int GetPlayerStatValue(string statName, PlayerStats playerStats)
        {
            switch (statName)
            {
                case "Knowledge": return playerStats.Knowledge;
                case "Awareness": return playerStats.Awareness;
                case "Motivation": return playerStats.Motivation;
                case "Resilience": return playerStats.Resilience;
                case "Creativity": return playerStats.Creativity;
                default: return 0;
            }
        }
    }
}