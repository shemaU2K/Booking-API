namespace Booking_API.Application.Rooms.Command.DTOs
{
     /// <summary>
     /// Represents a data transfer object (DTO) for a service associated with a room, including its name and price.
     /// </summary>
     public class ServiceDto
     {
         public string Name { get; set; } = string.Empty;
         public decimal Price { get; set; }
     }
}
