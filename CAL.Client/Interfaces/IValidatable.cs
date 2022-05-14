using System;

namespace CAL.Client.Interfaces
{
    public interface IValidatable
    {
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        Guid CalUserId { get; set; }
    }
}