using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SportFactoryApp.Profile
{
    /// <summary>
    /// Logique d'interaction pour MemberProfileView.xaml
    /// </summary>
    public partial class MemberProfileView : UserControl
    {
        private readonly GymContext _context;
        public Member Member { get; private set; }
        public int MembershipCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public MemberProfileView(Member member)
         
            {
                InitializeComponent();

                _context = new GymContext();

                // Fetch memberships from the database for the given member
                member.Memberships = FetchMembershipsForMember(member.MemberId);

                DisplayMemberData(member);
            //LoadMemberData();
            DataContext = member;
            Member = member; // Assign the passed member to the property
           
        }

        private Membership GetNewestActiveMembership()
        {
            // Check if Member is initialized
            if (Member == null)
            {
                MessageBox.Show("Member is not initialized. Please load member data.");
                return null; // Return null if no member is loaded
            }

            // Retrieve the newest active membership for the member
            var activeMembership = _context.Membershipss
                .Where(m => m.MemberId == Member.MemberId && m.Status == "Active") // Assuming IsActive is a property that indicates if the membership is active
                .OrderByDescending(m => m.Date) // Assuming StartDate is the property indicating when the membership started
                .FirstOrDefault(); // Get the most recent active membership

            return activeMembership; // Return the found membership or null if none exists
        }

        private List<Membership> FetchMembershipsForMember(int memberId)
        {
            // Ensure your DbSet name is correct
            var memberships = _context.Membershipss // Ensure the DbSet is named correctly (not Membershipss)
                .Where(m => m.MemberId == memberId)
                .OrderByDescending(m => m.Date)
                .ToList();

            // Debugging step: Ensure data is correct
            foreach (var membership in memberships)
            {
                Console.WriteLine($"MembershipType: {membership.Type}, StartDate: {membership.Date}");
            }

            return memberships;
        }
        private void DisplayMemberData(Member member)
        {
            // Assuming Member has a list of Memberships
            int membershipCount = member.Memberships.Count;
            decimal totalRevenue = member.Memberships.Sum(m => m.Price);

            // Set these values in the UI directly
            MembershipCountTextBlock.Text = membershipCount.ToString();
            int TotalRevenueString = (int)totalRevenue;
            string TotalDinars = TotalRevenueString.ToString() + " DT";
            TotalRevenueTextBlock.Text = TotalDinars;// Formats as currency
        }

        private void MembershipListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMembership = MembershipListView.SelectedItem as Membership;
            if (selectedMembership != null)
            {
                // Fetch the sessions for the selected membership
                var sessions = FetchSessionsForMembership(selectedMembership.MembershipId);

                // Set the sessions as the data source for the SessionsListView
                SessionsListView.ItemsSource = sessions;
                LoadSessionsForMembership(selectedMembership);
            }
        }
        private List<Session> FetchSessionsForMembership(int membershipId)
        {
            // Fetch sessions for the selected membership
            var sessions = _context.Sessions
                .Where(s => s.MembershipId == membershipId)
                .OrderByDescending(s => s.SessionDate)
                .ToList();

            return sessions;
        }

        private void LoadSessionsForMembership(Membership membership)
        {
            // Fetch the sessions related to the selected membership
            var sessions = _context.Sessions
                .Where(s => s.MembershipId == membership.MembershipId)
                .ToList();

            // Display the sessions in the ListView
            SessionsListView.ItemsSource = sessions;

            // Check if the number of sessions has reached 12
            if (_context.Sessions.Count(s => s.MembershipId == membership.MembershipId) >= 12)

                {
                    // Change the membership status to "Inactive"
                    membership.Status = "Desactive";

                // Save the changes to the database
                _context.SaveChanges();

                // Notify the user
                MessageBox.Show("This membership is now inactive due to 12 completed sessions.");

                // Refresh the memberships ListView to show the updated status
                MembershipListView.Items.Refresh();
            }
            else
            {
                MessageBox.Show("This membership is active ");
            }
        }

        private void AddSessionButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the newest active membership
            var activeMembership = GetNewestActiveMembership();

            if (activeMembership != null)
            {
                // Create a new session and associate it with the active membership
                var newSession = new Session
                {
                    MemberId = activeMembership.MemberId, // Use the member's ID from the active membership
                    SessionDate = DateTime.Now, // Set the session date to now or customize as needed
                    MembershipId = activeMembership.MembershipId // Associate with the active membership
                };

                try
                {
                    _context.Sessions.Add(newSession); // Add the new session to the context
                    _context.SaveChanges(); // Save changes to the database

                    // Check the total number of sessions for the active membership
                    int sessionCount = _context.Sessions.Count(s => s.MembershipId == activeMembership.MembershipId);
                    if (sessionCount >= 12)
                    {
                        // Change the membership status to "Desactive"
                        activeMembership.Status = "Desactive";
                        _context.SaveChanges(); // Save changes to update the membership status

                        // Notify the user
                        MessageBox.Show("This membership is now inactive due to 12 completed sessions.");
                    }

                    // Refresh the sessions list to show the newly added session
                    LoadSessions(activeMembership.MembershipId);
                    LoadMembershipsForMember(Member.MemberId);
                    // Notify the user that the session was added successfully
                    MessageBox.Show("Session added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during the save process
                    MessageBox.Show($"An error occurred while adding the session: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Notify the user that there is no active membership
                MessageBox.Show("No active membership found to add a session.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private void DeleteSessionButton_Click(object sender, RoutedEventArgs e)
        {
            if (SessionsListView.SelectedItem is Session selectedSession)
            {
                // Confirm deletion
                var result = MessageBox.Show("Are you sure you want to delete this session?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _context.Sessions.Remove(selectedSession);
                    _context.SaveChanges();

                    // Refresh sessions list
                    LoadSessions(selectedSession.MembershipId);
                }
            }
            else
            {
                MessageBox.Show("Please select a session to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadMembershipsForMember(int memberId)
        {
            // Fetch the memberships from the database
            var memberships = _context.Membershipss
                .Where(m => m.MemberId == memberId)
                .OrderByDescending(m => m.Date)
                .ToList();

            // Update the ItemsSource for the MembershipListView
            MembershipListView.ItemsSource = memberships;
        }

        /*private Membership GetNewestActiveMembership()
        {
            // Check if _member is null
            if (memo == null)
            {
                throw new InvalidOperationException("Member is not initialized.");
            }

            return _context.Membershipss
                .Where(m => m.MemberId == memo.MemberId && m.Status == "Active")
                .OrderByDescending(m => m.Date)
                .FirstOrDefault();
        }*/
        /*private void LoadMemberData()
        {
            // Assuming you have a method to get the member by ID
            Member = GetMemberById(memberId);
            if (Member == null)
            {
                // Handle the case where the member is not found
                MessageBox.Show("Member not found.");
                return;
            }
        }*/


        private void LoadSessions(int membershipId)
        {
            var sessions = _context.Sessions
                .Where(s => s.MembershipId == membershipId)
                .ToList();

            SessionsListView.ItemsSource = sessions; // Assuming SessionListView is the name of your ListView for sessions
        }
        private void SessionListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteSessionButton.IsEnabled = SessionsListView.SelectedItem != null;
        }







    }


}


