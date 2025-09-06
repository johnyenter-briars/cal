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
        ScheduleProcessingTask();

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
    void ScheduleProcessingTask()
    {
        Console.WriteLine("Scheduling background processing task...");

        var request = new BGProcessingTaskRequest("com.jyb.cal.processing")
        {
            RequiresNetworkConnectivity = true, // set true if you hit server
            RequiresExternalPower = false        // set true if you want it only when plugged in
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

            var content = new UNMutableNotificationContent
            {
                Title = "Background Processing",
                Body = "Task ran successfully in background 🛠️",
                Sound = UNNotificationSound.Default
            };

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
            var request = UNNotificationRequest.FromIdentifier(Guid.NewGuid().ToString(), content, trigger);
            await UNUserNotificationCenter.Current.AddNotificationRequestAsync(request);

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

    void SendLaunchNotification()
    {
        var content = new UNMutableNotificationContent
        {
            Title = "App Launched",
            Body = "Your app just launched -test 🚀",
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

