using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace CAL.ViewModels
{
    [QueryProperty(nameof(EventId), nameof(EventId))]
    public class EventDetailViewModel : BaseViewModel
    {
        private string eventId;
        private string name;
        private DateTime time;
        public Guid Id { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public DateTime Time
        {
            get => time;
            set => SetProperty(ref time, value);
        }

        public string EventId
        {
            get
            {
                return eventId;
            }
            set
            {
                eventId = value;
                LoadEventId(value);
            }
        }

        public async void LoadEventId(string eventId)
        {
            try
            {
                var item = await EventDataStore.GetEventAsync(new Guid(eventId));
                Id = item.Id;
                Name = item.Name;
                Time = item.StartTime;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Event");
            }
        }
    }
}
