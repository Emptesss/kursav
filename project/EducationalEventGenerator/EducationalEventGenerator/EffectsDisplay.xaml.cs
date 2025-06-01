using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EducationalEventGenerator
{
    public partial class EffectsDisplay : UserControl
    {
        public EffectsDisplay()
        {
            InitializeComponent();
        }

        public void UpdateEffects(List<TemporaryEffect> effects)
        {
            EffectsList.ItemsSource = effects;
            this.Visibility = effects.Count > 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
    }
}