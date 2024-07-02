using CAL.Client.Models.Server.Request;
using Newtonsoft.Json;
using System;

namespace CAL.Client.Models.Cal
{
    public class Event
    {
        public Guid Id { get; set; }
        private DateTime startTime;
        public DateTime StartTime
        {
            get
            {
                if (startTime.Kind != DateTimeKind.Local)
                {
                    throw new InvalidOperationException("Start time is not Local kind!");
                }
                return startTime;
            }
            set
            {
                if (value.Kind != DateTimeKind.Utc)
                {
                    throw new InvalidOperationException("Events must be initialized with UTC Time");
                }

                startTime = value.ToLocalTime();
            }
        }
        private DateTime endTime;
        public DateTime EndTime
        {
            get
            {
                if (endTime.Kind != DateTimeKind.Local)
                {
                    throw new InvalidOperationException("End time is not Local kind!");
                }
                return endTime;
            }
            set
            {
                if (value.Kind != DateTimeKind.Utc)
                {
                    throw new InvalidOperationException("Events must be initialized with UTC Time");
                }

                endTime = value.ToLocalTime();
            }
        }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public Guid CalUserId { get; set; }
        public Guid? SeriesId { get; set; }
        public Guid CalendarId { get; set; }
        public int NumTimesNotified { get; set; }
        public bool ShouldNotify { get; set; }
        public string Color { get; set; } = "";
        [JsonIgnore]
        public EntityType EntityType => EntityType.Event;
        public CreateEventRequest ToRequest()
        {
            return new CreateEventRequest
            {
                StartTime = StartTime.ToUniversalTime(),
                Description = Description,
                EndTime = EndTime.ToUniversalTime(),
                CalUserId = CalUserId,
                SeriesId = SeriesId,
                Name = Name,
                CalendarId = CalendarId,
                Color = Color,
                NumTimesNotified = NumTimesNotified,
                ShouldNotify = ShouldNotify,
            };
        }
        public UpdateEventRequest ToUpdateRequest()
        {
            return new UpdateEventRequest
            {
                Id = Id,
                StartTime = StartTime.ToUniversalTime(),
                Description = Description,
                EndTime = EndTime.ToUniversalTime(),
                CalUserId = CalUserId,
                SeriesId = SeriesId,
                Name = Name,
                CalendarId = CalendarId,
                Color = Color,
                NumTimesNotified = NumTimesNotified,
                ShouldNotify = ShouldNotify,
            };
        }

        public override string ToString() => $"{Name}-LOCAL START: {StartTime}- UTC START: {StartTime.ToUniversalTime()}-{EndTime}";

        public string? SeriesName { get; set; }

        public bool IsPartOfSeries => SeriesName != null;
        public bool IsSingularEvent => SeriesName == null;
    }
}
