using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Client.Models.Cal.Request;
using CAL.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CAL.ViewModels
{
    [QueryProperty(nameof(StartTimeUnixSeconds), nameof(StartTimeUnixSeconds))]
    [QueryProperty(nameof(EndTimeUnixSeconds), nameof(EndTimeUnixSeconds))]
    [QueryProperty(nameof(Id), nameof(Id))]
    [QueryProperty(nameof(Name), nameof(Name))]
    [QueryProperty(nameof(Description), nameof(Description))]
    public class EditEventViewModel : BaseViewModel
    {
        private string name;
        private string description;
        private Guid id;
        private long startTimeUnixSeconds;
        public DateTime CurrentDate = DateTime.Now;
        public long StartTimeUnixSeconds
        {
            get => startTimeUnixSeconds;
            set
            {
                startTimeUnixSeconds = value;
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
                StartSelectedDate = dateTime.ToLocalTime();
                StartSelectedTime = dateTime.ToLocalTime().TimeOfDay;
            }
        }
        private long endTimeUnixSeconds;
        public long EndTimeUnixSeconds
        {
            get => endTimeUnixSeconds;
            set
            {
                endTimeUnixSeconds = value;
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(endTimeUnixSeconds);
                EndSelectedDate = dateTime.ToLocalTime();
                EndSelectedTime = dateTime.ToLocalTime().TimeOfDay;
            }
        }
        private TimeSpan _startTime;
        public TimeSpan StartSelectedTime
        {
            get => _startTime; set
            {
                SetProperty(ref _startTime, value);
            }
        }
        private TimeSpan _endTime;
        public TimeSpan EndSelectedTime
        {
            get => _endTime; set
            {
                SetProperty(ref _endTime, value);
            }
        }
        public DateTime _startDate;
        public DateTime StartSelectedDate
        {
            get { return _startDate; }
            set
            {
                SetProperty(ref _startDate, value);
            }
        }
        public DateTime _endDate;
        public DateTime EndSelectedDate
        {
            get { return _endDate; }
            set
            {
                SetProperty(ref _endDate, value);
            }
        }
        public EditEventViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }
        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name);
        }
        public string Id
        {
            get => id.ToString();
            set => SetProperty(ref id, new Guid(value));
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
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
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var startingTimeDatePart = new DateTime(StartSelectedDate.Year, StartSelectedDate.Month, StartSelectedDate.Day, 0, 0, 0, kind: DateTimeKind.Local);
            var startTime = startingTimeDatePart + StartSelectedTime;

            var endingTimeDatePart = new DateTime(EndSelectedDate.Year, EndSelectedDate.Month, EndSelectedDate.Day, 0, 0, 0);
            var endTime = endingTimeDatePart + EndSelectedTime;

            Event newEvent = new Event()
            {
                Id = id,
                Name = name,
                Description = description,
                StartTime = startTime.ToUniversalTime(),
                EndTime = endTime.ToUniversalTime(),
                CalUserId = new Guid(PreferencesManager.GetUserId()),
            };

            if (id != null)
            {
                await EventDataStore.UpdateItemAsync(newEvent);
            }
            else
            {
                await EventDataStore.AddItemAsync(newEvent);
            }


            await Shell.Current.GoToAsync("..");
        }
    }
}
