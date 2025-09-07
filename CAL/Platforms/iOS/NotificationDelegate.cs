using UserNotifications;

namespace CAL;

class NotificationDelegate : UNUserNotificationCenterDelegate
{
    public override void WillPresentNotification(
        UNUserNotificationCenter center,
        UNNotification notification,
        Action<UNNotificationPresentationOptions> completionHandler)
    {
        // Force showing the notification while app is in foreground
        completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
    }
}
