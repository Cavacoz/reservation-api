namespace ReservationAPI.DTO
{
    public class SendReservationDto
    {
        public int ReservationID { get; set; }
        public required string ReservationName { get; set; }

        public DateOnly ReservationDay { get; set; }

    }
}