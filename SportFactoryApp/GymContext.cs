using Microsoft.EntityFrameworkCore;
using SportFactory.Models;
using SportFactoryApp;

public class GymContext : DbContext
{
    public DbSet<Member> Members { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Membership> Membershipss { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-NOH724N;Database=SportFactory;Trusted_Connection=True;TrustServerCertificate=True;").EnableSensitiveDataLogging(); ;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>().ToTable("Members");
        modelBuilder.Entity<Session>().ToTable("Sessions");
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Membership>().ToTable("Membership");
        modelBuilder.Entity<Message>().ToTable("Messages");

        // Define one-to-many relationship for Sender
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderID)
            .OnDelete(DeleteBehavior.Restrict); // To prevent cascading deletes

        // Define one-to-many relationship for Receiver
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Receiver)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.ReceiverID)
            .OnDelete(DeleteBehavior.Restrict); // To prevent cascading deletes
    }
}
