using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Server.Request;

var r = await CalClient.CreateEvent(new CreateEventRequest
{
    Name = "This came from CAL.Client",
    StartTime = DateTime.UtcNow,
});

var r2 = await CalClient.GetEvents();

Console.ReadKey();
