using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CAL.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
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

namespace CAL.Droid.LocalNotifications
{
	[BroadcastReceiver]
	public class SampleBootReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			if (intent.Action == "android.intent.action.BOOT_COMPLETED" || intent.Action == "android.intent.action.SCREEN_ON")
			{
				var alarmManager = context.GetSystemService(Context.AlarmService) as AlarmManager;
				var alarmIntent = new Intent(context, typeof(AlarmReceiver));
				alarmIntent.PutExtra("message", "this came from boot");
				var pending = PendingIntent.GetBroadcast(context, 0, alarmIntent, PendingIntentFlags.Immutable);
				alarmManager.SetInexactRepeating(
					AlarmType.ElapsedRealtimeWakeup,
					SystemClock.ElapsedRealtime() + 1 * 1000,
					100,
					pending);

			}
		}
	}
}
