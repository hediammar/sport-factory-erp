using System;

namespace SportFactory.Models
{
    public class Message
    {
        public int MessageID { get; set; }               // Primary key, unique identifier for each message
        public DateTime MessageDateTime { get; set; }    // Date and time of the message
        public string Content { get; set; }              // Content of the message
        public int SenderID { get; set; }                // Foreign key for the sender (UserID)
        public int ReceiverID { get; set; }              // Foreign key for the receiver (UserID)

        // Navigation properties (optional, depending on how you structure relationships)
        // Navigation properties for sender and receiver
        
        public virtual User Sender { get; set; }

        
        public virtual User Receiver { get; set; }
    }
}
