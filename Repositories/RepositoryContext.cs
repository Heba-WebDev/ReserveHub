using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.Configuration;
namespace Repositories;
public class RepositoryContext : IdentityDbContext<User>
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasOne(x => x.Customers)
            .WithOne(x => x.User)
            .HasForeignKey<Customer>(x => x.UserId);

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
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }

}
