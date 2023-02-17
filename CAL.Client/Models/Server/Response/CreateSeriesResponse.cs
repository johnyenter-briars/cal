using CAL.Client.Interfaces;
using System;

namespace CAL.Client.Models.Server.Response
{
    public class CreateSeriesResponse : BaseResponse
    {
        public Guid SeriesId { get; set; }
    }
}


