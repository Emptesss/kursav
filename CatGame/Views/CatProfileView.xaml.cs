using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatGame.Views
{
    public partial class CatProfileView : UserControl
    {
        public CatProfileView()
        {
            InitializeComponent();
            this.Loaded += CatProfileView_Loaded;
        }

        private void CatProfileView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext == null)
            {
                MessageBox.Show("DataContext is null!");
            }
        }
    }
}
