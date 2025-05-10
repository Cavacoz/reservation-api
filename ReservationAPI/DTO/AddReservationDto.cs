using System.ComponentModel.DataAnnotations;

namespace ReservationAPI.DTO
{
    public class AddReservationDto
    {
        public string ReservationName { get; set; }
        public int Year { get; set; }

        [Range(1, 12, ErrorMessage = "Month needs to be between 1 and 12!")]
        public int Month { get; set; }

        [ValidDay]
        public int Day { get; set; }
    }
}