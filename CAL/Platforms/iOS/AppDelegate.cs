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
        Console.WriteLine("App finished launching");

        // Request notification permissions
        UNUserNotificationCenter.Current.RequestAuthorization(
            UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
            (approved, err) =>
            {
                Console.WriteLine(approved ? "Notification approval granted" : "Notification approval denied");
            });

        UNUserNotificationCenter.Current.Delegate = new NotificationDelegate();

        // Register and schedule background tasks
        RegisterBackgroundTasks();
        ScheduleProcessingTask();

        return base.FinishedLaunching(app, options);
    }

    void RegisterBackgroundTasks()
    {
        Console.WriteLine("Registering background task...");

        BGTaskScheduler.Shared.Register("com.jyb.cal.processing", null, task =>
        {
            Console.WriteLine("Background task triggered!");
            if (task is BGProcessingTask processingTask)
            {
                _ = HandleProcessingTaskAsync(processingTask);
            }
        });
    }

    void ScheduleProcessingTask()
    {
        Console.WriteLine("Scheduling background processing task...");

        var request = new BGProcessingTaskRequest("com.jyb.cal.processing")
        {
            RequiresNetworkConnectivity = true,
            RequiresExternalPower = false,
            EarliestBeginDate = NSDate.FromTimeIntervalSinceNow(15 * 60) // hint: run after 15 min
        };

        NSError error;
        BGTaskScheduler.Shared.Submit(request, out error);
        if (error != null)
        {
            Console.WriteLine($"Error scheduling background task: {error}");
        }
        else
        {
            Console.WriteLine("Background processing task scheduled successfully.");
        }
    }

    async Task HandleProcessingTaskAsync(BGProcessingTask task)
    {
        bool completed = false;

        // Expiration handler: called if iOS kills the task early
        task.ExpirationHandler = () =>
        {
            Console.WriteLine("Background task expired!");
            if (!completed)
            {
                task.SetTaskCompleted(false);
                completed = true;
            }
        };

        try
        {
            Console.WriteLine("Running background task...");

            // Reschedule for next time
            ScheduleProcessingTask();

            // Check for upcoming events
            await CheckForNotifications();

            Console.WriteLine("Background task completed successfully.");

            if (!completed)
            {
                task.SetTaskCompleted(true);
                completed = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Background task failed: {ex}");
            if (!completed)
            {
                task.SetTaskCompleted(false);
                completed = true;
            }
        }
    }

    async Task CheckForNotifications()
    {
        Console.WriteLine("Checking for upcoming events...");

        var calClient = DependencyService.Get<ICalClient>();
        var calUserId = new Guid(PreferencesManager.GetUserId());

        var response = await calClient.GetUpcommingEventsAsync(calUserId);

        if (response.Events.Count == 0)
        {
            Console.WriteLine("No upcoming events found.");
            return;
        }

        foreach (var e in response.Events)
        {
            Console.WriteLine($"Scheduling notification for event: {e.Name} at {e.StartTime}");

            await SendNotification(
                title: "Upcoming Event",
                message: $"{e.Name} at {e.StartTime:HH:mm}"
            );

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

        // Trigger immediately for testing
        var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);

        var request = UNNotificationRequest.FromIdentifier(Guid.NewGuid().ToString(), content, trigger);

        await UNUserNotificationCenter.Current.AddNotificationRequestAsync(request);
        Console.WriteLine($"Notification sent: {title} - {message}");
    }

    // Optional: add a custom UNUserNotificationCenterDelegate to handle foreground notifications
    class NotificationDelegate : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Show notifications even when app is foreground
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
        }
    }
}

