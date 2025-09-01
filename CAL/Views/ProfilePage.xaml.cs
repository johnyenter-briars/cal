using CAL.Client;
using CAL.Managers;
using CAL.Models;
using CAL.ViewModels;
using System;
using System.ComponentModel;

namespace CAL.Views
{
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage()
		{
			//InitializeComponent();
			InitializeComponent();
			BindingContext = new ProfileViewModel();
		}
        protected override void OnAppearing()
        {
            var bc = (ProfileViewModel)BindingContext;

            bc?.Refresh();
        }
	}
}