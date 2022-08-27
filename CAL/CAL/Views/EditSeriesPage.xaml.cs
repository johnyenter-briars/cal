using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CAL.Views
{
    public partial class EditSeriesPage : ContentPage
    {
        public Event Item { get; set; }
        public EditSeriesPage ()
        {
            InitializeComponent();
            BindingContext = new EditSeriesViewModel();
        }
    }
}