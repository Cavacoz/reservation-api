using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationAPI.Models
{

    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationID { get; set; }

        public required string ReservationName { get; set; }

        public required DateOnly ReservationDay { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public override string ToString()
        {
            return $"Reservation name {ReservationName} on this {ReservationDay} day for Customer {User}";
        }

    }

}