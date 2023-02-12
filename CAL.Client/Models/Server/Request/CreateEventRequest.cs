using CAL.Client.Interfaces;
using System;

namespace CAL.Client.Models.Server.Request
{
	public class CreateEventRequest : IValidatable
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public Guid CalUserId { get; set; }
		public Guid? SeriesId { get; set; }
		public Guid CalendarId { get; set; }
		public string? Color { get; set; }

		public bool Validate()
		{
			return StartTime.Kind == DateTimeKind.Utc &&
					EndTime.Kind == DateTimeKind.Utc &&
					CalUserId != null;
		}
	}
}
