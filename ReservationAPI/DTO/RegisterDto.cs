using System.ComponentModel.DataAnnotations;

namespace ReservationAPI.DTO
{
    public class RegisterDto
    {
        public required string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email must include a valid domain like example@domain.com.")]
        public required string Email { get; set; }
        
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one number, and one special character.")]
        public required string Password { get; set; }
    }
}