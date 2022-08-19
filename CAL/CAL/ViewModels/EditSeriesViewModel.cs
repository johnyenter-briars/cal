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
    [QueryProperty(nameof(Id), nameof(Id))]
    [QueryProperty(nameof(Name), nameof(Name))]
    [QueryProperty(nameof(Description), nameof(Description))]
    [QueryProperty(nameof(RepeatEveryWeek), nameof(RepeatEveryWeek))]
    [QueryProperty(nameof(RepeatOnMon), nameof(RepeatOnMon))]
    [QueryProperty(nameof(RepeatOnTues), nameof(RepeatOnTues))]
    [QueryProperty(nameof(RepeatOnWed), nameof(RepeatOnWed))]
    [QueryProperty(nameof(RepeatOnThurs), nameof(RepeatOnThurs))]
    [QueryProperty(nameof(RepeatOnFri), nameof(RepeatOnFri))]
    [QueryProperty(nameof(RepeatOnSat), nameof(RepeatOnSat))]
    [QueryProperty(nameof(RepeatOnSun), nameof(RepeatOnSun))]
    [QueryProperty(nameof(StartTimeUnixSeconds), nameof(StartTimeUnixSeconds))]
    [QueryProperty(nameof(EndTimeUnixSeconds), nameof(EndTimeUnixSeconds))]
    [QueryProperty(nameof(CurrentlySelectedCalendar), nameof(CurrentlySelectedCalendar))]
    public class EditSeriesViewModel : BaseViewModel
    {
        public string CurrentlySelectedCalendar
        {
            get
            {
                return _currentlySelectedCalendar.ToString();
            }
            set
            {
                _currentlySelectedCalendar = Guid.Parse(value);
            }
        }
        private Guid _currentlySelectedCalendar;
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
                SeriesStartsOnSelectedDate = dateTime.ToLocalTime();
                SubEventsStartTime = dateTime.ToLocalTime().TimeOfDay;
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
                SeriesEndsOnSelectedDate = dateTime.ToLocalTime();
                SubEventEndTime = dateTime.ToLocalTime().TimeOfDay.Add(TimeSpan.FromHours(1));
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
        public bool RepeatOnMon { get; set; }
        public bool RepeatOnTues { get; set; }
        public bool RepeatOnWed { get; set; }
        public bool RepeatOnThurs { get; set; }
        public bool RepeatOnFri { get; set; }
        public bool RepeatOnSat { get; set; }
        public bool RepeatOnSun { get; set; }
        public int RepeatEveryWeek { get; set; }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var startingTimeDatePart = new DateTime(SeriesStartsOnSelectedDate.Year, SeriesStartsOnSelectedDate.Month, SeriesStartsOnSelectedDate.Day, 0, 0, 0, kind: DateTimeKind.Local);
            var startTime = startingTimeDatePart + SubEventsStartTime;

            var endingTimeDatePart = new DateTime(SeriesEndsOnSelectedDate.Year, SeriesEndsOnSelectedDate.Month, SeriesEndsOnSelectedDate.Day, 0, 0, 0, kind: DateTimeKind.Local);
            var endTime = endingTimeDatePart + SubEventEndTime;

            var request = new CreateSeriesRequest
            {
                Name = name,
                Description = description,
                RepeatOnMon = RepeatOnMon,
                RepeatOnTues = RepeatOnTues,
                RepeatOnWed = RepeatOnWed,
                RepeatOnThurs = RepeatOnThurs,
                RepeatOnFri = RepeatOnFri,
                RepeatOnSat = RepeatOnSat,
                RepeatOnSun = RepeatOnSun,
                RepeatEveryWeek = RepeatEveryWeek,
                StartsOn = startingTimeDatePart.ToUniversalTime(),
                EndsOn = endingTimeDatePart.ToUniversalTime(),
                EventStartTime = startTime.TimeOfDay,
                EventEndTime = endTime.TimeOfDay,
                CalUserId = new Guid(PreferencesManager.GetUserId()),
                CalendarId = _currentlySelectedCalendar,
            };

            await EventDataStore.CreateSeriesAsync(request);

            await Shell.Current.GoToAsync("..");
        }
    }
}
