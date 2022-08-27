using System;
using System.Collections.Generic;
using System.Text;

namespace CAL.Client.Models.Server.Response
{
    public class DeletedEntityResponse
    {
        public Guid Id { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
