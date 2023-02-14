using XCalendar.Core.Models;
using CAL.Models;
using CAL.Client.Models.Cal;

namespace CAL.Models
{
	public class CalendarEvent : BaseObservableModel
	{
		public Guid Id { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Guid CalUserId { get; set; }
		public Guid? SeriesId { get; set; }
		public Guid CalendarId { get; set; }
		public Color Color { get; set; }
		public EntityType EntityType => EntityType.Event;
		public override string ToString() => $"{Name}-LOCAL START: {StartTime}- UTC START: {StartTime.ToUniversalTime()}-{EndTime}";
		public string SeriesName { get; set; }
		public bool IsPartOfSeries => SeriesName != null;
		public int NumTimesNotified { get; set; }
		public bool ShouldNotify { get; set; }
	}
}
