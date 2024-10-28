using SportFactoryApp;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SportFactoryApp.Memberships;// Add this if AddMemberWindow is in the Members namespace
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using System.Windows.Media;
using SportFactoryApp.Profile;
using static SportFactoryApp.MainWindow;

namespace SportFactoryApp.Members
{
    public partial class MembersView : UserControl
    {
        private GymContext _context;
        private MainWindow _mainWindow;
        public MembersView(MainWindow mainWindow)
        {
            InitializeComponent();
            _context = new GymContext();
            LoadMembers();
            LoadMemberships();
            _mainWindow = mainWindow;
        }

        private void LoadMemberships()
        {
            var memberships = _context.Membershipss.Include(m => m.Member).ToList();
           
        }

        // Load Members from the database and display them in the ListBox
        private void LoadMembers()
        {
            var members = _context.Members.ToList();
            MembersDataGrid.ItemsSource = members;

            // Calculate total members
            int totalMembers = members.Count;

            // Calculate active members with 12-Session Pack
            int activeMembersWith12Pack = _context.Membershipss
                .Count(m => m.Status == "Active" && m.Type == "Pack 12 Seances");

            // Calculate loyalty percentage
            double loyaltyPercentage = totalMembers > 0
                ? (double)activeMembersWith12Pack / totalMembers * 100
                : 0;

            // Update TextBlocks with calculated values
            TotalMembersText.Text = totalMembers.ToString();
            ActiveMembersText.Text = activeMembersWith12Pack.ToString();
            LoyaltyPercentageText.Text = $"{loyaltyPercentage:F2}%";
            CalculateAttendanceRate();

           
        }

        private void MembersDataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Get the clicked element
            var clickedElement = e.OriginalSource as FrameworkElement;

            // Check if the clicked element is a button within the DataGrid
            if (clickedElement is Button)
            {
                return; // Allow button clicks
            }

            // Get the row under the mouse click
            var row = FindParent<DataGridRow>(clickedElement);
            if (row != null)
            {
                // Prevent the row from being selected
                e.Handled = true;
            }
        }

        // Helper method to find the parent of a specified type
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T parent)
                {
                    return parent;
                }
                child = VisualTreeHelper.GetParent(child);
            }
            return null;
        }



        // Add Member Event Handler
        private void AddMember_Click(object sender, RoutedEventArgs e)
        {
            var addMemberWindow = new AddMemberWindow();
            if (addMemberWindow.ShowDialog() == true) // If user confirms addition
            {
                var newMember = addMemberWindow.NewMember;
                _context.Members.Add(newMember);
                _context.SaveChanges();
                LoadMembers(); // Refresh the list
                
            }
        }

        // Update Member Event Handler
        private void UpdateMember_Click(object sender, RoutedEventArgs e)
        {
            if (MembersDataGrid.SelectedItem is Member selectedMember)
            {
                var updateMemberWindow = new UpdateMemberWindow(selectedMember);
                if (updateMemberWindow.ShowDialog() == true) // If user confirms update
                {
                    _context.SaveChanges(); // Save changes made in the update window
                    LoadMembers(); // Refresh the list
                    LoadMemberships();
                }
            }
            else
            {
                MessageBox.Show("Please select a member to update.");
            }
        }

        // Delete Member Event Handler
        private void DeleteMember_Click(object sender, RoutedEventArgs e)
        {
            if (MembersDataGrid.SelectedItem is Member selectedMember)
            {
                _context.Members.Remove(selectedMember);
                _context.SaveChanges();
                LoadMembers(); // Refresh the list
                LoadMemberships();
            }
            else
            {
                MessageBox.Show("Please select a member to delete.");
            }
        }

        // Add Membership Event Handler
        private void AddMembership_Click(object sender, RoutedEventArgs e)
        {
            var addMembershipWindow = new AddMembershipWindow();
            if (addMembershipWindow.ShowDialog() == true) // If user confirms addition
            {
                //var newMembership = addMembershipWindow.NewMembership;
                //_context.Membershipss.Add(newMembership);
                //_context.SaveChanges();
                LoadMemberships(); // Refresh the list
            }
        }

        // Update Membership Event Handler
       
        private void MembersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // You can implement any logic you want to execute when a member is selected
            // For example, you might want to display details of the selected member:
            if (MembersDataGrid.SelectedItem is Member selectedMember)
            {
                // Display details or perform any actions with the selected member
                // For example, you might load the memberships associated with the selected member
                // LoadMembershipsForMember(selectedMember); // Optional method to implement
            }
        }
        
       
        private void ShowMemberProfile_Click(object sender, RoutedEventArgs e)
        {
            // Ensure the _mainWindow instance is available
            if (_mainWindow == null)
            {
                MessageBox.Show("Main window instance is not available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check if a member is selected in the DataGrid
            if (MembersDataGrid.SelectedItem is Member selectedMember)
            {
                // Fetch member details based on the selected member ID
                var member = GetMemberById(selectedMember.MemberId);

                if (member != null)
                {
                    try
                    {
                        // Create the MemberProfileView UserControl
                        var memberProfileView = new MemberProfileView(member);

                        // Set the UserControl to MainContentControl in MainWindow
                        _mainWindow.MainContentControl.Content = memberProfileView;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while displaying member profile: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Member not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a member.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private Member GetMemberById(int memberId)
        {
            try
            {
                using (var context = new GymContext())
                {
                    // Fetch the member by ID from the database
                    return context.Members.FirstOrDefault(m => m.MemberId == memberId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while retrieving member details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private void CalculateAttendanceRate()
        {
            // Total possible sessions for active members with "Pack 12 Seances"
            int activeMembersWith12Pack = _context.Membershipss
                .Count(m => m.Status == "Active" && m.Type == "Pack 12 Seances");

            // Calculate the maximum possible sessions (12 per active member with the pack)
            int totalPossibleSessions = activeMembersWith12Pack * 12;

            // Count the actual attended sessions for members with "Pack 12 Seances" and "Active" status
            int attendedSessions = _context.Sessions
                .Count(s => s.Membership.Status == "Active" && s.Membership.Type == "Pack 12 Seances");
                //.Any(m => m.Type == "Pack 12 Seances" && m.Status == "Active"));

            // Calculate attendance rate as a percentage
            double attendanceRate = totalPossibleSessions > 0
                ? (double)attendedSessions / totalPossibleSessions * 100
                : 0;

            // Display the attendance rate in a TextBlock
            AttendanceRateText.Text = $"{attendanceRate:F2}%";
            
        }




    }
}
