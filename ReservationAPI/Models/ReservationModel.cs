namespace ReservationAPI.Models
{

    public class Reservation
    {
        public int ReservationId { get; set; }

        public required string ReservationName { get; set; }

        public required DateOnly ReservationDay { get; set; }

        public required string User { get; set; }

        public override string ToString()
        {
            return $"Reservation name {ReservationName} on this {ReservationDay} day for Customer {User}";
        }

    }

}