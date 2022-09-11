using Android.App;
using Android.App.Job;
using Android.Content.PM;
using CAL.Models;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CAL.Droid.Jobs
{
    [Service(Name = "com.jyb.cal.NotificationJob",
         Permission = "android.permission.BIND_JOB_SERVICE")]
    public class NotificationJob : JobService
    {
        public override bool OnStartJob(JobParameters jobParams)
        {
            Task.Run(() =>
            {
                DependencyService.Get<INotificationManager>().SendNotification("idk", "idk");

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
