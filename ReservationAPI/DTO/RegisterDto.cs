using System.ComponentModel.DataAnnotations;

namespace ReservationAPI.DTO
{
    public class RegisterDto
    {
        public required string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email must include a valid domain like example@domain.com.")]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}