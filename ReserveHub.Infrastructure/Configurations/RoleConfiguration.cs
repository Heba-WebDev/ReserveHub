using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReserveHub.Infrastructure.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "e7efadf3-5c85-4500-9c3e-2639f96c4a82",
                Name = "Owner",
                NormalizedName = "OWNER"
            },
            new IdentityRole
            {
                Id = "5b0bf66b-8f06-40b8-9295-7f1a6b0d012b",
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            },
            new IdentityRole
            {
                Id = "a942161d-6faf-48d9-90b8-70f5582e1632",
                Name = "Staff",
                NormalizedName = "STAFF"
            },
            new IdentityRole
            {
                Id = "742be66d-2c78-4341-865a-93fb247d99ab",
                Name = "Admin",
                NormalizedName = "ADMIN"
            }
        );
    }
}