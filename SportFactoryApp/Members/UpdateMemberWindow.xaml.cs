using System.Windows;

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
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Update member details
            _member.FirstName = FirstNameTextBox.Text;
            _member.LastName = LastNameTextBox.Text;

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
