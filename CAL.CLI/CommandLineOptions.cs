using CommandLine;

public class CommandLineOptions
{
    [Option(shortName: 'd', longName: "day", Required = false, HelpText = "The day of the current month to list events for")]
    public int? DayToListEvents { get; set; }
}
