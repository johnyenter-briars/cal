using CAL.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace CAL.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}