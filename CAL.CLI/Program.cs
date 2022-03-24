using CAL.Client;

var cal = new CalClient();
var response = await cal.GetEvents();

Console.ReadKey();
