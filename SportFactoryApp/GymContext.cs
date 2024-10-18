using Microsoft.EntityFrameworkCore;
using SportFactoryApp;

public class GymContext : DbContext
{
    public DbSet<Member> Members { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Membership> Membershipss { get; set; }

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
    }
}
