using CAL.Client;
using CAL.Client.Models.Cal;
using CAL.Managers;
using CAL.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CAL.Views
{
	public partial class CalendarPage : ContentPage
	{
		public CalendarPage()
		{
			InitializeComponent();

			BindingContext = new CalendarViewModel();
		}
		protected override void OnAppearing()
		{
			var bc = (CalendarViewModel)BindingContext;

			bc?.Refresh();
		}
	}
}
