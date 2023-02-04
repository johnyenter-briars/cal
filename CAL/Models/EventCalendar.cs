using System;
using System.Linq;
using XCalendar.Core.Models;

namespace CAL.Models
{
	public class EventCalendar : Calendar<EventDay>
	{
		#region Properties
		public ObservableRangeCollection<CalendarEvent> Events { get; set; } = new ObservableRangeCollection<CalendarEvent>();
		#endregion

		#region Methods
		public override void UpdateDay(EventDay Day, DateTime NewDateTime)
		{
			base.UpdateDay(Day, NewDateTime);
			Day.Events.ReplaceRange(Events.Where(x => x.StartTime.Date == NewDateTime.Date));
		}
		#endregion
	}
}
