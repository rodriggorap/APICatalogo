using System.Collections.Concurrent;

namespace APICatalogo.Logging
{
    public class CustomLoggerProvider 
    {
        readonly CustomerLoggerProviderConfiguration loggerConfig;

        readonly ConcurrentDictionary<string, CustomerLogger> logger = 
            new ConcurrentDictionary<string, CustomerLogger>();

        public CustomLoggerProvider(CustomerLoggerProviderConfiguration config)
        {
            loggerConfig = config;
        }

        public void Dispose()
        {
            logger.Clear();
        }
    }
}
