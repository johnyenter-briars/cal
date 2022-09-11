using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content;
using CAL.Droid.LocalNotifications;
using Xamarin.Forms;
using CAL.Droid.Helpers;
using CAL.Droid.Jobs;
using Android.App.Job;
using Android.Net;

namespace CAL.Droid
{
    [Activity(Label = "CAL",
                Icon = "@mipmap/icon",
                Theme = "@style/MainTheme",
                MainLauncher = true,
                ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize,
                LaunchMode = LaunchMode.SingleTop
        )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnStart()
        {
            base.OnStart();

            var jobInfo = this.CreateJobBuilderUsingJobId<NotificationJob>(1)
                                 .SetPeriodic(900000, 900000)
                                 .SetPersisted(true)
                                 .SetRequiresCharging(false)
                                 .SetRequiredNetworkType(NetworkType.Any)
                                 .SetRequiresStorageNotLow(false)
                                 .Build();

            var jobScheduler = (JobScheduler)GetSystemService(JobSchedulerService);
            var scheduleResult = jobScheduler.Schedule(jobInfo);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}