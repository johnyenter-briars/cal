using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Cal.Request;
using CAL.Client.Models.Server.Request;
using CommandLine;

await Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsedAsync<CommandLineOptions>(async options =>
{
    var client = CalClientFactory.GetNewCalClient();

    var day = options.DayToListEvents;

    if(day is int value)
    {
        var events = await client.GetEventsForDayAsync(value);
        foreach(var e in events)
        {
            Console.WriteLine(e);
        }
    }
});
