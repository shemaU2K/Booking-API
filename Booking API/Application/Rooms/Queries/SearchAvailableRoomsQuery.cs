using Booking_API.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking_API.Application.Rooms.Queries;

/// <summary>
/// Represents a query to search for available rooms based on specified criteria.
/// </summary>
public class SearchAvailableRoomsQuery : IRequest<List<RoomResultDto>>
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }
}

/// <summary>
/// Represents a data transfer object (DTO) for the result of a room search, including its ID, name, and base price.
/// </summary>
public class RoomResultDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
}

/// <summary>
/// Handles the SearchAvailableRoomsQuery by querying the database for available rooms that meet the specified criteria,
/// returning a list of RoomResultDto objects.
/// </summary>
public class SearchAvailableRoomsQueryHandler : IRequestHandler<SearchAvailableRoomsQuery, List<RoomResultDto>>
{
    private readonly BookingDbContext _context;

    public SearchAvailableRoomsQueryHandler(BookingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the search for available rooms based on the provided query parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<RoomResultDto>> Handle(SearchAvailableRoomsQuery request, CancellationToken cancellationToken)
    {
        var availableRooms = await _context.Rooms
            .Where(r => r.Capacity >= request.Capacity)
            .Where(r => !r.Bookings.Any(b => request.StartTime < b.EndTime && request.EndTime > b.StartTime))
            .Select(r => new RoomResultDto
            {
                Id = r.Id,
                Name = r.Name,
                BasePrice = r.BasePrice
            })
            .ToListAsync(cancellationToken);

        return availableRooms;
    }
}