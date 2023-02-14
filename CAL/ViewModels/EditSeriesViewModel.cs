using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Client.Models.Server.Request;
using CAL.Managers;
using System;
using System.Text;

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
	[QueryProperty(nameof(EntityType), nameof(EntityType))]
	[QueryProperty(nameof(Color), nameof(Color))]
	[QueryProperty(nameof(NumTimesNotified), nameof(NumTimesNotified))]
	[QueryProperty(nameof(ShouldNotify), nameof(ShouldNotify))]
	public class EditSeriesViewModel : BaseViewModel
	{
		public Color CurrentlySelectedColor
		{
			get
			{
				return Microsoft.Maui.Graphics.Color.Parse(_color);
			}
			set { }
		}
		public Command ColorPickerChangedCommand { get; }
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
		private Guid id;
		private string _color = "red";
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
				if (new Guid(Id) == Guid.Empty)
				{
					SubEventsStartTime = DateTime.Now.TimeOfDay;
				}
				else
				{
					SubEventsStartTime = dateTime.ToLocalTime().TimeOfDay;
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
				SeriesEndsOnSelectedDate = dateTime.ToLocalTime();
				if (new Guid(Id) == Guid.Empty)
				{
					SubEventEndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromHours(1));
				}
				else
				{
					SubEventEndTime = dateTime.ToLocalTime().TimeOfDay;
				}
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
			DeleteCommand = new Command(OnDelete);
			ColorPickerChangedCommand = new Command(OnPickerSelectedIndexChanged);
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
		public string Color
		{
			get => _color;
			set => SetProperty(ref _color, value);
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
		private int numTimesNotified;
		public int NumTimesNotified
		{
			get => numTimesNotified;
			set => SetProperty(ref numTimesNotified, value);
		}
		private bool shouldNotify;
		public bool ShouldNotify
		{
			get => shouldNotify;
			set => SetProperty(ref shouldNotify, value);
		}

		public Command SaveCommand { get; }
		public Command CancelCommand { get; }
		public Command DeleteCommand { get; }

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

			if (id == Guid.Empty)
			{
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
					Color = Color,
					NumTimesNotified = numTimesNotified,
					ShouldNotify = shouldNotify,
				};

				await CalClientSingleton.CreateSeriesAsync(request);
			}
			else
			{
				var request = new UpdateSeriesRequest
				{
					Id = id,
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
					Color = Color,
					NumTimesNotified = numTimesNotified,
					ShouldNotify = shouldNotify,
				};

				await CalClientSingleton.UpdateSeriesAsync(request);
			}

			await Shell.Current.GoToAsync("..");
		}
		private async void OnDelete()
		{
			await CalClientSingleton.DeleteEntityAsync(id, _entityType);
			await Shell.Current.GoToAsync("..");
		}
		private void OnPickerSelectedIndexChanged(object sender)
		{
			var selectedOption = (string)(sender as Picker).SelectedItem;

			CurrentlySelectedColor = Microsoft.Maui.Graphics.Color.Parse(selectedOption);
			Color = selectedOption;
		}
	}
}
