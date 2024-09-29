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
        private readonly List<Reservation> _reservations = [];

        private readonly AppDbContext _DbContext;

        private readonly ILogger<ReservationService> _logger;

        public ReservationService(AppDbContext dbContext, ILogger<ReservationService> logger)
        {
            _DbContext = dbContext;
            _logger = logger;
        }

        async Task<List<Reservation>> IReservationService.GetReservations()
        {
            _logger.LogInformation($"Returning {_DbContext.Reservations.Count()} reservations");
            return await _DbContext.Reservations.ToListAsync();
        }

        async Task<Reservation?> IReservationService.GetReservation(int reservationId)
        {
            _logger.LogInformation($"Trying to find reservations with Id: {reservationId}");
            Reservation? reservation = await _DbContext.Reservations.FirstOrDefaultAsync(r => r.ReservationID == reservationId);
            _logger.LogInformation(reservation == null ? "No servation found" : $"Found reservation: {reservation}");
            return reservation;
        }

        async Task IReservationService.AddReservation(string reservationName, DateOnly reservationDay, string user)
        {
            Reservation reservation = new() { ReservationName = reservationName, ReservationDay = reservationDay, User = user };
            _logger.LogWarning($"Adding this reservation: {reservation}");
            await _DbContext.AddAsync(reservation);
            await _DbContext.SaveChangesAsync();
            _logger.LogInformation($"Reservation {reservation} added.");
        }

        async Task<bool> IReservationService.DeleteReservation(int reservationId)
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

        void IReservationService.UpdateReservation(int reservationId)
        {
            throw new NotImplementedException();
        }

    }
}