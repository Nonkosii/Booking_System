using Booking_System.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Booking_System.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1 - to - many Resource → Bookings
            modelBuilder.Entity<Resource>()
             .HasMany(r => r.Bookings)
             .WithOne(bk => bk.Resource)
             .HasForeignKey(bk => bk.ResourceId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resource>().HasData(
                new Resource { Id = 1, Name = "Meeting Room Alpha", Description = "Room with projector", Location = "1st Floor", Capacity = 10, IsAvailable = true },
                new Resource { Id = 2, Name = "Company Car 1", Description = "Compact sedan", Location = "Parking Lot", Capacity = 4, IsAvailable = true },
                new Resource { Id = 3, Name = "Conference Hall", Description = "Large room with AV", Location = "3rd Floor", Capacity = 30, IsAvailable = true }
    );
        }
    }
}
