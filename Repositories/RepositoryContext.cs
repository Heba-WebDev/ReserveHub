using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Configuration;
namespace Repositories;
public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions<RepositoryContext> options)
    : base(options)
    {
    }

    public DbSet<Room> Rooms{ get; set; }
    public DbSet<Amenity> Amenities{ get; set; }
    public DbSet<RoomAmenity> RoomAmenities{ get; set; }
    public DbSet<Reservation> Reservations{ get; set; }
    public DbSet<Customer> Customers{ get; set; }
    public DbSet<User> Users{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Room>()
        .Property(x => x.Type)
        .HasConversion<string>()
        .IsRequired();

    modelBuilder.Entity<Room>()
        .Property(x => x.Status)
        .HasConversion<string>()
        .IsRequired();

    modelBuilder.Entity<RoomAmenity>()
        .HasKey(x => x.Id);

    modelBuilder.Entity<RoomAmenity>()
        .HasOne(x => x.Room)
        .WithMany(y => y.RoomAmenities)
        .HasForeignKey(x => x.RoomId);

    modelBuilder.Entity<RoomAmenity>()
        .HasOne(x => x.Amenity)
        .WithMany(y => y.RoomAmenities)
        .HasForeignKey(x => x.AmenityId);

    modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    modelBuilder.ApplyConfiguration(new RoomConfiguration());
}

}
