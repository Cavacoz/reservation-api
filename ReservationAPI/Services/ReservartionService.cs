using ReservationAPI.Models;
using ReservationAPI.Data;
using Microsoft.EntityFrameworkCore;
using ReservationAPI.DTO;
using ReservationAPI.Exceptions;

namespace ReservationAPI.Services
{
    public interface IReservationService
    {
        Task<List<SendReservationDto>> GetReservations(string userId);
        Task<SendReservationDto?> GetReservation(int reservationId, string userId);
        Task<SendReservationDto> AddReservation(string reservationName, DateOnly reservationDay, string user);
        Task<bool> DeleteReservation(int reservationId, string userId);
        void UpdateReservation(int reservationId);
    }

    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _DbContext;
        private readonly Logging.ILogger _customLogger;

        public ReservationService(AppDbContext dbContext, Logging.ILoggerFactory loggerFactory)
        {
            _DbContext = dbContext;
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

            if (reservation == null)
            {
                _customLogger.LogInformation($"No reservation found for User: {userId} with Id: {reservationId}");
                throw new ReservationNotFound($"No reservation found for User: {userId} with Id: {reservationId}");
            }
            _customLogger.LogInformation($"Found reservation: {reservationId} for User: {userId}");

            return new SendReservationDto {
                ReservationID = reservation.ReservationID, 
                ReservationName = reservation.ReservationName, 
                ReservationDay = reservation.ReservationDay
                };
        }

        public async Task<SendReservationDto> AddReservation(string reservationName, DateOnly reservationDay, string userId)
        {
            Reservation reservation = new() { ReservationName = reservationName, ReservationDay = reservationDay, UserId = int.Parse(userId) };
            await _DbContext.AddAsync(reservation);
            await _DbContext.SaveChangesAsync();
            _customLogger.LogInformation($"Reservation {reservation} added for User: {userId}.");

            return new SendReservationDto {
                ReservationID = reservation.ReservationID,
                ReservationName = reservation.ReservationName,
                ReservationDay = reservation.ReservationDay
                };
        }

        public async Task<bool> DeleteReservation(int reservationId, string userId)
        {
            Reservation? reservationToDelete = await _DbContext.Reservations.FirstOrDefaultAsync(r => r.ReservationID == reservationId && r.UserId == int.Parse(userId));
            
            if (reservationToDelete == null)
            {
                _customLogger.LogInformation($"Reservation {reservationId} was not found for deletion to User: {userId}.");
                return false;
            }
            
            _DbContext.Reservations.Remove(reservationToDelete);
            await _DbContext.SaveChangesAsync();
            _customLogger.LogInformation($"Reservation {reservationId} deleted for User: {userId}.");
            
            return true;
        }

        public void UpdateReservation(int reservationId)
        {
            throw new NotImplementedException("Method not yet implemented.");
        }

    }
}