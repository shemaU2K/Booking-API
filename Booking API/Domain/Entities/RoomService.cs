namespace Booking_API.Domain.Entities
{
    /// <summary>
    /// Represents an additional service that can be associated with a room in the booking system.
    /// </summary>
    public class RoomService
    {
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }

        public Room Room { get; set; } = null!;

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}
