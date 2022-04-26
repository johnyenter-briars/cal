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
        private string description;
        private DateTime startTime;
        private DateTime endTime;
        public Guid Id { get; set; }

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

        public DateTime StartTime
        {
            get => startTime;
            set => SetProperty(ref startTime, value);
        }
        public DateTime EndTime
        {
            get => endTime;
            set => SetProperty(ref endTime, value);
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
                var e = await EventDataStore.GetEventAsync(new Guid(eventId));
                Id = e.Id;
                Name = e.Name;
                StartTime = e.StartTime;
                EndTime = e.EndTime;
                Description = e.Description;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Event");
            }
        }
    }
}
