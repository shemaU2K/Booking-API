using BookingAPI.Domain.Services;
using Booking_API.Domain.Entities;
using Booking_API.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking_API.Application.Rooms.Commands;

/// <summary>
/// Represents a command to book a room in the booking system, including the room ID, booking time range, and selected services.
/// </summary>
public class BookRoomCommand : IRequest<BookingResponseDto>
{
    public Guid RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<Guid> SelectedServiceIds { get; set; } = new();
}

/// <summary>
/// Represents the response data for a successful room booking, including the booking ID and total price.
/// </summary>
public class BookingResponseDto
{
    public Guid BookingId { get; set; }
    public decimal TotalPrice { get; set; }
}

/// <summary>
/// Handles the BookRoomCommand by checking room availability, calculating the total price including selected services,
/// and saving the booking to the database.
/// </summary>
public class BookRoomCommandHandler : IRequestHandler<BookRoomCommand, BookingResponseDto>
{
    private readonly BookingDbContext _context;
    private readonly IPricingCalculator _pricingCalculator;

    public BookRoomCommandHandler(BookingDbContext context, IPricingCalculator pricingCalculator)
    {
        _context = context;
        _pricingCalculator = pricingCalculator;
    }

    /// <summary>
    /// Handles the booking of a room based on the provided command. It checks for room availability, 
    /// calculates the total price including selected services, and saves the booking to the database.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<BookingResponseDto> Handle(BookRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _context.Rooms
            .Include(r => r.Bookings)
            .Include(r => r.Services)
            .FirstOrDefaultAsync(r => r.Id == request.RoomId, cancellationToken);

        if (room == null)
            throw new KeyNotFoundException($"Room with ID {request.RoomId} not found.");

        if (!room.IsAvailable(request.StartTime, request.EndTime))
            throw new InvalidOperationException("Зал уже заброньовано на обраний час.");

        var selectedServices = room.Services
            .Where(s => request.SelectedServiceIds.Contains(s.Id))
            .ToList();

        decimal servicesTotal = selectedServices.Sum(s => s.Price);

        decimal totalPrice = _pricingCalculator.CalculateTotal(
            room.BasePrice,
            request.StartTime,
            request.EndTime,
            servicesTotal);

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TotalPrice = totalPrice,
            SelectedServiceIds = request.SelectedServiceIds
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(cancellationToken);

        return new BookingResponseDto
        {
            BookingId = booking.Id,
            TotalPrice = booking.TotalPrice
        };
    }
}