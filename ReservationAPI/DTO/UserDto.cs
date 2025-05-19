namespace ReservationAPI.DTO
{
    public class UserDto
    {
        public int UserId { get; set; }
        public required string Email { get; set; }
        public required string JwtToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}