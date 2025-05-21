namespace ReserveHub.Domain.Entities;

public class Hotel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Country { get; set; } = "";
    public string Location { get; set; } = "";

    public string OwnerId { get; set; } = "";

    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
