using System;
using System.Windows;
using System.Windows.Controls;

namespace SportFactoryApp.Members
{
    public partial class UpdateMemberWindow : Window
    {
        private GymContext _context;
        private Member _member;

        public UpdateMemberWindow(Member member)
        {
            InitializeComponent();
            _context = new GymContext();
            _member = member;

            // Load member details into the text boxes
            FirstNameTextBox.Text = _member.FirstName;
            LastNameTextBox.Text = _member.LastName;

            // Set gender in ComboBox
            GenderComboBox.SelectedItem = _member.Gender; // Assuming Gender is a string matching ComboBoxItem Content
            BirthDatePicker.SelectedDate = _member.BirthDate; // Assuming BirthDate is a DateTime
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Update member details
            _member.FirstName = FirstNameTextBox.Text;
            _member.LastName = LastNameTextBox.Text;
            _member.Gender = GenderComboBox.SelectedItem != null ? (GenderComboBox.SelectedItem as ComboBoxItem).Content.ToString() : null; // Get selected gender
            _member.BirthDate = BirthDatePicker.SelectedDate ?? DateTime.MinValue; // Handle date picker selection

            // Save changes to the database
            _context.Members.Update(_member);
            _context.SaveChanges();

            // Close the window
            this.DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
