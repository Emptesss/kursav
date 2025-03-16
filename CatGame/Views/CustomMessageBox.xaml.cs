using System.Windows;

namespace CatGame.Views
{
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        public static void Show(string message)
        {
            var messageBox = new CustomMessageBox(message);
            messageBox.ShowDialog();
        }
    }
}