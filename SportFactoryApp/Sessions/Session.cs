using System;
using SportFactoryApp;
    public class Session
    {
        public int SessionId { get; set; }
        public int MemberId { get; set; }
        public DateTime SessionDate { get; set; }
        public int SessionNumber { get; set; }

        public Member Member { get; set; } // Navigation property
    }