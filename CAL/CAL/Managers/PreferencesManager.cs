using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace CAL.Managers
{
    internal static class PreferencesManager
    {
        private static readonly string apiKeyTag = "apiKey";
        private static readonly string hostnameTag = "hostname";
        private static readonly string portTag = "portKey";
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
        public static void SetSettings(string hostname, int port, string apiKey)
        {
            Preferences.Set(hostnameTag, hostname);
            Preferences.Set(apiKeyTag, apiKey);
            Preferences.Set(portTag, port);
        }
    }
}
