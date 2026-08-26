namespace Booking_API.Domain.Entities
{
    /// <summary>
    /// Represents a room in the booking system, including its properties, associated services, and bookings.
    /// </summary>
    public class Room
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public decimal BasePrice { get; set; }

        public ICollection<RoomService> Services { get; set; } = new List<RoomService>();

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public bool IsAvailable(DateTime startTime, DateTime endTime)
        {
            if (!Bookings.Any()) return true;

            bool hasOverlap = Bookings.Any(b =>
                startTime < b.EndTime && endTime > b.StartTime
            );

            return !hasOverlap;
        }
    }
}