namespace ReservationAPI.DTO
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string JwtToken {get;set;}
    }
}