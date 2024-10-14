using System.Linq;
using System;
using System.Windows;
using System.Windows.Input;
using SportFactoryApp.Members;

namespace SportFactoryApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadUserProfile(); // Load user profile on startup
            ShowMembersView(); // Default view
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

        private void LoadUserProfile()
        {
            // Fetch user details based on CurrentUserId
            var user = GetUserById(LogSession.CurrentUserId); // Fetch user details
            if (user != null)
            {
                string fullName = $"{user.FirstName} {user.LastName}";
                LoggedInUserTextBlock.Text = fullName; // Display full name in TextBlock
            }
        }

        private User GetUserById(int userId)
        {
            // Implement the data access logic to get the user from the database
            try
            {
                using (var context = new GymContext())
                {
                    return context.Users.FirstOrDefault(u => u.Id == userId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while retrieving user details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private void MembersButton_Click(object sender, RoutedEventArgs e)
        {
            ShowMembersView();
        }

        private void SessionsButton_Click(object sender, RoutedEventArgs e)
        {
            var MembersView = new MembersView(); // Assuming ChargesView is a UserControl
            MainContentControl.Content = MembersView;
        }

        private void ShowMembersView()
        {
            // MainContent.Content = new MembersView(); // Replace with your Members view user control
        }

        private void ShowSessionsView()
        {
            // MainContent.Content = new SessionsView(); // Replace with your Sessions view user control
        }
    }
}
