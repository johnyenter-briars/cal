﻿using CAL.Client;
using CAL.Managers;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CAL.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            Title = "Settings";
        }
        private string buildVersion = Assembly.GetExecutingAssembly().GetName().ToString();
        public string BuildVersion { get => buildVersion; set => SetProperty(ref buildVersion, value); }
        private string userId = PreferencesManager.GetUserId();
        public string UserId { get => userId; set => SetProperty(ref userId, value); }
        private string apiKey = PreferencesManager.GetApiKey();
        public string ApiKey { get => apiKey; set => SetProperty(ref apiKey, value); }
        private string hostname = PreferencesManager.GetHostname();
        public string HostName { get => hostname; set => SetProperty(ref hostname, value); }
        private string port = PreferencesManager.GetPort().ToString();
        public string Port { get => port; set => SetProperty(ref port, value); }
        private string defaultCalendarId = PreferencesManager.GetDefaultCalendarId();
        public string DefaultCalendarId { get => defaultCalendarId; set => SetProperty(ref defaultCalendarId, value); }
        private string maxNumTimesNotify = PreferencesManager.GetMaxNumTimesToNotify().ToString();
        public string MaxNumTimesNotify { get => maxNumTimesNotify; set => SetProperty(ref maxNumTimesNotify, value); }
        private string defaultYearsToRepeat = PreferencesManager.GetDefaultYearsToRepeat().ToString();
        public string DefaultYearsToRepeat { get => defaultYearsToRepeat; set => SetProperty(ref defaultYearsToRepeat, value); }
        private Command saveChangesCommand;
        public ICommand SaveChangesCommand
        {
            get
            {
                if (saveChangesCommand == null)
                {
                    saveChangesCommand = new Command(SaveChanges);
                }

                return saveChangesCommand;
            }
        }
        private void SaveChanges()
        {
            if (int.TryParse(port, out int p) && int.TryParse(maxNumTimesNotify, out int n) && int.TryParse(defaultYearsToRepeat, out int dy))
            {
                PreferencesManager.SetSettings(hostname, p, apiKey, userId, defaultCalendarId, n, dy);
                CalClientSingleton.UpdateSettings(PreferencesManager.GetHostname(),
                                            PreferencesManager.GetPort(),
                                            PreferencesManager.GetApiKey(),
                                            PreferencesManager.GetUserId());
            }
        }
    }
}