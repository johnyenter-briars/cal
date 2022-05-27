using CAL.Client;
using CAL.Managers;
using System;
using System.ComponentModel;
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
        private string userId = PreferencesManager.GetUserId();
        public string UserId { get => userId; set => SetProperty(ref userId, value); }
        private string apiKey = PreferencesManager.GetApiKey();
        public string ApiKey { get => apiKey; set => SetProperty(ref apiKey, value); }
        private string hostname = PreferencesManager.GetHostname();
        public string HostName { get => hostname; set => SetProperty(ref hostname, value); }
        private string port = PreferencesManager.GetPort().ToString();
        public string Port { get => port; set => SetProperty(ref port, value); }
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
            if (int.TryParse(port, out int p))
            {
                PreferencesManager.SetSettings(hostname, p, apiKey, userId);
                EventDataStore.UpdateAuthentication();
            }
        }
    }
}