using CAL.Client.Models.Server.Request;
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
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CalUserId { get; set; }
        public Guid? SeriesId { get; set; }
        public CreateEventRequest ToRequest()
        {
            return new CreateEventRequest
            {
                StartTime = StartTime,
                Description = Description,
                EndTime = EndTime,
                CalUserId = CalUserId,
                SeriesId = SeriesId,
                Name = Name,
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
            };
        }

        public override string ToString() => $"{Name}-LOCAL START: {StartTime}- UTC START: {StartTime.ToUniversalTime()}-{EndTime}";
    }
}
