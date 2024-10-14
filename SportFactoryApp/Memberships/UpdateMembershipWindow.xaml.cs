using System.Linq;
using System.Windows;

namespace SportFactoryApp.Memberships
{
    public partial class UpdateMembershipWindow : Window
    {
        private GymContext _context;
        private Membership _membership;

        public UpdateMembershipWindow(Membership membership)
        {
            InitializeComponent();
            _context = new GymContext();
            _membership = membership;

            LoadMembers();
            LoadMembershipDetails();
        }

        // Load members from the database
        private void LoadMembers()
        {
            var members = _context.Members.ToList();
            MemberComboBox.ItemsSource = members;
            MemberComboBox.DisplayMemberPath = "FullName"; // Display full name in the ComboBox
        }

        // Load the membership details into the UI elements
        private void LoadMembershipDetails()
        {
            MemberComboBox.SelectedItem = _context.Members.Find(_membership.MemberId);
            TypeTextBox.Text = _membership.Type;
            PriceTextBox.Text = _membership.Price.ToString();
        }

        // Event handler for the Update button
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (MemberComboBox.SelectedItem is Member selectedMember &&
                decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                // Update the membership details
                _membership.MemberId = selectedMember.MemberId;
                _membership.Type = TypeTextBox.Text;
                _membership.Price = price;

                _context.SaveChanges();

                MessageBox.Show("Membership updated successfully!");
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
