using System;
using System.Collections.Generic;
using System.Text;

namespace CAL.Client.Interfaces
{
    internal interface IBaseResponse
    {
        string GetMessage();
        int GetStatusCode();
    }
}
