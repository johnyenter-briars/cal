using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Client.Models.Server.Request;
using CAL.Client.Models.Server.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Client
{
    public static class CalClientFactory
    {
        public static ICalClient GetNewCalClient()
        {
            return new CalClient();
        } 
    }
}

