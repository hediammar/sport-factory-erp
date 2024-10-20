using System;
using System.Windows;
using System.Windows.Controls;

namespace SportFactoryApp.Members
{
    public partial class AddMemberWindow : Window
    {
        public Member NewMember { get; private set; }

        public AddMemberWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NewMember = new Member
            {
                FirstName = FirstNameTextBox.Text,
                LastName = LastNameTextBox.Text,
                StartDate = DateTime.Now,
                PhoneNumber = PhoneNumberTextBox.Text,
                Gender = (GenderComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(), // Get selected gender
                BirthDate = BirthDatePicker.SelectedDate // Get the selected birthdate
        };
            DialogResult = true; // Close the window and return true
            Close();
        }
    }
}
