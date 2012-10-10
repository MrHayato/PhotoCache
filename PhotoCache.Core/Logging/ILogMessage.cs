using System;

namespace PhotoCache.Core.Logging
{
    public interface ILogMessage
    {
        string Message { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
