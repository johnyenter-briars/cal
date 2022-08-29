using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAL.Client.Models.Cal
{
    public class Calendar
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CalUserId { get; set; }
        public string Color { get; set; }
        [JsonIgnore]
        public EntityType EntityType => EntityType.Calendar;
    }
}
