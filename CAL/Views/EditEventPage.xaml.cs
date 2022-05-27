using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Views
{
    public partial class EditEventPage : ContentPage
    {
        public Event Item { get; set; }
        public EditEventPage()
        {
            InitializeComponent();
            BindingContext = new EditEventViewModel();
        }
    }
}