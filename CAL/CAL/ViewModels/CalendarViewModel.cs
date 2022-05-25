using CAL.Client.Models.Cal;
using CAL.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Models;

namespace CAL.ViewModels
{
    internal class CalendarViewModel : BaseViewModel
    {
        public Command AddEventCommand { get; }
        public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));
        public ObservableCollection<Event> Events { get; }
        public EventCollection EventCollection { get; }
        public DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }
        public CalendarViewModel()
        {
            Title = "Calendar";
            _selectedDate = DateTime.Now;
            //Task.Run(async () => await ExecuteLoadEventsAsync());

            EventCollection = new EventCollection();

            AddEventCommand = new Command(OnAddEvent);

            Events = EventDataStore.GetAsObservable();


            Action<List<Event>> actionOnCollectionChange = (List<Event> events) =>
            {
                try
                {
                    foreach (var e in events)
                    {
                        if (EventCollection.ContainsKey(e.StartTime))
                        {
                            ((List<Event>)EventCollection[e.StartTime]).Add(e);
                        }
                        else
                        {
                            EventCollection.Add(e.StartTime, new List<Event> { e });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    IsBusy = false;
                }
            };

            //EventDataStore..AddOnCollectionChangedAction(actionOnCollectionChange);

            EventDataStore.GetAsObservable().CollectionChanged += CalendarViewModel_CollectionChanged;
        }

        private void CalendarViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                //System.Collections.Specialized.ReadOnlyList x;
                var newItems = e.NewItems;

                foreach (Event ev in newItems)
                {
                    if (EventCollection.ContainsKey(ev.StartTime))
                    {
                        var listOfEvents = ((List<Event>)EventCollection[ev.StartTime]);

                        if (!listOfEvents.Contains(ev))
                        {
                            listOfEvents.Add(ev);
                        }
                    }
                    else
                    {
                        EventCollection.Add(ev.StartTime, new List<Event> { ev });
                    }
                }

            }
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                //your code
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                //your code
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                //your code
            }
        }

        //private async Task ExecuteLoadEventsAsync()
        //{
        //    IsBusy = true;

        //    try
        //    {
        //        Events.Clear();
        //        EventCollection.Clear();
        //        var events = await EventDataStore.GetItemsAsync();
        //        foreach (var e in events)
        //        {
        //            Events.Add(e);
        //            if (EventCollection.ContainsKey(e.StartTime))
        //            {
        //                ((List<Event>)EventCollection[e.StartTime]).Add(e);
        //            }
        //            else
        //            {
        //                EventCollection.Add(e.StartTime, new List<Event> { e });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}
        private async void OnAddEvent(object obj)
        {
            var unixTimeSeconds = ((DateTimeOffset)SelectedDate).ToUnixTimeSeconds();
            await Shell.Current.GoToAsync($"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={unixTimeSeconds}");
        }
        private async Task ExecuteEventSelectedCommand(object item)
        {
            if (item is Event e)
            {
                var startUnixTimeSeconds = ((DateTimeOffset)e.StartTime.ToUniversalTime()).ToUnixTimeSeconds();
                var endUnixTimeSeconds = ((DateTimeOffset)e.EndTime.ToUniversalTime()).ToUnixTimeSeconds();
                await Shell.Current.GoToAsync($@"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditEventViewModel.Id)}={e.Id}&{nameof(EditEventViewModel.Name)}={e.Name}&{nameof(EditEventViewModel.Description)}={e.Description}");
            }
        }
    }
}
