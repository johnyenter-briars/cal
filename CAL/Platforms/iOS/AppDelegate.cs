using BackgroundTasks;
using Foundation;
using UIKit;
using UserNotifications;

namespace CAL;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        RegisterBackgroundTasks();
        ScheduleAppRefresh();

        UNUserNotificationCenter.Current.RequestAuthorization(
            UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
            (approved, err) =>
            {
                Console.WriteLine(approved
                    ? "Notification approval granted"
                    : "Notification approval denied");
            });

        return base.FinishedLaunching(app, options);
    }

    void ScheduleAppRefresh()
    {
        var request = new BGAppRefreshTaskRequest("com.jyb.cal.refresh")
        {
            EarliestBeginDate = NSDate.FromTimeIntervalSinceNow(15 * 60) // 15 min
        };

        NSError error;
        BGTaskScheduler.Shared.Submit(request, out error);

        if (error != null)
            Console.WriteLine($"Error scheduling refresh: {error}");
    }

    void RegisterBackgroundTasks()
    {
        BGTaskScheduler.Shared.Register("com.jyb.cal.refresh", null, task =>
        {
            if (task is BGAppRefreshTask refreshTask)
            {
                _ = HandleAppRefreshAsync(refreshTask);
            }
        });
    }

    async Task HandleAppRefreshAsync(BGAppRefreshTask task)
    {
        var completed = false;

        task.ExpirationHandler = () =>
        {
            if (!completed)
            {
                task.SetTaskCompleted(success: false);
                completed = true;
            }
        };

        try
        {
            ScheduleAppRefresh();

            // call server and show notification(s)
            await Task.Delay(2000);
            var content = new UNMutableNotificationContent
            {
                Title = "foo bar",
                Body = "bing bong",
                Sound = UNNotificationSound.Default
            };

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
            var request = UNNotificationRequest.FromIdentifier(Guid.NewGuid().ToString(), content, trigger);

            await UNUserNotificationCenter.Current.AddNotificationRequestAsync(request);

            if (!completed)
            {
                task.SetTaskCompleted(success: true);
                completed = true;
            }
        }
        catch (Exception ex)
        {
            if (!completed)
            {
                task.SetTaskCompleted(success: false);
                completed = true;
            }
        }
    }
}

