using Microsoft.EntityFrameworkCore;
using club.Models;

namespace club.Context
{
    public class ClubDbContext : DbContext
    {
        public ClubDbContext(DbContextOptions<ClubDbContext> options) : base(options)
        { 
        }

        public DbSet<Sport> Sports { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sport>().ToTable("Sports");
            modelBuilder.Entity<Sport>().HasKey(p => p.Id);
            modelBuilder.Entity<Sport>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<Sport>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Sport>().HasMany(p => p.Courts).WithOne(p => p.Sport).HasForeignKey(p => p.SportId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Member>().ToTable("Members");
            modelBuilder.Entity<Member>().HasKey(p => p.Id);
            modelBuilder.Entity<Member>().Property(p => p.Name).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Member>().Property(p => p.Surname).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Member>().Property(p => p.Phone).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Member>().Property(p => p.Address).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Member>().HasMany(p => p.Bookings).WithOne(p => p.Member).HasForeignKey(p => p.MemberId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Court>().ToTable("Courts");
            modelBuilder.Entity<Court>().HasKey(p => p.Id);
            modelBuilder.Entity<Court>().Property(p => p.Reference).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Court>().Property(p => p.SportId).IsRequired();
            modelBuilder.Entity<Court>().HasMany(p => p.Bookings).WithOne(p => p.Court).HasForeignKey(p => p.CourtId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
