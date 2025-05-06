using ReservationAPI.Models;
using ReservationAPI.Data;
using Microsoft.EntityFrameworkCore;
using ReservationAPI.DTO;

namespace ReservationAPI.Services
{
    public interface IReservationService
    {
        Task<List<SendReservationDto>> GetReservations(string userId);
        Task<SendReservationDto?> GetReservation(int reservationId, string userId);
        Task<Reservation> AddReservation(string reservationName, DateOnly reservationDay, string user);
        Task<bool> DeleteReservation(int reservationId);
        void UpdateReservation(int reservationId);
    }

    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _DbContext;
        private readonly ILogger<ReservationService> _logger;
        private readonly Logging.ILogger _customLogger;

        public ReservationService(AppDbContext dbContext, ILogger<ReservationService> logger, Logging.ILoggerFactory loggerFactory)
        {
            _DbContext = dbContext;
            _logger = logger;
            _customLogger = loggerFactory.CreateLogger("ReservationService.txt");
        }

        public async Task<List<SendReservationDto>> GetReservations(string userId)
        {

            var reservationsFromUser = await _DbContext.Reservations
                .Where(r => r.UserId == int.Parse(userId))
                .Select(r => new SendReservationDto 
                {
                    ReservationID = r.ReservationID,
                    ReservationName = r.ReservationName,
                    ReservationDay = r.ReservationDay
                })
                .ToListAsync();

            _customLogger.LogInformation($"Returning {reservationsFromUser.Count} reservations to User: {userId}");

            return reservationsFromUser;
        }

        public async Task<SendReservationDto?> GetReservation(int reservationId, string userId)
        {
            Reservation? reservation = await _DbContext.Reservations.FirstOrDefaultAsync(r => r.ReservationID == reservationId && r.UserId == int.Parse(userId));

            _customLogger.LogInformation(reservation == null ? "No servation found" : $"Found reservation: {reservation}");

            return reservation != null ? new SendReservationDto {
                ReservationID = reservation.ReservationID, 
                ReservationName = reservation.ReservationName, 
                ReservationDay = reservation.ReservationDay} 
                : null;
        }

        public async Task<Reservation> AddReservation(string reservationName, DateOnly reservationDay, string userId)
        {

            Reservation reservation = new() { ReservationName = reservationName, ReservationDay = reservationDay, UserId = int.Parse(userId) };
            await _DbContext.AddAsync(reservation);
            await _DbContext.SaveChangesAsync();
            _customLogger.LogInformation($"Reservation {reservation} added.");

            return reservation;
        }

        public async Task<bool> DeleteReservation(int reservationId)
        {
            // _logger.LogWarning($"Deleting reservation with Id: {reservationId}");
            Reservation? reservationToDelete = await _DbContext.Reservations.FirstOrDefaultAsync(r => r.ReservationID == reservationId);
            if (reservationToDelete == null)
            {
                // _logger.LogInformation("Reservation was not found or deleted.");
                _customLogger.LogInformation($"Reservation {reservationId} was not found or deleted.");
                return false;
            }
            _DbContext.Reservations.Remove(reservationToDelete);
            await _DbContext.SaveChangesAsync();
            // _logger.LogInformation($"Reservation Deleted.");
            _customLogger.LogInformation($"Reservation {reservationId} Deleted.");
            return true;
        }

        public void UpdateReservation(int reservationId)
        {
            throw new NotImplementedException();
        }

    }
}