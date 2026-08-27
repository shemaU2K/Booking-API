using BookingAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking_API.Controllers;

/// <summary>
/// Represents a controller for generating reports in the booking system, providing endpoints for retrieving revenue and booking statistics by room.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly BookingDbContext _context;

    /// <summary>
    /// Initializes a new instance of the ReportsController class with the specified BookingDbContext for accessing the database.
    /// </summary>
    /// <param name="context"></param>
    public ReportsController(BookingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a report of revenue and booking statistics for each room, including the room name, total number of bookings, and total revenue generated.
    /// </summary>
    /// <returns></returns>
    [HttpGet("revenue-by-room")]
    public async Task<IActionResult> GetRevenueByRoom()
    {
        var report = await _context.Rooms
            .Select(r => new
            {
                RoomName = r.Name,
                TotalBookings = r.Bookings.Count,
                TotalRevenue = r.Bookings.Sum(b => b.TotalPrice)
            })
            .OrderByDescending(x => x.TotalRevenue)
            .ToListAsync();

        return Ok(report);
    }
}