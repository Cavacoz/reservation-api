using Microsoft.AspNetCore.Mvc;
using ReservationAPI.Services;

namespace ReservationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {

        private readonly ILogger<ReservationController> _logger;
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService, ILogger<ReservationController> logger)
        {
            _logger = logger;
            _reservationService = reservationService;
        }

        /// <summary>
        /// Gets all reservations.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Getting all reservations from service.");
            var reservations = await _reservationService.GetReservations();

            if (reservations.Count == 0)
            {
                _logger.LogInformation("No reservations available.");
                return NoContent();
            }

            _logger.LogInformation($"Returning {reservations.Count} reservations");
            return Ok(reservations);
        }

        /// <summary>
        /// Gets a specific reservation
        /// </summary>
        /// <param name="reservationId">Reservation Id</param>
        /// <returns>Returns the reservation with that id</returns>
        [HttpGet("{reservationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReservation(int reservationId)
        {
            _logger.LogInformation($"Getting reservation {reservationId} from service.");
            var reservation = await _reservationService.GetReservation(reservationId);

            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(reservation);
        }

        /// <summary>
        /// Adds a reservation
        /// </summary>
        /// <param name="reservationName">Reservations Name</param>
        /// <param name="year">Reservation Day</param>
        /// <param name="month">Reservation Day</param>
        /// <param name="day">Reservation Day</param>
        /// <param name="user">Reservation's</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddReservation(string reservationName, int year, int month, int day, string user)
        {
            DateOnly reservationDay = new(year, month, day);
            _logger.LogInformation($"Calling Service to add reservation to this day {reservationDay}");
            await _reservationService.AddReservation(reservationName, reservationDay, user);
            return CreatedAtAction(nameof(AddReservation), new { reservationNAME = reservationName, reservationDAY = reservationDay });
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
            _logger.LogWarning($"Calling Service to delete reservation with Id: {reservationId}");
            bool wasDeleted = await _reservationService.DeleteReservation(reservationId);
            if (!wasDeleted)
            {
                return NotFound("Either it wasn't deleted or not found!");
            }
            return NoContent();
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
            catch (NotImplementedException)
            {
                _logger.LogError("Http method PUT not implementd!");
                return StatusCode(501, "Method is not yet implemented");
            }
            return NoContent();
        }
    }
}