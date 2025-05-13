namespace ReservationAPI.DTO
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public required string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}