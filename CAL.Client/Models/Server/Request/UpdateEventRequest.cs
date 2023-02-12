using CAL.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Client.Models.Server.Request
{
	public class UpdateEventRequest : IValidatable
	{
		public Guid Id { get; set; }
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
