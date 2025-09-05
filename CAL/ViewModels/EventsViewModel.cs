using CAL.Client.Models.Cal;
using CAL.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static CAL.Constants;

namespace CAL.ViewModels;

public class EventsViewModel : BaseViewModel
{
    private int _currentPage = 0;
    private string _searchText;
    private DateTime? _selectedDate;

    private readonly ObservableCollection<Event> _allEvents = new(); // raw data

    public ObservableCollection<Event> Events { get; } = new();       // filtered

    public ICommand SearchCommand { get; }

    public EventsViewModel()
    {
        SearchCommand = new Command(ApplyFilters);

        Task.Run(LoadInitial);
    }
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }
    }

    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (_selectedDate != value)
            {
                _selectedDate = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }
    }
    private async Task LoadInitial()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            var response = await CalClientSingleton.GetEventsAsync();

            if (response.StatusCode != 200)
            {
                return;
            }

            if (response != null && response.Events.Count > 0)
            {
                foreach (var ev in response
                    .Events
                    .OrderByDescending(e => e.StartTime)
                    )
                {
                    _allEvents.Add(ev);
                }
                _currentPage++;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
    private async Task RefreshEvents()
    {
        return; //idc right now
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            _currentPage = 0;
            Events.Clear();
            await LoadInitial();
        }
        finally
        {
            IsBusy = false;
        }
    }
    private void ApplyFilters()
    {
        var filtered = _allEvents.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(e => e.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        if (SelectedDate.HasValue)
            filtered = filtered.Where(e => e.StartTime.Date == SelectedDate.Value.Date);

        Events.Clear();
        foreach (var ev in filtered)
            Events.Add(ev);
    }
    private async void ExecuteEventSelectedCommand(Event e)
    {
        if (e.SeriesId != null)
        {
            var series = (await CalClientSingleton.GetSeriesAsync((Guid)e.SeriesId)).Series;
            var startUnixTimeSeconds = ((DateTimeOffset)series.StartsOn.Add(series.EventStartTime).ToUniversalTime()).ToUnixTimeSeconds();
            var endUnixTimeSeconds = ((DateTimeOffset)series.EndsOn.Add(series.EventEndTime).ToUniversalTime()).ToUnixTimeSeconds();

            var color = SupportedColors.Where(c => Color.Parse(c).ToString() == e.Color.ToString()).Single();

            var yearlyEvent =
                (!series.RepeatOnSun && !series.RepeatOnMon && !series.RepeatOnTues && !series.RepeatOnWed && !series.RepeatOnThurs && !series.RepeatOnFri && !series.RepeatOnSat && series.RepeatEveryWeek == 0);

            await Shell.Current.GoToAsync($@"{nameof(EditSeriesPage)}?{nameof(EditSeriesViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditSeriesViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditSeriesViewModel.Id)}={series.Id}&{nameof(EditSeriesViewModel.Name)}={series.Name}&{nameof(EditSeriesViewModel.Description)}={series.Description}&{nameof(EditSeriesViewModel.RepeatEveryWeek)}={series.RepeatEveryWeek}&{nameof(EditSeriesViewModel.RepeatOnMon)}={series.RepeatOnMon}&{nameof(EditSeriesViewModel.RepeatOnTues)}={series.RepeatOnTues}&{nameof(EditSeriesViewModel.RepeatOnWed)}={series.RepeatOnWed}&{nameof(EditSeriesViewModel.RepeatOnThurs)}={series.RepeatOnThurs}&{nameof(EditSeriesViewModel.RepeatOnFri)}={series.RepeatOnFri}&{nameof(EditSeriesViewModel.RepeatOnSat)}={series.RepeatOnSat}&{nameof(EditSeriesViewModel.RepeatOnSun)}={series.RepeatOnSun}&{nameof(EditSeriesViewModel.EntityType)}={series.EntityType}&{nameof(EditSeriesViewModel.CurrentlySelectedCalendar)}={null}&{nameof(EditSeriesViewModel.Color)}={color}&{nameof(EditSeriesViewModel.ShouldNotify)}={e.ShouldNotify}&{nameof(EditSeriesViewModel.NumTimesNotified)}={e.NumTimesNotified}&{nameof(EditSeriesViewModel.YearlyEvent)}={yearlyEvent}");
        }
        else
        {
            var startUnixTimeSeconds = ((DateTimeOffset)e.StartTime.ToUniversalTime()).ToUnixTimeSeconds();
            var endUnixTimeSeconds = ((DateTimeOffset)e.EndTime.ToUniversalTime()).ToUnixTimeSeconds();

            var color = SupportedColors.Where(c => Color.Parse(c).ToString() == e.Color.ToString()).Single();

            await Shell.Current.GoToAsync($@"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditEventViewModel.Id)}={e.Id}&{nameof(EditEventViewModel.Name)}={e.Name}&{nameof(EditEventViewModel.Description)}={e.Description}&{nameof(EditEventViewModel.EntityType)}={e.EntityType}&{nameof(EditEventViewModel.Color)}={color}&{nameof(EditEventViewModel.ShouldNotify)}={e.ShouldNotify}&{nameof(EditEventViewModel.NumTimesNotified)}={e.NumTimesNotified}");
        }
    }

}
