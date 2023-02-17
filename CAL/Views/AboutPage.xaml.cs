using CAL.Managers;
using CAL.Models;
using CAL.ViewModels;
using System;
using System.ComponentModel;

namespace CAL.Views
{
	public partial class AboutPage : ContentPage
	{
		public AboutPage()
		{
			//InitializeComponent();
			InitializeComponent();
			BindingContext = new SettingsViewModel();
		}
	}
}