using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Cal.Request;
using CAL.Client.Models.Server.Request;

var client = CalClientFactory.GetNewCalClient();

var createUserResponse = await client.CreateCalUserAsync(new CreateCalUserRequest{
    FirstName = "Test1",
    LastName = "test2",
});

var userResponse = await client.GetCalUserAsync(createUserResponse.CalUserId.Value);

var newEventResponse = await client.CreateEventAsync(new CreateEventRequest
{
    Name = "This came from CAL.Client",
    StartTime = DateTime.UtcNow,
    EndTime = DateTime.UtcNow,
    CalUserId = userResponse.User.Id,
});

var r2 = await client.GetEventsAsync();

Console.ReadKey();
