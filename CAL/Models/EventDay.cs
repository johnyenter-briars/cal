using System;
using XCalendar.Core.Interfaces;
using XCalendar.Core.Models;

namespace CAL.Models
{
	public class EventDay : BaseObservableModel, ICalendarDay
	{
		public DateTime DateTime { get; set; }
		public ObservableRangeCollection<CalendarEvent> Events { get; } = new ObservableRangeCollection<CalendarEvent>();
		public bool IsSelected { get; set; }
		public bool IsCurrentMonth { get; set; }
		public bool IsToday { get; set; }
		public bool IsInvalid { get; set; }
	}
}
