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
using System.Linq;
using AndroidX.Work;
//using Xamarin.Android.Arch.Work.Runtime;
using System.Threading;

namespace CAL.Droid
{
	public class CalculatorWorker : Worker
	{
		public CalculatorWorker(Context context, WorkerParameters workerParameters) : base(context, workerParameters)
		{

		}
		public override Result DoWork()
		{
			var taxReturn = CalculateTaxes();
			Android.Util.Log.Debug("CalculatorWorker", $"Your Tax Return is: {taxReturn}");
			return Result.InvokeSuccess();
		}

		public double CalculateTaxes()
		{
			return 2000;
		}
	}
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

			Context context = Android.App.Application.Context;

			var alarmIntent = new Intent(this, typeof(AlarmReceiver));
			alarmIntent.PutExtra("message", "this came from main act");

			var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.Immutable);

			//alarmManager.SetAndAllowWhileIdle(
			//	AlarmType.ElapsedRealtimeWakeup,
			//	SystemClock.ElapsedRealtime() + intervalInMinutes, 
			//	pendingIntent);

			//var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
			//alarmManager.SetInexactRepeating(
			//	AlarmType.ElapsedRealtimeWakeup,
			//	SystemClock.ElapsedRealtime() + 1 * 1000,
			//	100,
			//	pending);
			PeriodicWorkRequest taxWorkRequest = PeriodicWorkRequest.Builder.From<CalculatorWorker>(TimeSpan.FromMinutes(20)).Build();
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