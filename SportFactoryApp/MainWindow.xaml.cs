using System.Linq;
using System;
using System.Windows;

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
            ShowSessionsView();
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
