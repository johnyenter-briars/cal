using System;
using CAL.Client.Models.Server.Request;
using Newtonsoft.Json;

namespace CAL.Client.Models.Cal
{
    public class CalUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CreateCalUserRequest ToRequest()
        {
            return new CreateCalUserRequest
            {
                FirstName = FirstName,
                LastName = LastName,
            };
        }
        [JsonIgnore]
        public EntityType EntityType => EntityType.CalUser;
    }
}
