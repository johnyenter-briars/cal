using Android.Content;
using CAL.Client;
using CAL.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Platforms.Android
{
	[BroadcastReceiver]
	public class EventNotificationReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			Task.Run(async () =>
			   {
				   var calClient = DependencyService.Get<ICalClient>();

				   var response = await calClient.GetEventsAsync(DateTime.Now.Year, DateTime.Now.Month);

				   var now = DateTime.Now;

				   foreach (var e in response.Events)
				   {
					   if (now.Date != e.StartTime.Date || !e.ShouldNotify || e.NumTimesNotified > PreferencesManager.GetMaxNumTimesToNotify())
					   {
						   continue;
					   }

					   var span = e.StartTime.Subtract(now);

					   //if (span.Minutes >= 30 &&
					   // span.Minutes <= 45)
					   //{
					   // DependencyService.Get<INotificationManager>().SendNotification(e.Name, $"Upcomming Event in 30-50 mintues at: {e.StartTime}");
					   // e.NumTimesNotified += 1;
					   // await calClient.UpdateEventAsync(e.ToUpdateRequest());
					   //}

					   if (span.TotalMinutes >= 16 && span.TotalMinutes <= 30)
					   {
						   DependencyService.Get<INotificationManager>().SendNotification(e.Name, $"Upcomming Event at: {e.StartTime:HH:mm}");
						   e.NumTimesNotified += 1;
						   await calClient.UpdateEventAsync(e.ToUpdateRequest());
						   return;
					   }

					   if (span.TotalMinutes >= 0 && span.TotalMinutes <= 15)
					   {
						   DependencyService.Get<INotificationManager>().SendNotification(e.Name, $"Upcomming Event at: {e.StartTime:HH:mm}");
						   e.NumTimesNotified += 1;
						   await calClient.UpdateEventAsync(e.ToUpdateRequest());
						   return;
					   }
				   }
			   });
		}
	}
}
