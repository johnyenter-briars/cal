﻿using CAL.Client;
using CAL.Client.Models.Cal;
using CAL.Managers;
using CAL.Services;
using CAL.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CAL.Views
{
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            Task.Run(async () =>
            {
                var calClient = DependencyService.Get<ICalClient>();
                var calendarsForUser = await calClient.GetCalendarsForUserAsync(new Guid(PreferencesManager.GetUserId()));
                var viewModel = new CalendarViewModel(calendarsForUser.Calendars.FirstOrDefault());

                BindingContext = viewModel;

                foreach(var calendar in calendarsForUser.Calendars)
                {
                    ToolbarItems.Add(new ToolbarItem
                    {
                        Order = ToolbarItemOrder.Secondary,
                        Text = calendar.Name,
                        Command = viewModel.SelectCalendarCommand,
                        CommandParameter = calendar,
                    });
                }
            });

            InitializeComponent();
        }
    }
}
