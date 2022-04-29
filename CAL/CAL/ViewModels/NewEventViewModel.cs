using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Client.Models.Cal.Request;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CAL.ViewModels
{
    [QueryProperty(nameof(UnixTimeSeconds), nameof(UnixTimeSeconds))]
    public class NewEventViewModel : BaseViewModel
    {
        private string text;
        private string description;
        private long unixTimeSeconds;
        public long UnixTimeSeconds
        {
            get => unixTimeSeconds;
            set
            {
                unixTimeSeconds = value;
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                SelectedDate = dateTime.AddSeconds(unixTimeSeconds).ToLocalTime();
                SelectedTime = dateTime.TimeOfDay;
            }
        }
        private TimeSpan _time;
        public TimeSpan SelectedTime
        {
            get => _time; set
            {
                SetProperty(ref _time, value);
            }
        }
        public DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                SetProperty(ref _selectedDate, value);
            }
        }
        public NewEventViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }
        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(text);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var startingTimeDatePart = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, 0, 0, 0);
            var startTime = startingTimeDatePart + SelectedTime;

            Event newItem = new Event()
            {
                Name = text,
                Description = description,
                StartTime = startTime.ToUniversalTime(),
                EndTime = DateTime.UtcNow,
                CalUserId = new Guid("a188e597-29f9-4e2f-aa46-e3713d9939da"),
            };

            await EventDataStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
