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

            // Do background work here
            await Task.Delay(2000); // Simulate work


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

        var response = await calClient.GetEventsAsync(DateTime.Now.Year, DateTime.Now.Month);

        var now = DateTime.Now;

        foreach (var e in response.Events)
        {
            if (now.Date != e.StartTime.Date || !e.ShouldNotify || e.NumTimesNotified > PreferencesManager.GetMaxNumTimesToNotify())
            {
                continue;
            }

            var span = e.StartTime.Subtract(now);

            //if (span.Minutes >= 30 &&
            // span.Minutes <= 45)
            //{
            // DependencyService.Get<INotificationManager>().SendNotification(e.Name, $"Upcomming Event in 30-50 mintues at: {e.StartTime}");
            // e.NumTimesNotified += 1;
            // await calClient.UpdateEventAsync(e.ToUpdateRequest());
            //}

            if (span.TotalMinutes >= 16 && span.TotalMinutes <= 30)
            {
                await SendNotification(e.Name, $"Upcomming Event at: {e.StartTime:HH:mm}", null);
                e.NumTimesNotified += 1;
                await calClient.UpdateEventAsync(e.ToUpdateRequest());
                return;
            }

            if (span.TotalMinutes >= 0 && span.TotalMinutes <= 15)
            {
                await SendNotification(e.Name, $"Upcomming Event at: {e.StartTime:HH:mm}", null);
                e.NumTimesNotified += 1;
                await calClient.UpdateEventAsync(e.ToUpdateRequest());
                return;
            }
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

