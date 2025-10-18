using CAL.Client.Interfaces;

namespace CAL.Client.Models.Server.Request;

public class CreateNotificationRequest : IValidatable
{
    public Guid EventId { get; set; }
	public Guid CalUserId { get; set; }

    public bool Validate()
    {
        return true;
    }
}
