using Android.Content;
using CAL.Client;
using CAL.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                   try
                   {
                       var calClient = DependencyService.Get<ICalClient>();
                       var calUserId = new Guid(PreferencesManager.GetUserId());

                       var response = await calClient.GetUpcommingEventsAsync(calUserId);

                       foreach (var e in response.Events)
                       {
                           DependencyService.Get<INotificationManager>().SendNotification(e.Name, $"Upcomming Event at: {e.StartTime:HH:mm}");

                           await calClient.CreateNotificationAsync(new Client.Models.Server.Request.CreateNotificationRequest
                           {
                               CalUserId = calUserId,
                               EventId = e.Id,
                           });
                       }
                   }
                   catch (Exception ex)
                   {
                       Debug.WriteLine(ex);
                   }
               });
        }
    }
}
