using System.ComponentModel.DataAnnotations;

namespace ReservationAPI.DTO
{
    public class RegisterDto
    {
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email Address.")]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}