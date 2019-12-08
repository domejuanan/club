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
            modelBuilder.Entity<Sport>().ToTable("Sports");
            modelBuilder.Entity<Sport>().HasKey(p => p.Id);
            modelBuilder.Entity<Sport>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Sport>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Sport>().HasMany(p => p.Courts);


            //modelBuilder.Entity<Court>()                
            //    .HasOne<Sport>(s => s.Sport)
            //    .WithMany(c => c.Courts)
            //    .HasForeignKey(s => s.SportId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Booking>()
            //    .HasOne<Court>(c => c.Court);

            //modelBuilder.Entity<Booking>()
            //    .HasOne<Member>(m => m.Member);
        }
    }
}
