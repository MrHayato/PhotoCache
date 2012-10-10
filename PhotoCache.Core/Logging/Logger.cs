using System;

namespace PhotoCache.Core.Logging
{
    public class Logger : ILogger
    {
        public void Log(ILogMessage message)
        {
            Console.WriteLine(message.Message);
        }
    }
}
