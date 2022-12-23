using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAL
{
	internal interface INotificationManager
	{
		event EventHandler NotificationReceived;
		void Initialize();
		void SendNotification(string title, string message, DateTime? notifyTime = null);
		void ReceiveNotification(string title, string message);
	}
}
