using CAL.Client;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CAL.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
        }

        public ICommand OpenWebCommand => new Command(TestCalClient);
        private async void TestCalClient(object obj)
        {
            var events = await CalClient.GetEvents();
        }
    }
}