namespace BookingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a booking in the booking system, including its properties, associated room, and selected services.
    /// </summary>
    public class Booking
    {
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }

        public Room Room { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal TotalPrice { get; set; }

        public ICollection<Guid> SelectedServiceIds { get; set; } = new List<Guid>();
    }
}
