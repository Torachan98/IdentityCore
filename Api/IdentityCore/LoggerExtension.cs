namespace IdentityCore
{
    public class LoggerExtension
    {
        private readonly ILogger<LoggerExtension> _logger;

        public LoggerExtension(ILogger<LoggerExtension> logger)
        {
            _logger = logger;
        }
    }
}
