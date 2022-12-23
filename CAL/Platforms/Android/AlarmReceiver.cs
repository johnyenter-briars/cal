using Android.Content;
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
			DependencyService.Get<INotificationManager>().SendNotification("context", "i'm so sad");
		}
	}
}
