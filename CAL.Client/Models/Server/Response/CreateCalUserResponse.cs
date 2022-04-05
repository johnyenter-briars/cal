using CAL.Client.Interfaces;
using System;

namespace CAL.Client.Models.Server.Response
{
    public class CreateCalUserResponse : IResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Guid? CalUserId { get; set; }
        public string GetMessage() => Message;
        public int GetStatusCode() => StatusCode;
    }
}

