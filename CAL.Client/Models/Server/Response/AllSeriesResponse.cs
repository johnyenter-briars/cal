using CAL.Client.Interfaces;
using CAL.Client.Models.Cal;
using System.Collections.Generic;

namespace CAL.Client.Models.Server.Response
{
    public class AllSeriesResponse : BaseResponse 
    {
        public List<Series> Series { get; set; }
    }
}



