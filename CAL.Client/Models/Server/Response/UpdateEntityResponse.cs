using CAL.Client.Interfaces;
using System;

namespace CAL.Client.Models.Server.Response
{
    public class UpdateEntityResponse : BaseResponse
    {
        public Guid? EntityId { get; set; }
    }
}
