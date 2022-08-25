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
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(startTimeUnixSeconds);
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
                SubEventEndTime = dateTime.ToLocalTime().TimeOfDay;
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
        private bool repeatOnMon;
        public bool RepeatOnMon 
        {
            get => repeatOnMon;
            set => SetProperty(ref repeatOnMon, value);
        }
        private bool repeatOnTues;
        public bool RepeatOnTues  
        {
            get => repeatOnTues;
            set => SetProperty(ref repeatOnTues, value);
        }
        private bool repeatOnWed;
        public bool RepeatOnWed   
        {
            get => repeatOnWed;
            set => SetProperty(ref repeatOnWed, value);
        }
        private bool repeatOnThurs;
        public bool RepeatOnThurs    
        {
            get => repeatOnThurs;
            set => SetProperty(ref repeatOnThurs, value);
        }
        private bool repeatOnFri;
        public bool RepeatOnFri    
        {
            get => repeatOnFri;
            set => SetProperty(ref repeatOnFri, value);
        }
        private bool repeatOnSat;
        public bool RepeatOnSat
        {
            get => repeatOnSat;
            set => SetProperty(ref repeatOnSat, value);
        }
        private bool repeatOnSun;
        public bool RepeatOnSun
        {
            get => repeatOnSun;
            set => SetProperty(ref repeatOnSun, value);
        }
        private int repeatEveryWeek;
        public int RepeatEveryWeek
        {
            get => repeatEveryWeek;
            set => SetProperty(ref repeatEveryWeek, value);
        }

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
