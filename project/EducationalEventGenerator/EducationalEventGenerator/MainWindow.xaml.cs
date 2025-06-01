using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer(); // Добавьте эту строку
            InitializeSystems();
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
            timer.Interval = TimeSpan.FromSeconds(1); // Точно 1 секунда
            timer.Tick += Timer_Tick;
            stopwatch = new Stopwatch();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (currentEvent is TimedEvent timedEvent)
            {
                double elapsedSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
                remainingTime = Math.Max(0, timedEvent.TimeLimit - (int)elapsedSeconds);

                TimeProgressBar.Value = remainingTime;
                TimeProgressBar.Maximum = timedEvent.TimeLimit;
                TimerText.Text = $"Осталось: {remainingTime} сек";

                if (remainingTime <= 0)
                {
                    timer.Stop();
                    stopwatch.Stop();
                    TimerPanel.Visibility = Visibility.Collapsed;

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
                    return;
                }

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
            if (CheckLevelUp()) return;

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
                TimeProgressBar.Maximum = timedEvent.TimeLimit;
                TimeProgressBar.Value = timedEvent.TimeLimit;
                TimeProgressBar.Foreground = new SolidColorBrush(Colors.Green);
                TimerText.Foreground = new SolidColorBrush(Colors.Green);
                TimerText.Text = $"Осталось: {remainingTime} сек";
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

        private void ProcessChoice(Option selectedOption)
        {

            timer.Stop();
            stopwatch.Stop();
            TimerPanel.Visibility = Visibility.Collapsed;
            // Вычисляем базовые изменения
            int totalKnowledge = selectedOption.Effects.KnowledgeEffect;
            int totalAwareness = selectedOption.Effects.AwarenessEffect;
            int totalMotivation = selectedOption.Effects.MotivationEffect;

            // Добавляем изменения от новых временных эффектов
            if (selectedOption.Effects.TemporaryEffects != null)
            {
                foreach (var effect in selectedOption.Effects.TemporaryEffects)
                {
                    totalKnowledge += effect.KnowledgeEffect;
                    totalAwareness += effect.AwarenessEffect;
                    totalMotivation += effect.MotivationEffect;
                }
            }

            // Добавляем изменения от уже активных эффектов
            foreach (var effect in activeEffects)
            {
                totalKnowledge += effect.KnowledgeEffect;
                totalAwareness += effect.AwarenessEffect;
                totalMotivation += effect.MotivationEffect;
            }

            // Формируем краткий отчет только с итоговыми значениями
            var report = new StringBuilder();
            report.AppendLine("\nИтоговые изменения:");
            if (totalKnowledge != 0) report.AppendLine($"Знания: {(totalKnowledge > 0 ? "+" : "")}{totalKnowledge}");
            if (totalAwareness != 0) report.AppendLine($"Осознанность: {(totalAwareness > 0 ? "+" : "")}{totalAwareness}");
            if (totalMotivation != 0) report.AppendLine($"Мотивация: {(totalMotivation > 0 ? "+" : "")}{totalMotivation}");

            // Сначала применяем эффекты
            int oldExp = playerStats.Experience;
            playerStats.ApplyEffects(selectedOption.Effects);
            int expGained = playerStats.Experience - oldExp;
            if (expGained > 0)
            {
                report.AppendLine($"Опыт: +{expGained}");
            }

            // Обновляем временные эффекты
            if (selectedOption.Effects.TemporaryEffects != null)
            {
                activeEffects.AddRange(selectedOption.Effects.TemporaryEffects);
                ActiveEffectsDisplay.ItemsSource = activeEffects.Where(e => e.Duration > 0);
            }

            // Показываем объяснение и итоговый отчет
            ExplanationText.Text = currentEvent.Explanation + report.ToString();
            ExplanationBox.Visibility = Visibility.Visible;
            NextButton.IsEnabled = true;

            // Деактивируем кнопки выбора
            foreach (var child in OptionsPanel.Children)
            {
                if (child is Button btn) btn.IsEnabled = false;
            }

            UpdateUI();
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

        private bool CheckLevelUp()
        {
            if (playerStats.Level >= 15)
            {
                EventText.Text = "ПОБЕДА!\nВы достигли максимального уровня мастерства!";
                EventCategory.Text = "Поздравляем!";
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
                        MessageBox.Show($"Не выполнены требования для изучения навыка {skill.Name}!\n{skill.GetRequirementsText()}",
                                      "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void UpdateUI()
        {
            KnowledgeProgress.Value = playerStats.Knowledge;
            AwarenessProgress.Value = playerStats.Awareness;
            MotivationProgress.Value = playerStats.Motivation;

            KnowledgeText.Text = $"{playerStats.Knowledge}/100";
            AwarenessText.Text = $"{playerStats.Awareness}/100";
            MotivationText.Text = $"{playerStats.Motivation}/100";

            if (playerStats.Level >= 6)
            {
                AdvancedStatsPanel.Visibility = Visibility.Visible;
                ResilienceProgress.Value = playerStats.Resilience;
                ResilienceText.Text = $"{playerStats.Resilience}/100";

                // Добавляем обновление креативности
                CreativityProgress.Value = playerStats.Creativity;
                CreativityText.Text = $"{playerStats.Creativity}/100";
            }
            else
            {
                AdvancedStatsPanel.Visibility = Visibility.Collapsed;
            }

            KnowledgeChangeText.Text = "";
            AwarenessChangeText.Text = "";
            MotivationChangeText.Text = "";

            // Обновление прогресса уровня
            LevelProgress.Value = playerStats.Experience;
            LevelProgress.Maximum = playerStats.ExperienceToNextLevel;
            LevelText.Text = $"Уровень {playerStats.Level} ({playerStats.Experience}/{playerStats.ExperienceToNextLevel})";

            SkillsPanel.Visibility = playerStats.Level >= 5 ? Visibility.Visible : Visibility.Collapsed;
            if (playerStats.Level >= 5)
            {
                SkillsList.ItemsSource = skillSystem.GetAvailableSkills(playerStats);
            }
        }
    }
}