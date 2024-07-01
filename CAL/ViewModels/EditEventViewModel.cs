using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Managers;
using Microsoft.Maui.Graphics.Text;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAL.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    [QueryProperty(nameof(StartTimeUnixSeconds), nameof(StartTimeUnixSeconds))]
    [QueryProperty(nameof(EndTimeUnixSeconds), nameof(EndTimeUnixSeconds))]
    [QueryProperty(nameof(Name), nameof(Name))]
    [QueryProperty(nameof(Description), nameof(Description))]
    [QueryProperty(nameof(CurrentlySelectedCalendar), nameof(CurrentlySelectedCalendar))]
    [QueryProperty(nameof(EntityType), nameof(EntityType))]
    [QueryProperty(nameof(Color), nameof(Color))]
    [QueryProperty(nameof(NumTimesNotified), nameof(NumTimesNotified))]
    [QueryProperty(nameof(ShouldNotify), nameof(ShouldNotify))]
    [SuppressPropertyChangedWarnings]
    public class EditEventViewModel : BaseViewModel
    {
        public string Id
        {
            get => id.ToString();
            set => SetProperty(ref id, new Guid(value));
        }
        public Color CurrentlySelectedColor
        {
            get
            {
                return Microsoft.Maui.Graphics.Color.Parse(_color);
            }
            set { }
        }

        private EntityType _entityType;
        public string EntityType
        {
            get => _entityType.ToString(); set
            {
                Enum.TryParse(value, out EntityType entityType);
                _entityType = entityType;
            }
        }
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
        private string _color = "red";
        private Guid id;
        private long startTimeUnixSeconds;
        public long StartTimeUnixSeconds
        {
            get => startTimeUnixSeconds;
            set
            {
                startTimeUnixSeconds = value;
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
                StartSelectedDate = dateTime.ToLocalTime();
                if (new Guid(Id) == Guid.Empty)
                {
                    StartSelectedTime = DateTime.Now.TimeOfDay;
                }
                else
                {
                    StartSelectedTime = dateTime.ToLocalTime().TimeOfDay;
                }
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
                if (new Guid(Id) == Guid.Empty)
                {
                    EndSelectedTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromHours(1));
                }
                else
                {
                    EndSelectedTime = dateTime.ToLocalTime().TimeOfDay;
                }
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
            DeleteCommand = new Command(OnDelete);
            ColorPickerChangedCommand = new Command(OnPickerSelectedIndexChanged);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }
        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name);
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
        public string Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }
        private int numTimesNotified;
        public int NumTimesNotified
        {
            get => numTimesNotified;
            set => SetProperty(ref numTimesNotified, value);
        }
        private bool shouldNotify = true;
        public bool ShouldNotify
        {
            get => shouldNotify;
            set => SetProperty(ref shouldNotify, value);
        }

        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }
        public Command CancelCommand { get; }
        public Command ColorPickerChangedCommand { get; }
        private void OnPickerSelectedIndexChanged(object sender)
        {
            var selectedOption = (string)(sender as Picker).SelectedItem;
            CurrentlySelectedColor = Microsoft.Maui.Graphics.Color.Parse(selectedOption);
            Color = selectedOption;
        }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }
        private async void OnDelete()
        {
            await CalClientSingleton.DeleteEntityAsync(id, _entityType);
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var startingTimeDatePart = new DateTime(StartSelectedDate.Year, StartSelectedDate.Month, StartSelectedDate.Day, 0, 0, 0, kind: DateTimeKind.Local);
            var startTime = startingTimeDatePart + StartSelectedTime;

            var endingTimeDatePart = new DateTime(EndSelectedDate.Year, EndSelectedDate.Month, EndSelectedDate.Day, 0, 0, 0, kind: DateTimeKind.Local);
            var endTime = endingTimeDatePart + EndSelectedTime;

            Event newEvent = new Event()
            {
                Id = id,
                Name = name,
                Description = description,
                StartTime = startTime.ToUniversalTime(),
                EndTime = endTime.ToUniversalTime(),
                CalUserId = new Guid(PreferencesManager.GetUserId()),
                CalendarId = _currentlySelectedCalendar,
                Color = Color,
                NumTimesNotified = numTimesNotified,
                ShouldNotify = shouldNotify,
            };

            if (id != Guid.Empty)
            {
                await CalClientSingleton.UpdateEventAsync(newEvent.ToUpdateRequest());
            }
            else
            {
                await CalClientSingleton.CreateEventAsync(newEvent.ToRequest());
            }

            await Shell.Current.GoToAsync("..");
        }
    }
}
