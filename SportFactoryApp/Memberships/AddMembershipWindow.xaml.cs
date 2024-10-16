using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SportFactoryApp.Memberships
{
    public partial class AddMembershipWindow : Window
    {
        private GymContext _context;

        // Property to hold the newly created membership
        public Membership NewMembership { get; private set; }

        public AddMembershipWindow()
        {
            InitializeComponent();
            _context = new GymContext();
            LoadMembers();
        }

        // Load members from the database
        private void LoadMembers()
        {
            var members = _context.Members.ToList();
            MemberComboBox.ItemsSource = members;
            MemberComboBox.DisplayMemberPath = "FullName"; // Display full name in the ComboBox
        }

        // Event handler for the Add button
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (MemberComboBox.SelectedItem is Member selectedMember &&
                decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                // Create a new membership object
                NewMembership = new Membership
                {
                    MemberId = selectedMember.MemberId,
                    Type = (MembershipType.SelectedItem as ComboBoxItem)?.Content.ToString(),
                   // Type = TypeTextBox.Text,
                    Price = price,
                    Status = "Active",
                    Date = DateTime.Now
                    // Do not assign MembershipId here
                };

                _context.Membershipss.Add(NewMembership);
                _context.SaveChanges();

                MessageBox.Show("Membership added successfully!");
                this.DialogResult = true; // Set dialog result to true
                this.Close(); // Close the window
            }
            else
            {
                MessageBox.Show("Please select a member and enter a valid price.");
            }
        }

        // Event handler for the Cancel button
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Set dialog result to false
            this.Close(); // Close the window
        }
    }
}
