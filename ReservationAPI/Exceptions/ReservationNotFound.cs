namespace ReservationAPI.Exceptions
{
    public class ReservationNotFound : Exception
    {
        public ReservationNotFound(string message) : base(message) {}        
    }
}