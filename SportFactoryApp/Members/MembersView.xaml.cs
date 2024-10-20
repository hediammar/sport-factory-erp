using SportFactoryApp;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SportFactoryApp.Memberships;// Add this if AddMemberWindow is in the Members namespace
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using System.Windows.Media;

namespace SportFactoryApp.Members
{
    public partial class MembersView : UserControl
    {
        private GymContext _context;

        public MembersView()
        {
            InitializeComponent();
            _context = new GymContext();
            LoadMembers();
            LoadMemberships();
        }

        private void LoadMemberships()
        {
            var memberships = _context.Membershipss.Include(m => m.Member).ToList();
            MembershipsDataGrid.ItemsSource = memberships; // Update this line
        }

        // Load Members from the database and display them in the ListBox
        private void LoadMembers()
        {
            var members = _context.Members.ToList();
            MembersDataGrid.ItemsSource = members;
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
        private void UpdateMembership_Click(object sender, RoutedEventArgs e)
        {
            if (MembershipsDataGrid.SelectedItem is Membership selectedMembership)
            {
                var updateMembershipWindow = new UpdateMembershipWindow(selectedMembership);
                if (updateMembershipWindow.ShowDialog() == true) // If user confirms update
                {
                    //_context.SaveChanges(); // Save changes made in the update window
                    LoadMemberships(); // Refresh the list
                }
            }
            else
            {
                MessageBox.Show("Please select a membership to update.");
            }
        }

        // Delete Membership Event Handler
        private void DeleteMembership_Click(object sender, RoutedEventArgs e)
        {
            if (MembershipsDataGrid.SelectedItem is Membership selectedMembership)
            {
                _context.Membershipss.Remove(selectedMembership);
                _context.SaveChanges();
                LoadMemberships(); // Refresh the list
            }
            else
            {
                MessageBox.Show("Please select a membership to delete.");
            }
        }
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
        
        private void MembershipsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) // Update this line
        {
            if (MembershipsDataGrid.SelectedItem is Membership selectedMembership) // Update this line
            {
                // Implement any logic you want when a membership is selected
            }
        }
    }
}
