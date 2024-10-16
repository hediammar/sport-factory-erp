using System;

public class Member
{
    public int MemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now; // Default to now
    public string Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}
