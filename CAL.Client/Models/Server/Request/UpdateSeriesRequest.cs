using CAL.Client.Converters;
using CAL.Client.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Client.Models.Server.Request
{
	public class UpdateSeriesRequest : IValidatable
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = "";
		public string Description { get; set; } = "";
		public int RepeatEveryWeek { get; set; }
		public bool RepeatOnMon { get; set; }
		public bool RepeatOnTues { get; set; }
		public bool RepeatOnWed { get; set; }
		public bool RepeatOnThurs { get; set; }
		public bool RepeatOnFri { get; set; }
		public bool RepeatOnSat { get; set; }
		public bool RepeatOnSun { get; set; }
		public DateTime StartsOn { get; set; }
		public DateTime EndsOn { get; set; }
		[JsonConverter(typeof(TimespanConverter))]
		[JsonProperty(TypeNameHandling = TypeNameHandling.All)]
		public TimeSpan EventStartTime { get; set; }
		[JsonConverter(typeof(TimespanConverter))]
		[JsonProperty(TypeNameHandling = TypeNameHandling.All)]
		public TimeSpan EventEndTime { get; set; }
		public Guid CalUserId { get; set; }
		public Guid CalendarId { get; set; }
		public string Color { get; set; } = "";
		public int NumTimesNotified { get; set; }
		public bool ShouldNotify { get; set; }

		public bool Validate()
		{
			return StartsOn.Kind == DateTimeKind.Utc &&
					EndsOn.Kind == DateTimeKind.Utc &&
					CalUserId != Guid.Empty;
		}

		public CreateSeriesRequest ToCreateSeriesRequest()
		{
			return new CreateSeriesRequest
			{
				Id = Id,
				Name = Name,
				Description = Description,
				RepeatEveryWeek = RepeatEveryWeek,
				RepeatOnMon = RepeatOnMon,
				RepeatOnTues = RepeatOnTues,
				RepeatOnWed = RepeatOnWed,
				RepeatOnThurs = RepeatOnThurs,
				RepeatOnFri = RepeatOnFri,
				RepeatOnSat = RepeatOnSat,
				RepeatOnSun = RepeatOnSun,
				StartsOn = StartsOn,
				EndsOn = EndsOn,
				EventStartTime = EventStartTime,
				EventEndTime = EventEndTime,
				CalUserId = CalUserId,
				CalendarId = CalendarId,
				Color = Color,
				NumTimesNotified = NumTimesNotified,
				ShouldNotify = ShouldNotify,
			};
		}
	}
}
