using System.Threading.Tasks;
using System.Windows;

namespace SportFactoryApp
{
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
            Loaded += LoadingWindow_Loaded;
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
