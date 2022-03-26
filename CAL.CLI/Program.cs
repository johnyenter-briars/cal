using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Request;

var cal = new CalClient();

var r = await cal.CreateEvent(new CreateEventRequest
{
    Name = "This came from CAL.Client",
    Time = DateTime.UtcNow,
});

var r2 = await cal.GetEvents();

Console.ReadKey();
