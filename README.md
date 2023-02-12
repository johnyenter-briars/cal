# CAL

Simple calendar mangement.

CAL provides a multitude of methods for calendar management across different interfaces. Currently CAL supports a cross-platform app and a command line interface.

## Project Breakdown
| Project      | Description |
| ----------- | ----------- |
| CAL      | Maui cross platform app       |
| CAL.Client   | Class library for handing shared business logic for connecting to cal-server        |
| CAL.CLI      | CLI for interfacing with cal-server       |

## Server
CAL interops with the [cal-server](https://github.com/johnyenter-briars/cal-server), a server implementation which handles state management and data storage.

## About
The philosophy of this project is to leverage .NET's ability to share business logic efficently, and extend shared features simply between an app and command line interface.