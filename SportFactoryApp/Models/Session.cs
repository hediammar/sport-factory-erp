using System;
using SportFactoryApp;
    public class Session
    {
        public int SessionId { get; set; }
        public int MemberId { get; set; }
        public DateTime SessionDate { get; set; }
        public int MembershipId { get; set; }

        public Member Member { get; set; }
        public Membership Membership { get; set; }// Navigation property
}