namespace ReservationAPI.Logging
{
    enum LogLevel
    {
        LogInformation,
        LogWarning,
        LogCritical,
        LogError,
    }

    public interface ILogger
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogCritical(string message);
        void LogError(string message);
    }

    class FileLogger : ILogger
    {

        private static readonly string _RootDirectory = Path.Combine(Directory.GetCurrentDirectory() + "\\Logging\\");
        private readonly string _fileName;

        private static readonly object _lock = new();

        public FileLogger(string fileName)
        {
            _fileName = fileName;

            if (!File.Exists($"{_RootDirectory}{DateTime.Now.ToLongDateString()}_{_fileName}"))
            {
                File.Create($"{_RootDirectory}{DateTime.Now.ToLongDateString()}_{_fileName}").Close();
            }
        }

        void WriteLog(string message, LogLevel logLevel)
        {
            lock (_lock)
            {
                string logMessage = $"[{DateTime.Now}] -- [{logLevel}] -- {message}\n";
                File.AppendAllText($"{_RootDirectory}{DateTime.Now.ToLongDateString()}_{_fileName}", logMessage);
            }

        }

        public void LogInformation(string message)
        {
            WriteLog(message, LogLevel.LogInformation);
        }

        public void LogWarning(string message)
        {
            WriteLog(message, LogLevel.LogWarning);
        }

        public void LogCritical(string message)
        {
            WriteLog(message, LogLevel.LogCritical);
        }

        public void LogError(string message)
        {
            WriteLog(message, LogLevel.LogError);
        }

    }

}