namespace ReservationAPI.DTO
{
    public class SendReservationDto
    {
        public int ReservationID { get; set; }
        public string ReservationName { get; set; }

        public DateOnly ReservationDay { get; set; }

    }
}