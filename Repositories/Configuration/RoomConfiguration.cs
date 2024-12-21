using Entities.Enums;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configuration;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasData(
            new Room
            {
                Id = new Guid("ff46c213-a342-4d06-b0d4-fe668046dbfa"),
                Number = 1,
                Floor = 1,
                Type = RoomType.Single,
                Status = RoomStatus.Vacant,
                Price_Per_Night = 175,
            },
            new Room
            {
                Id = new Guid("023ee22e-a67f-4fc1-8801-c7437125815c"),
                Number = 26,
                Floor = 2,
                Type = RoomType.Suite,
                Status = RoomStatus.Occupied,
                Price_Per_Night = 250,
            }
        );
    }
}
