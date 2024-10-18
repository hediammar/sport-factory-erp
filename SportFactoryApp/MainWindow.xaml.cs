using System.Linq;
using System;
using System.Windows;
using System.Windows.Input;
using SportFactoryApp.Members;
using SportFactoryApp.Profile;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SportFactoryApp
{
    public partial class MainWindow : Window
    {
        private ToggleButton _lastCheckedButton;

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

       /* private void MembersButton_Click(object sender, RoutedEventArgs e)
        {
            ShowMembersView();
        }*/

        private void MembersButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton clickedButton = sender as ToggleButton;

            // If there's a previously checked button and it's different from the clicked button, uncheck it
            if (_lastCheckedButton != null && _lastCheckedButton != clickedButton)
            {
                _lastCheckedButton.IsChecked = false;
            }

            // Set the clicked button as the new last checked button
            _lastCheckedButton = clickedButton;

            var MembersView = new MembersView(); // Assuming ChargesView is a UserControl
            MainContentControl.Content = MembersView;
        }

        private void MembershipsButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton clickedButton = sender as ToggleButton;

            // If there's a previously checked button and it's different from the clicked button, uncheck it
            if (_lastCheckedButton != null && _lastCheckedButton != clickedButton)
            {
                _lastCheckedButton.IsChecked = false;
            }

            // Set the clicked button as the new last checked button
            _lastCheckedButton = clickedButton;

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
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTextBox.Text;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                SearchResultListBox.ItemsSource = null;
                SearchResultPopup.IsOpen = false;
                return;
            }

            // Fetch member names and IDs from the database based on the search term
            var filteredResults = GetMemberNames(searchText);

            // Update ListBox with filtered results
            SearchResultListBox.ItemsSource = filteredResults;

            // Show or hide the Popup based on whether there are results
            SearchResultPopup.IsOpen = filteredResults.Any();
        }

        private List<MemberSearchResult> GetMemberNames(string searchTerm)
        {
            using (var context = new GymContext())
            {
                var members = context.Members.ToList(); // Load all members into memory

                return members
                    .Where(m => string.IsNullOrWhiteSpace(searchTerm) ||
                                 m.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
                                 m.LastName.ToLower().Contains(searchTerm.ToLower()))
                    .Select(m => new MemberSearchResult
                    {
                        Id = m.MemberId, // Assuming Id is the primary key
                        FullName = $"{m.FirstName} {m.LastName}"
                    })
                    .ToList();
            }
        }

        private void SearchResultListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchResultListBox.SelectedItem is MemberSearchResult selectedMember)
            {
                // Fetch member details based on the selected member ID
                var member = GetMemberById(selectedMember.Id); // Fetch using the member ID

                if (member != null)
                {
                    // Create the MemberProfileView UserControl
                    var memberProfileView = new MemberProfileView(member); // Pass the member to the UserControl

                    // Set the UserControl to MainContentControl
                    MainContentControl.Content = memberProfileView;

                    // Close the search results
                    SearchResultPopup.IsOpen = false;
                }
            }
        }

        private Member GetMemberById(int memberId)
        {
            try
            {
                using (var context = new GymContext())
                {
                    return context.Members.FirstOrDefault(m => m.MemberId == memberId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while retrieving member details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        // Helper class to hold member search results
        public class MemberSearchResult
        {
            public int Id { get; set; }
            public string FullName { get; set; }

            public override string ToString() => FullName; // Override ToString to display FullName in ListBox
        }



        private void ShowMemberProfile(Member selectedMember)
        {
            // Pass the selected member to the MemberProfileView constructor
            var memberProfileView = new MemberProfileView(selectedMember);
            memberProfileView.DataContext = selectedMember;  // Optionally, bind the selected member to the DataContext
            MainContentControl.Content = memberProfileView;  // Display the profile in the main content area
        }
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton clickedButton = sender as ToggleButton;

            // If there's a previously checked button and it's different from the clicked button, uncheck it
            if (_lastCheckedButton != null && _lastCheckedButton != clickedButton)
            {
                _lastCheckedButton.IsChecked = false;
            }

            // Set the clicked button as the new last checked button
            _lastCheckedButton = clickedButton;
        }



    }
}
