using BackgroundTasks;
using CAL.Client;
using CAL.Managers;
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
        ScheduleProcessingTask();

        UNUserNotificationCenter.Current.RequestAuthorization(
            UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
            (approved, err) =>
            {
                Console.WriteLine(approved
                    ? "Notification approval granted"
                    : "Notification approval denied");
            });

        UNUserNotificationCenter.Current.Delegate = new NotificationDelegate();

        return base.FinishedLaunching(app, options);
    }
    void ScheduleProcessingTask()
    {
        Console.WriteLine("Scheduling background processing task...");

        var request = new BGProcessingTaskRequest("com.jyb.cal.processing")
        {
            RequiresNetworkConnectivity = true,
            RequiresExternalPower = false
        };

        NSError error;
        BGTaskScheduler.Shared.Submit(request, out error);

        if (error != null)
            Console.WriteLine($"Error scheduling background processing: {error}");
        else
            Console.WriteLine("Background processing task scheduled.");
    }

    void RegisterBackgroundTasks()
    {
        Console.WriteLine("Registering background tasks...");

        BGTaskScheduler.Shared.Register("com.jyb.cal.processing", null, task =>
        {
            Console.WriteLine("Background processing task triggered!");
            if (task is BGProcessingTask processingTask)
            {
                _ = HandleProcessingTaskAsync(processingTask);
            }
        });
    }
    async Task HandleProcessingTaskAsync(BGProcessingTask task)
    {
        bool completed = false;

        task.ExpirationHandler = () =>
        {
            Console.WriteLine("Processing task expired!");
            if (!completed)
            {
                task.SetTaskCompleted(success: false);
                completed = true;
            }
        };

        try
        {
            ScheduleProcessingTask(); // reschedule for next time

            await CheckForNotifications();

            Console.WriteLine("Processing task completed and notification sent.");

            if (!completed)
            {
                task.SetTaskCompleted(success: true);
                completed = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Processing task failed: {ex}");
            if (!completed)
            {
                task.SetTaskCompleted(success: false);
                completed = true;
            }
        }
    }
    async Task CheckForNotifications()
    {
        var calClient = DependencyService.Get<ICalClient>();
        var calUserId = new Guid(PreferencesManager.GetUserId());

        var response = await calClient.GetUpcommingEventsAsync(calUserId);

        foreach (var e in response.Events)
        {
            await SendNotification(e.Name, $"Upcomming Event at: {e.StartTime:HH:mm}");

            await calClient.CreateNotificationAsync(new Client.Models.Server.Request.CreateNotificationRequest
            {
                CalUserId = calUserId,
                EventId = e.Id,
            });
        }
    }

    async Task SendNotification(string title, string message, DateTime? notifyTime = null)
    {
        var content = new UNMutableNotificationContent
        {
            Title = title,
            Body = message,
            Sound = UNNotificationSound.Default
        };

        var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);

        var request = UNNotificationRequest.FromIdentifier(Guid.NewGuid().ToString(), content, trigger);

        await UNUserNotificationCenter.Current.AddNotificationRequestAsync(request);
    }
}

