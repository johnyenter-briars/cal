using CAL.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAL.Client.Models.Server.Response
{
    public abstract class BaseResponse : IResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = "";
        public string GetMessage() => Message;
        public int GetStatusCode() => StatusCode;
        IResponse IResponse.SetMessage(string message)
        {
            Message = message;
            return this;
        }
        IResponse IResponse.SetStatusCode(int statusCode)
        {
            StatusCode = statusCode;
            return this;
        }
    }
}
