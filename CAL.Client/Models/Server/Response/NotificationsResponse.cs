using CAL.Client.Models.Cal;


namespace CAL.Client.Models.Server.Response;

public class NotificationsResponse : BaseResponse
{
    public List<Notification> Notifications { get; set; } = new();
}

