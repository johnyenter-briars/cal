using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using CAL.Platforms.Android;

namespace CAL;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
	protected override void OnStart()
	{
		base.OnStart();

		var alarmIntent = new Intent(this, typeof(AlarmReceiver));
		alarmIntent.PutExtra("message", "this came from main act part 2 lez goo");

		var pendingIntent = PendingIntent.GetBroadcast(this, 1, alarmIntent, PendingIntentFlags.Immutable);

		var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();

		alarmManager.SetInexactRepeating(
			AlarmType.ElapsedRealtimeWakeup,
			SystemClock.ElapsedRealtime() + 1 * 1000,
			1000,
			pendingIntent);
	}

}
