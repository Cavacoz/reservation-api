namespace ReservationAPI.Models
{
    public class User
    {
        public int UserId { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public string PasswordHash { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }

    }
}