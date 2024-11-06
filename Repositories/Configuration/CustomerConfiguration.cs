using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configuration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasData(
            new Customer
            {
                Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                FirstName = "John",
                LastName = "Doe",
                Email = "john_doe@test.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Test123_456"),
                PhoneNumber = "462225252",
                Address = "Street 102, Building 3, House 11"
            }
        );
    }
}