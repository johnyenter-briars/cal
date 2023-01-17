using Android.Content;
using CAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Platforms.Android
{
	[BroadcastReceiver]
	public class AlarmReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			//Task.Run(async () =>
			//   {
			//	   var calClient = DependencyService.Get<ICalClient>();

			//	   var response = await calClient.GetEventsAsync();

			//	   var now = DateTime.UtcNow;

			//	   foreach (var e in response.Events)
			//	   {
			//		   if (now.Date != e.StartTime.Date)
			//		   {
			//			   continue;
			//		   }

			//		   var span = e.StartTime.ToUniversalTime().Subtract(now);

			//		   if (span.Minutes >= 30 &&
			//			   span.Minutes <= 45)
			//		   {
			//			   DependencyService.Get<INotificationManager>().SendNotification(e.Name, $"Upcomming Event in 30-50 mintues at: {e.StartTime}");
			//		   }

			//		   if (span.Minutes <= 5 &&
			//			   span.Minutes > 0)
			//		   {
			//			   DependencyService.Get<INotificationManager>().SendNotification(e.Name, $"Upcomming Event starting now at: {e.StartTime}");
			//		   }
			//	   }
			//   });
		}
	}
}
