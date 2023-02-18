using Android.App;
using Android.Content;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Platforms.Android
{
	public class BootAlarmReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			if (intent.Action == "android.intent.action.BOOT_COMPLETED" || intent.Action == "android.intent.action.SCREEN_ON")
			{
				var alarmManager = context.GetSystemService(Context.AlarmService) as AlarmManager;
				var alarmIntent = new Intent(context, typeof(EventNotificationReceiver));
				var id = int.Parse(context.GetString(Resource.String.notification_alarm_id));
				var interval = int.Parse(context.GetString(Resource.String.notification_interval_millis));
				var pendingIntent = PendingIntent.GetBroadcast(context, id, alarmIntent, PendingIntentFlags.Immutable);
				alarmManager.SetInexactRepeating(
					AlarmType.ElapsedRealtimeWakeup,
					SystemClock.ElapsedRealtime() + 1000, //virtually now
					interval,
					pendingIntent);
			}
		}
	}

}
