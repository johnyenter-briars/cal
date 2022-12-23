using CAL.Client.Interfaces;
using System;

namespace CAL.Client.Models.Server.Response
{
    public class CreateCalUserResponse : BaseResponse
    {
        public Guid? CalUserId { get; set; }
    }
}

