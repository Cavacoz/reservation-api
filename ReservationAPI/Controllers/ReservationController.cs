using Microsoft.AspNetCore.Mvc;
using ReservationAPI.Services;
using ReservationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ReservationAPI.DTO;
using ReservationAPI.Exceptions;

namespace ReservationAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly Logging.ILogger _customLogger;

        public ReservationController(IReservationService reservationService, Logging.ILoggerFactory loggerFactory)
        {
            _reservationService = reservationService;
            _customLogger = loggerFactory.CreateLogger("ReservationController.txt");
        }

        /// <summary>
        /// Gets all reservations for the specific logged in User.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<SendReservationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _customLogger.LogInformation($"Getting all reservations from service for User: {userId}.");

            var reservations = await _reservationService.GetReservations(userId);

            if (reservations.Count == 0)
            {
                _customLogger.LogInformation($"No reservations available for User:  {userId}.");
                return NoContent();
            }

            _customLogger.LogInformation($"Returning {reservations.Count} reservations to User: {userId}");
            return Ok(reservations);
        }

        /// <summary>
        /// Gets a specific reservation for the current logged in User.
        /// </summary>
        /// <param name="reservationId">Reservation Id</param>
        /// <returns>Returns the reservation with that id</returns>
        [HttpGet("{reservationId}")]
        [ProducesResponseType(typeof(SendReservationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReservation(int reservationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _customLogger.LogInformation($"Getting reservation {reservationId} from service for User: {userId}.");

            SendReservationDto? reservation;
            try 
            {
                reservation = await _reservationService.GetReservation(reservationId, userId);
            } 
            catch (ReservationNotFound ex)
            {
                return NotFound(ex.Message);
            }

            return Ok(reservation);
        }

        /// <summary>
        /// Adds a reservation with the current logged in user.
        /// </summary>
        /// <param name="dto">Data Transfer Object with information about the reservation to add.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(SendReservationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddReservation(AddReservationDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            DateOnly reservationDay = new(dto.Year, dto.Month, dto.Day);
            _customLogger.LogInformation($"Calling Service to add reservation to this day {reservationDay} for User: {userId}");
            SendReservationDto reservation = await _reservationService.AddReservation(dto.ReservationName, reservationDay, userId);

            return CreatedAtAction(nameof(AddReservation), reservation);
        }

        /// <summary>
        /// Deletes a reservation
        /// </summary>
        /// <param name="reservationId">Reservation Id</param>
        /// <returns></returns>
        [HttpDelete("{reservationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _customLogger.LogWarning($"Calling Service to delete reservation with Id: {reservationId} for User: {userId}");
            bool wasDeleted = await _reservationService.DeleteReservation(reservationId, userId);

            return wasDeleted ? NoContent() : NotFound();
        }

        /// <summary>
        /// Updates a reservation
        /// </summary>
        /// <param name="reservationId">Reservation Id</param>
        /// <returns></returns>
        [HttpPut("{reservationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public IActionResult UpdateReservation(int reservationId)
        {
            try
            {
                _reservationService.UpdateReservation(reservationId);
            }
            catch (NotImplementedException ex)
            {
                _customLogger.LogError(ex.Message);
                return StatusCode(501, ex.Message);
            }
            return NoContent();
        }
    }
}