using Booking_API.Infrastructure;
using MediatR;

namespace Booking_API.Application.Rooms.Commands;

/// <summary>
/// Represents a command to delete an existing room from the booking system based on its unique identifier (ID).
/// </summary>
public class DeleteRoomCommand : IRequest
{
    public Guid Id { get; set; }
}

/// <summary>
/// Handles the DeleteRoomCommand by removing an existing Room entity from the database based on its unique identifier (ID).
/// </summary>
public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand>
{
    private readonly BookingDbContext _context;

    public DeleteRoomCommandHandler(BookingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the deletion of a room based on the provided DeleteRoomCommand.
    /// If the room with the specified ID does not exist, a KeyNotFoundException is thrown.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _context.Rooms.FindAsync(new object[] { request.Id }, cancellationToken);
        if (room == null)
            throw new KeyNotFoundException($"Room with ID {request.Id} not found.");

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync(cancellationToken);
    }
}