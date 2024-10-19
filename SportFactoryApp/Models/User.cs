using System.Collections.Generic;

namespace SportFactory.Models
{
    public class User
    {
        public int Id { get; set; }                     // Primary key
        public string FirstName { get; set; }           // User's first name
        public string LastName { get; set; }            // User's last name
        public string Username { get; set; }            // Username for login
        public string Password { get; set; }            // Password for login
        public string Role { get; set; }                // User's role (e.g., Admin, Member)

        // Navigation properties to associate messages with users
        public ICollection<Message> SentMessages { get; set; }   // Messages sent by the user
        public ICollection<Message> ReceivedMessages { get; set; } // Messages received by the user
    }
}
