using System;
using System.ComponentModel.DataAnnotations;

public class Membership
{
    [Key]
    public int MembershipId { get; set; }
    public int MemberId { get; set; }
    public string Type { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; } = "Active"; // Default to "Active"
    public DateTime Date { get; set; } = DateTime.Now; // Default to now

    public virtual Member Member { get; set; }
}
