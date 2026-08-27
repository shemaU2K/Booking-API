using BookingAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly BookingDbContext _context;

    public ReportsController(BookingDbContext context)
    {
        _context = context;
    }

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