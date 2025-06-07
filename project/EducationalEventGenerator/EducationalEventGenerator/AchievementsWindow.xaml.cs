using System.Collections.Generic;
using System.Windows;

namespace EducationalEventGenerator
{
    /// <summary>
    /// Логика взаимодействия для AchievementsWindow.xaml
    /// </summary>
    public partial class AchievementsWindow : Window
    {
        public AchievementsWindow(List<Achievement> achievements)
        {
            InitializeComponent();
            AchievementsList.ItemsSource = achievements;
        }
    }
}