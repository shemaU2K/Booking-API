namespace Booking_API.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public decimal BasePrice { get; set; }

        public ICollection<RoomService> Services { get; set; } = new List<RoomService>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
}
