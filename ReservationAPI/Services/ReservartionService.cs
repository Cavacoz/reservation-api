using ReservationAPI.Models;

namespace ReservationAPI.Services
{
    public interface IReservationService
    {
        List<Reservation> GetReservations();
        Reservation? GetReservation(int reservationId);
        void AddReservation(int reservationId, string reservationName, DateOnly reservationDay, string user);
        bool DeleteReservation(int reservationId);
        void UpdateReservation(int reservationId);
    }

    class ReservationService : IReservationService
    {
        private readonly List<Reservation> _reservations = [];

        private readonly ILogger<ReservationService> _logger;

        public ReservationService(ILogger<ReservationService> logger)
        {
            _logger = logger;
        }

        List<Reservation> IReservationService.GetReservations()
        {
            _logger.LogInformation($"Returning {_reservations.Count} reservations");
            return _reservations;
        }

        Reservation? IReservationService.GetReservation(int reservationId)
        {
            _logger.LogInformation($"Trying to find reservations with Id: {reservationId}");
            var reservation = _reservations.Find(r => r.ReservationId == reservationId);
            _logger.LogInformation($"Found reservation: {reservation}");
            return reservation;
        }

        void IReservationService.AddReservation(int reservationId, string reservationName, DateOnly reservationDay, string user)
        {
            Reservation reservation = new() { ReservationId = reservationId, ReservationName = reservationName, ReservationDay = reservationDay, User = user };
            _logger.LogWarning($"Adding this reservation: {reservation}");
            _reservations.Add(reservation);
            _logger.LogInformation($"Reservation {reservation} added.");
        }

        bool IReservationService.DeleteReservation(int reservationId)
        {
            _logger.LogWarning($"Deleting reservation with Id: {reservationId}");
            var reservationToDelete = _reservations.Find(r => r.ReservationId == reservationId);
            if (reservationToDelete == null)
            {
                _logger.LogInformation("Reservation was not found or deleted.");
                return false;
            }
            _logger.LogInformation($"Reservation Deleted.");
            return _reservations.Remove(reservationToDelete);
        }

        void IReservationService.UpdateReservation(int reservationId)
        {
            throw new NotImplementedException();
        }

    }
}