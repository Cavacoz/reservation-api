using ReservationAPI.Models;
using ReservationAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ReservationAPI.Services
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetReservations();
        Task<Reservation?> GetReservation(int reservationId);
        Task AddReservation(string reservationName, DateOnly reservationDay, string user);
        Task<bool> DeleteReservation(int reservationId);
        void UpdateReservation(int reservationId);
    }

    class ReservationService : IReservationService
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

        public async Task<List<Reservation>> GetReservations()
        {
            _logger.LogInformation($"Returning {_DbContext.Reservations.Count()} reservations");
            return await _DbContext.Reservations.ToListAsync();
        }

        public async Task<Reservation?> GetReservation(int reservationId)
        {
            _logger.LogInformation($"Trying to find reservations with Id: {reservationId}");
            Reservation? reservation = await _DbContext.Reservations.FirstOrDefaultAsync(r => r.ReservationID == reservationId);
            _logger.LogInformation(reservation == null ? "No servation found" : $"Found reservation: {reservation}");
            return reservation;
        }

        public async Task AddReservation(string reservationName, DateOnly reservationDay, string user)
        {
            Reservation reservation = new() { ReservationName = reservationName, ReservationDay = reservationDay, User = user };
            _logger.LogWarning($"Adding this reservation: {reservation}");
            await _DbContext.AddAsync(reservation);
            await _DbContext.SaveChangesAsync();
            _logger.LogInformation($"Reservation {reservation} added.");
        }

        public async Task<bool> DeleteReservation(int reservationId)
        {
            _logger.LogWarning($"Deleting reservation with Id: {reservationId}");
            Reservation? reservationToDelete = await _DbContext.Reservations.FirstOrDefaultAsync(r => r.ReservationID == reservationId);
            if (reservationToDelete == null)
            {
                _logger.LogInformation("Reservation was not found or deleted.");
                return false;
            }
            _DbContext.Reservations.Remove(reservationToDelete);
            await _DbContext.SaveChangesAsync();
            _logger.LogInformation($"Reservation Deleted.");
            return true;
        }

        public void UpdateReservation(int reservationId)
        {
            throw new NotImplementedException();
        }

    }
}