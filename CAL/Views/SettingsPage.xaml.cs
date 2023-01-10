using CAL.Managers;
using CAL.Models;
using CAL.ViewModels;
using System;
using System.ComponentModel;

namespace CAL.Views
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent();
			BindingContext = new SettingsViewModel();
		}
	}
}