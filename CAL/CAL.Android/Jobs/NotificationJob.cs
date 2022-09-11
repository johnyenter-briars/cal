using Android.App;
using Android.App.Job;
using Android.Content.PM;
using CAL.Client;
using CAL.Managers;
using CAL.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CAL.Droid.Jobs
{
    [Service(Name = "com.jyb.cal.NotificationJob",
         Permission = "android.permission.BIND_JOB_SERVICE"

        )]
    public class NotificationJob : JobService
    {
        public override bool OnStartJob(JobParameters jobParams)
        {
            Task.Run(async () =>
            {
                var calClient = DependencyService.Get<ICalClient>();

                var response = await calClient.GetEventsAsync();

                var now = DateTime.UtcNow;

                foreach (var e in response.Events)
                {
                    if (now.Date != e.StartTime.Date)
                    {
                        continue;
                    }

                    var span = e.StartTime.ToUniversalTime().Subtract(now);

                    if (span.Minutes >= 30 &&
                        span.Minutes <= 45)
                    {
                        DependencyService.Get<INotificationManager>().SendNotification(e.Name, $"Upcomming Event in 30-50 mintues at: {e.StartTime}");
                    }

                    if (span.Minutes >= 5 &&
                        span.Minutes <= 20)
                    {
                        DependencyService.Get<INotificationManager>().SendNotification(e.Name, $"Upcomming Event in 5-20 mintues at: {e.StartTime}");
                    }

                    if (span.Minutes <= 5 &&
                        span.Minutes > 0)
                    {
                        DependencyService.Get<INotificationManager>().SendNotification(e.Name, $"Upcomming Event starting now at: {e.StartTime}");
                    }
                }

                JobFinished(jobParams, false);
            });

            return true;
        }

        public override bool OnStopJob(JobParameters jobParams)
        {
            return false;
        }
    }
}
