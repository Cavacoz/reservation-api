namespace ReservationAPI.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(string name);
    }

    public class LoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(string name)
        {
            return new FileLogger(name);
        }
    }

}