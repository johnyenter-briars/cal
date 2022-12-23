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

namespace CAL.Droid.LocalNotifications
{
	[BroadcastReceiver]
	public class AlarmReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			var message = intent.GetStringExtra("message");

			DependencyService.Get<INotificationManager>().SendNotification("test", $"this is a test: {message}");
		}
	}
}
