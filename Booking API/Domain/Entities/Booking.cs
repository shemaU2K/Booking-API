namespace Booking_API.Domain.Entities
{
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
