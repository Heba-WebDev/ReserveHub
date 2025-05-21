using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReserveHub.Domain.Entities;
using ReserveHub.Infrastructure.Identity;
namespace ReserveHub.Infrastructure.Repositories;

public class RepositoryContext : IdentityDbContext
{
    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Hotel>()
        .HasOne<ApplicationUser>()
        .WithMany(u => u.OwnedHotels)
        .HasForeignKey(h => h.OwnerId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Hotel>()
        .HasMany(h => h.Rooms)
        .WithOne(r => r.Hotel)
        .HasForeignKey(r => r.HotelId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Room>()
        .HasMany(r => r.Reservations)
        .WithOne(x => x.Room)
        .HasForeignKey(x => x.RoomId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
