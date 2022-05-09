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
    public class NewEventViewModel : BaseViewModel
    {
        private string text;
        private string description;
        private long startTimeUnixSeconds;
        public DateTime CurrentDate = DateTime.Now;
        public long StartTimeUnixSeconds
        {
            get => startTimeUnixSeconds;
            set
            {
                startTimeUnixSeconds = value;
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                StartSelectedDate = dateTime.AddSeconds(startTimeUnixSeconds).ToLocalTime();
                StartSelectedTime = dateTime.TimeOfDay;
                EndSelectedDate = StartSelectedDate;
                EndSelectedTime = StartSelectedTime + TimeSpan.FromHours(1);
            }
        }
        private long endTimeUnixSeconds;
        //public long EndTimeUnixSeconds
        //{
        //    get => endTimeUnixSeconds;
        //    set
        //    {
        //        endTimeUnixSeconds = value;
        //        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //        EndSelectedDate = dateTime.AddSeconds(endTimeUnixSeconds).ToLocalTime();
        //        EndSelectedTime = dateTime.TimeOfDay;
        //    }
        //}
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
        public NewEventViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            //StartSelectedDate = DateTime.Now;
            //StartSelectedTime = DateTime.Now.TimeOfDay;
            //EndSelectedTime = StartSelectedTime + TimeSpan.FromHours(1);
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
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var startingTimeDatePart = new DateTime(StartSelectedDate.Year, StartSelectedDate.Month, StartSelectedDate.Day, 0, 0, 0);
            var startTime = startingTimeDatePart + StartSelectedTime;

            var endingTimeDatePart = new DateTime(EndSelectedDate.Year, EndSelectedDate.Month, EndSelectedDate.Day, 0, 0, 0);
            var endTime = endingTimeDatePart + EndSelectedTime;

            Event newItem = new Event()
            {
                Name = text,
                Description = description,
                StartTime = startTime.ToUniversalTime(),
                EndTime = endTime.ToUniversalTime(),
                CalUserId = new Guid(PreferencesManager.GetUserId()),
            };

            await EventDataStore.AddItemAsync(newItem);

            await Shell.Current.GoToAsync("..");
        }
    }
}
