using BookingAPI.Domain.Entities;
using BookingAPI.Infrastructure;
using BookingAPI.Application.Rooms.Command.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Application.Rooms.Command
{
    /// <summary>
    /// Represents a command to update an existing room in the booking system, including its properties and associated services.
    /// </summary>
    public class UpdateRoomCommand : IRequest<Unit>
    {
        public Guid Id { get; set; } = Guid.Empty;
        public int Capacity { get; set; } = 1;
        public decimal BasePrice { get; set; } = 0.0m;
        public List<ServiceDto> Services { get; set; } = new();
    }

    /// <summary>
    /// Handles the UpdateRoomCommand by updating an existing Room entity and its associated services in the database.
    /// </summary>
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, Unit>
    {
        private readonly BookingDbContext _context;

        public UpdateRoomCommandHandler(BookingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles the update of a room's details and its associated services in the database.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<Unit> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _context.Rooms
            .Include(r => r.Services)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (room == null)
            {
                throw new KeyNotFoundException($"Room with ID {request.Id} not found.");
            }

            room.Capacity = request.Capacity;
            room.BasePrice = request.BasePrice;
            room.Services.Clear();
            foreach (var serviceDto in request.Services)
            {
                room.Services.Add(new RoomService
                {
                    Id = Guid.NewGuid(),
                    Name = serviceDto.Name,
                    Price = serviceDto.Price,
                    RoomId = room.Id
                });
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
