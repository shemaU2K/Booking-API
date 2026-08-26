namespace Booking_API.Domain.Entities
{
    public class RoomService
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
