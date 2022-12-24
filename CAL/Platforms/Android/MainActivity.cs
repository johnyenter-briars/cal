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
		var id = int.Parse(GetString(Resource.String.notification_alarm_id));
		var interval = int.Parse(GetString(Resource.String.notification_interval_millis));

		var pendingIntent = PendingIntent.GetBroadcast(this, id, alarmIntent, PendingIntentFlags.Immutable);
		var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
		alarmManager.SetInexactRepeating(
			AlarmType.ElapsedRealtimeWakeup,
			SystemClock.ElapsedRealtime() + 5000, //virtually now
			interval,
			pendingIntent);
	}
}
