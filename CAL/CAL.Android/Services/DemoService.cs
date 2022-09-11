using Android.App;
using Android.Content;
using Android.OS;

namespace CAL.Droid.Services
{
    [Service]
    public class DemoService : Service
    {
        // Magical code that makes the service do wonderful things.
        public override IBinder OnBind(Intent intent)
        {
            throw new System.NotImplementedException();
        }
    }
}
