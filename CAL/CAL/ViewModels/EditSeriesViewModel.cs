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
    public class EditSeriesViewModel : BaseViewModel
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
                //DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
                //SeriesStartsOnSelectedDate = dateTime.ToLocalTime();
                //SubEventsStartTime = dateTime.ToLocalTime().TimeOfDay;
            }
        }
        private long endTimeUnixSeconds;
        public long EndTimeUnixSeconds
        {
            get => endTimeUnixSeconds;
            set
            {
                endTimeUnixSeconds = value;
                //DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(endTimeUnixSeconds);
                //SeriesEndsOnSelectedDate = dateTime.ToLocalTime();
                //SubEventEndTime = dateTime.ToLocalTime().TimeOfDay;
            }
        }
        private TimeSpan _subEventStartTime;
        public TimeSpan SubEventsStartTime
        {
            get => _subEventStartTime; set
            {
                SetProperty(ref _subEventStartTime, value);
            }
        }
        private TimeSpan _subEventEndTime;
        public TimeSpan SubEventEndTime
        {
            get => _subEventEndTime; set
            {
                SetProperty(ref _subEventEndTime, value);
            }
        }
        public DateTime _seriesStartsOnDate;
        public DateTime SeriesStartsOnSelectedDate
        {
            get { return _seriesStartsOnDate; }
            set
            {
                SetProperty(ref _seriesStartsOnDate, value);
            }
        }
        public DateTime _seriesEndsOnDate;
        public DateTime SeriesEndsOnSelectedDate
        {
            get { return _seriesEndsOnDate; }
            set
            {
                SetProperty(ref _seriesEndsOnDate, value);
            }
        }
        public EditSeriesViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }
        private bool ValidateSave()
        {
            //return !string.IsNullOrWhiteSpace(name);
            return true;
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
            //var startingTimeDatePart = new DateTime(SeriesStartsOnSelectedDate.Year, SeriesStartsOnSelectedDate.Month, SeriesStartsOnSelectedDate.Day, 0, 0, 0, kind: DateTimeKind.Local);
            //var startTime = startingTimeDatePart + SubEventsStartTime;

            //var endingTimeDatePart = new DateTime(SeriesEndsOnSelectedDate.Year, SeriesEndsOnSelectedDate.Month, SeriesEndsOnSelectedDate.Day, 0, 0, 0, kind: DateTimeKind.Local);
            //var endTime = endingTimeDatePart + SubEventEndTime;

            //Event newEvent = new Event()
            //{
            //    Id = id,
            //    Name = name,
            //    Description = description,
            //    StartTime = startTime.ToUniversalTime(),
            //    EndTime = endTime.ToUniversalTime(),
            //    CalUserId = new Guid(PreferencesManager.GetUserId()),
            //};

            var request = new CreateSeriesRequest
            {
                Name = "test",
                Description = "please",
                RepeatOnThurs = true,
                RepeatEveryWeek = 2,
                StartsOn = new DateTime(2022, 6, 29, 0, 0, 0, DateTimeKind.Utc),
                EndsOn = new DateTime(2022, 10, 29, 0, 0, 0, DateTimeKind.Utc),
                EventStartTime = new TimeSpan(SubEventsStartTime.Hours, SubEventsStartTime.Minutes, SubEventsStartTime.Seconds)
            };

            await EventDataStore.CreateSeriesAsync(request);

            await Shell.Current.GoToAsync("..");
        }
    }
}
