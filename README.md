# CAL

Simple calendar mangement.

CAL provides a multitude of methods for calendar management across different interfaces. Currently CAL supports a cross-platform app and a command line interface.

### Built With

* [MAUI](https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui?view=net-maui-7.0)
* [XCalendar](https://github.com/ME-MarvinE/XCalendar)

## Project Breakdown
| Project      | Description |
| ----------- | ----------- |
| CAL      | Maui cross platform app       |
| CAL.Client   | .NET 6.0 Class library for handing shared business logic for connecting to cal-server        |
| CAL.CLI      | .NET 6.0 CLI for interfacing with cal-server       |

## Server
CAL interops with the [cal-server](https://github.com/johnyenter-briars/cal-server), a server implementation which handles state management and data storage.

## About
The philosophy of this project is to leverage .NET's ability to share business logic efficently, and extend shared features simply between an app and command line interface.

## Known Issues on Android
- [Pressing the return key or 'done' on the keyboard does not dismiss the keyboard](https://github.com/dotnet/maui/issues/10858)
- Because the app makes frequent network requests using [`AlarmManager`](https://developer.android.com/reference/android/app/AlarmManager.html) the app needs to be set to 'Unrestricted Battery Access' in device settings
