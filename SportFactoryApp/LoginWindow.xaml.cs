using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation; // Add this for Storyboard

namespace SportFactoryApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            // Set window to full screen
            this.WindowState = WindowState.Maximized;
            // Remove window borders if desired
        }
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void MaximizeRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                MaximizeRestoreButton.Content = "❐"; // Restore button icon
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                MaximizeRestoreButton.Content = "◻"; // Maximize button icon
            }
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            // Disable the login button to prevent multiple clicks
            LoginButton.IsEnabled = false;

            // Validate the username and password asynchronously
            var validationTask = Task.Run(() => ValidateUser(username, password));
            bool isValidUser = await validationTask;

            if (isValidUser)
            {
                // If validation is successful, get the user ID asynchronously
                LogSession.CurrentUserId = await Task.Run(() => GetUserId(username));

                // Trigger the fade-out animation
                await FadeOut();

                // Open the MainWindow and close the LoginWindow
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close(); // Close the login window
            }
            else
            {
                // Show an error message if validation fails
                MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Re-enable the login button
            LoginButton.IsEnabled = true;
        }


        private bool ValidateUser(string username, string password)
        {
            try
            {
                using (var context = new GymContext())
                {
                    // Check if there's a user with the given username and password
                    return context.Users.Any(user => user.Username == username && user.Password == password);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while validating the user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private int GetUserId(string username)
        {
            try
            {
                using (var context = new GymContext())
                {
                    // Retrieve the UserId for the given username
                    return context.Users.Where(user => user.Username == username)
                                       .Select(user => user.Id)
                                       .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while retrieving the user ID: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0; // Assuming 0 indicates an error or no user found
            }
        }

        // Method to start fade-out animation after a successful login
        private async Task FadeOut()
        {
            // Create a fade-out animation
            var fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
            fadeOutAnimation.Completed += FadeOutAnimation_Completed; // Handle the animation completion

            // Begin the animation on the window's opacity
            this.BeginAnimation(Window.OpacityProperty, fadeOutAnimation);

            // Wait for the animation to finish
            await Task.Delay(1000);
        }

        // Event handler for when the fade-out animation completes
        private void FadeOutAnimation_Completed(object sender, EventArgs e)
        {
            // Optionally, you can perform additional cleanup here
        }
    }
}
