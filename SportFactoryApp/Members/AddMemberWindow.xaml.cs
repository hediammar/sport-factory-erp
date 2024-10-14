using System;
using System.Windows;

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
                StartDate = DateTime.Now
            };
            DialogResult = true; // Close the window and return true
            Close();
        }
    }
}
