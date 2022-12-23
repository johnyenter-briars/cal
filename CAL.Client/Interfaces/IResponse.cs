using System;
using System.Collections.Generic;
using System.Text;

namespace CAL.Client.Interfaces
{
    internal interface IResponse
    {
        string GetMessage();
        int GetStatusCode();
        IResponse SetMessage(string message);
        IResponse SetStatusCode(int statusCode);
    }
}
