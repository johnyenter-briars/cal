using Android.OS;

namespace CAL.Droid.Services
{
    public class TimestampBinder : Binder
    {
        public TimestampBinder(TimestampService service)
        {
            this.Service = service;
        }

        public TimestampService Service { get; private set; }
    }
}
