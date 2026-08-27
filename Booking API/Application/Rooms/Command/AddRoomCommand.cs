using BookingAPI.Domain.Entities;
using BookingAPI.Infrastructure;
using BookingAPI.Application.Rooms.Command.DTOs;
using MediatR;

namespace BookingAPI.Application.Rooms.Commands;

/// <summary>
/// Represents a command to add a new room to the booking system, including its properties and associated services.
/// </summary>
public class AddRoomCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; } = 1;
    public decimal BasePrice { get; set; } = 0.0m;
    public List<ServiceDto> Services { get; set; } = new();
}

/// <summary>
/// Handles the AddRoomCommand by creating a new Room entity and saving it to the database, returning the ID of the newly created room.
/// </summary>
public class AddRoomCommandHandler : IRequestHandler<AddRoomCommand, Guid>
{
    private readonly BookingDbContext _context;

    /// <summary>
    /// Initializes a new instance of the AddRoomCommandHandler class with the specified BookingDbContext.
    /// </summary>
    /// <param name="context"></param>
    public AddRoomCommandHandler(BookingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the AddRoomCommand by creating a new Room entity, adding it to the database context,
    /// and saving changes. Returns the ID of the newly created room.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Guid> Handle(AddRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Capacity = request.Capacity,
            BasePrice = request.BasePrice,
            Services = request.Services.Select(s => new RoomService
            {
                Id = Guid.NewGuid(),
                Name = s.Name,
                Price = s.Price
            }).ToList()
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync(cancellationToken);

        return room.Id;
    }
}