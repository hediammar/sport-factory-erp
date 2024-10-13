using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SportFactoryApp
{
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
            Loaded += LoadingWindow_Loaded;
        }
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private async void LoadingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Show the loading window for 3 seconds
            await Task.Delay(5000);

            // After the loading, open the LoginWindow
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            // Close the loading window
            this.Close();
        }
    }
}
