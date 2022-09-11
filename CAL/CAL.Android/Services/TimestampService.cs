
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using System.Threading.Tasks;
using CAL.Client;
using Xamarin.Forms;
using System;

namespace CAL.Droid.Services
{
    [Service(Name = "com.xamarin.ServicesDemo1")]
    public class TimestampService : Service, IGetTimestamp
    {
        static readonly string TAG = typeof(TimestampService).FullName;
        //IGetTimestamp timestamper;

        public IBinder Binder { get; private set; }

        public override void OnCreate()
        {
            // This method is optional to implement
            base.OnCreate();
            Log.Debug(TAG, "OnCreate");
            //timestamper = new UtcTimestamper();
        }

        public override IBinder OnBind(Intent intent)
        {
            // This method must always be implemented
            Log.Debug(TAG, "OnBind");
            this.Binder = new TimestampBinder(this);
            return this.Binder;
        }

        public override bool OnUnbind(Intent intent)
        {
            // This method is optional to implement
            Log.Debug(TAG, "OnUnbind");
            return base.OnUnbind(intent);
        }

        public override void OnDestroy()
        {
            // This method is optional to implement
            Log.Debug(TAG, "OnDestroy");
            Binder = null;
            base.OnDestroy();
        }

        /// <summary>
        /// This method will return a formatted timestamp to the client.
        /// </summary>
        /// <returns>A string that details what time the service started and how long it has been running.</returns>
        public string GetFormattedTimestamp()
        {
            return "idk";
        }

        public async Task ConnectToAPI()
        {
            ICalClient client = DependencyService.Get<ICalClient>();

            while (true)
            {
                try
                {
                    var events = await client.GetEventsAsync();
                }
                catch (Exception e)
                {

                    var i = 50;

                }
                    
                var k = 50;
                await Task.Delay(2000);
                var j = 50;

            }

        }
    }

}

