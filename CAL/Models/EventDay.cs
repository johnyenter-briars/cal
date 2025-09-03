using System;
using XCalendar.Core.Collections;
using XCalendar.Core.Interfaces;
using XCalendar.Core.Models;

namespace CAL.Models
{
    public class EventDay : CalendarDay<Event>
    {
        public DateTime DateTime { get; set; }
        //public ObservableRangeCollection<CalendarEvent> Events => Events;
        public bool IsSelected { get; set; }
        public bool IsCurrentMonth { get; set; }
        public bool IsToday { get; set; }
        public bool IsInvalid { get; set; }
    }
}
