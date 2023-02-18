using System;
using System.Collections.Generic;
using System.Text;

namespace CAL.Managers
{
	internal static class PreferencesManager
	{
		private static readonly string apiKeyTag = "apiKey";
		private static readonly string userIdTag = "userId";
		private static readonly string hostnameTag = "hostname";
		private static readonly string portTag = "portKey";
		private static readonly string defaultCalendarIdTag = "defaultCalendarId";
		private static readonly string maxNumTimesToNotifyTag = "maxNumTimesToNotify";
		public static string GetUserId()
		{
			return Preferences.Get(userIdTag, null);
		}
		public static string GetApiKey()
		{
			return Preferences.Get(apiKeyTag, null);
		}
		public static string GetHostname()
		{
			return Preferences.Get(hostnameTag, null);
		}
		public static int GetPort()
		{
			return Preferences.Get(portTag, 0);
		}
		public static string GetDefaultCalendarId()
		{
			return Preferences.Get(defaultCalendarIdTag, null);
		}
		public static int GetMaxNumTimesToNotify()
		{
			return Preferences.Get(maxNumTimesToNotifyTag, 3);
		}
		public static void SetSettings(string hostname, int port, string apiKey, string userId, string defaultCalendarId, int maxNumTimesToNotify)
		{
			Preferences.Set(userIdTag, userId);
			Preferences.Set(hostnameTag, hostname);
			Preferences.Set(apiKeyTag, apiKey);
			Preferences.Set(portTag, port);
			Preferences.Set(defaultCalendarIdTag, defaultCalendarId);
			Preferences.Set(maxNumTimesToNotifyTag, maxNumTimesToNotify);
		}
	}
}
