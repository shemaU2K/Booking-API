using BookingAPI.Application.Rooms.Command;
using BookingAPI.Application.Rooms.Commands;
using BookingAPI.Application.Rooms.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Booking_API.Controllers
{
    /// <summary>
    /// Represents a controller for managing conference rooms in the booking system,
    /// providing endpoints for creating, updating, deleting, searching, and booking rooms.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the RoomsController class with the specified IMediator for handling requests.
        /// </summary>
        /// <param name="mediator"></param>
        public RoomsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Creates a new conference room with the specified details.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] AddRoomCommand command)
        {
            var roomId = await mediator.Send(command);
            return Ok(new { Id = roomId, Message = "Room created successfully" });
        }

        /// <summary>
        /// Updates an existing conference room with the specified ID and details.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomCommand command)
        {
            command.Id = id;
            await mediator.Send(command);
            return Ok(new { Message = "Room updated successfully" });
        }

        /// <summary>
        /// Deletes an existing conference room with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            await mediator.Send(new DeleteRoomCommand { Id = id });
            return Ok(new { Message = "Room deleted successfully" });
        }

        /// <summary>
        /// Searches for available conference rooms based on the specified criteria.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("available")]
        public async Task<IActionResult> SearchAvailableRooms([FromQuery] SearchAvailableRoomsQuery query)
        {
            var rooms = await mediator.Send(query);
            return Ok(rooms);
        }

        /// <summary>
        /// Books a conference room with the specified ID and booking details.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{id:guid}/book")]
        public async Task<IActionResult> BookRoom(Guid id, [FromBody] BookRoomCommand command)
        {
            command.RoomId = id;
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}
