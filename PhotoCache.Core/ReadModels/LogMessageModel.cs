using System;
using PhotoCache.Core.Logging;

namespace PhotoCache.Core.ReadModels
{
    public class LogMessageModel : IModel, ILogMessage
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }

        public LogMessageModel(string message)
        {
            Message = message;
            TimeStamp = DateTime.Now;
            Id = Guid.NewGuid();
        }
    }
}
