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

                if (approved)
                {
                    SendLaunchNotification();
                }
            });

        UNUserNotificationCenter.Current.Delegate = new NotificationDelegate();

        return base.FinishedLaunching(app, options);
    }

    void ScheduleAppRefresh()
    {
        var request = new BGAppRefreshTaskRequest("com.jyb.cal.refresh")
        {
            EarliestBeginDate = NSDate.FromTimeIntervalSinceNow(15 * 60)  //Man I hope this works : (
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

    void SendLaunchNotification()
    {
        var content = new UNMutableNotificationContent
        {
            Title = "App Launched",
            Body = "Your app just launched 🚀",
            Sound = UNNotificationSound.Default
        };

        // Show notification after 1 second
        var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(15, false);
        var request = UNNotificationRequest.FromIdentifier(Guid.NewGuid().ToString(), content, trigger);

        UNUserNotificationCenter.Current.AddNotificationRequestAsync(request)
            .ContinueWith(task =>
            {
                var foo = 10;
                if (task.Exception != null)
                {
                    Console.WriteLine($"Failed to schedule notification: {task.Exception}");
                }
                else
                {
                    Console.WriteLine("Launch notification scheduled");
                }
            });
    }
}

