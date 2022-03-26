using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Request;

var r = await CalClient.CreateEvent(new CreateEventRequest
{
    Name = "This came from CAL.Client",
    Time = DateTime.UtcNow,
});

var r2 = await CalClient.GetEvents();

Console.ReadKey();
