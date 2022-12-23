using System;
using System.Collections.Generic;
using System.Text;

namespace CAL.Client.Models.Server.Response
{
    public class DeletedEntityResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}
