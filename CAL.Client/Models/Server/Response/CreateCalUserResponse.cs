using System;

namespace CAL.Client.Models.Server.Response
{
    public class CreateCalUserResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Guid? CalUserId { get; set; }
    }
}

