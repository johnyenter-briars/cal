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
        private int eventId;
        private string name;
        private DateTime time;
        public int Id { get; set; }

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

        public int EventId
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

        public async void LoadEventId(int eventId)
        {
            try
            {
                var item = await DataStore.GetEventAsync(eventId);
                Id = item.Id;
                Name = item.Name;
                Time = item.Time;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Event");
            }
        }
    }
}
