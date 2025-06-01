using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EducationalEventGenerator
{
    public partial class MainWindow : Window
    {
        private PlayerStats playerStats = new PlayerStats();
        private EventSystem eventSystem = new EventSystem();
        private SkillSystem skillSystem = new SkillSystem();
        private Random random = new Random();

        private Event currentEvent;
        private List<TemporaryEffect> activeEffects = new List<TemporaryEffect>();
        private List<Event> eventHistory = new List<Event>();
        private EventChain currentChain = null;
        private int chainProgress = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSystems();
            UpdateUI();
        }

        private void InitializeSystems()
        {
            eventSystem.InitializeEvents();
            skillSystem.InitializeSkills();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = false;
            NextButton.IsEnabled = true;
            RestartButton.IsEnabled = true;
            ShowRandomEvent();
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
            playerStats = new PlayerStats();
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
            EventCategory.Text = eventToShow.Category.ToUpper();
            EventText.Text = eventToShow.Description;
            OptionsPanel.Children.Clear();
            ExplanationBox.Visibility = Visibility.Collapsed;
            NextButton.IsEnabled = false;

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
            playerStats.ApplyEffects(selectedOption.Effects);

            if (selectedOption.Effects.TemporaryEffects != null)
            {
                activeEffects.AddRange(selectedOption.Effects.TemporaryEffects);
                ActiveEffectsDisplay.ItemsSource = activeEffects.Where(e => e.Duration > 0);
            }

            ExplanationText.Text = currentEvent.Explanation;

            if (activeEffects.Any(e => e.Duration > 0))
            {
                TemporaryEffectsText.Text = "Активные эффекты:\n" +
                    string.Join("\n", activeEffects.Select(e => e.Description));
            }
            else
            {
                TemporaryEffectsText.Text = "";
            }

            ExplanationBox.Visibility = Visibility.Visible;
            NextButton.IsEnabled = true;

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
            }
            else
            {
                AdvancedStatsPanel.Visibility = Visibility.Collapsed;
            }

            LevelProgress.Value = playerStats.Experience;
            LevelProgress.Maximum = playerStats.ExperienceToNextLevel;
            LevelText.Text = $"Уровень {playerStats.Level} ({playerStats.Experience}/{playerStats.ExperienceToNextLevel})";

            SkillsPanel.Visibility = playerStats.Level >= 5 ? Visibility.Visible : Visibility.Collapsed;
            if (playerStats.Level >= 5)
            {
                SkillsList.ItemsSource = skillSystem.GetAvailableSkills(playerStats.Level);
            }
        }
    }
}