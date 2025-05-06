namespace ReservationAPI.DTO
{
    public class AddReservationDto
    {
        public string ReservationName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}