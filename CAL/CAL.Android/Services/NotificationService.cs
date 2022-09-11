using Android.App;
using Android.Content;
using Android.OS;
using System.Threading;

namespace CAL.Droid.Services
{
    public class NotificationService : Service
    {
        Timer timer;
        //TimerTask timerTask;
        ActionBar timerTask;
        string TAG = "Timers";
        int Your_X_SECS = 5;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            // This method executes on the main thread of the application.
            //Log.Debug("DemoService", "DemoService started");
            base.OnStartCommand(intent, flags, startId);

            startTimer();

            return StartCommandResult.Sticky;
        }
        public void startTimer()
        {
            //set a new Timer
            //timer = new Timer();

            //initialize the TimerTask's job
            initializeTimerTask();

            //schedule the timer, after the first 5000ms the TimerTask will run every 10000ms
            //timer.schedule(timerTask, 5000, Your_X_SECS * 1000); //
            //timer.schedule(timerTask, 5000,1000); //
        }
        public void stoptimertask()
        {
            //stop the timer, if it's not already null
            if (timer != null)
            {
                //timer.cancel();
                timer = null;
            }
        }

        public void initializeTimerTask()
        {

            //timerTask = new TimerTask()
            //{
            //public void run()
            //{

            //    //use a handler to run a toast that shows the current timestamp
            //    handler.post(new Runnable()
            //    {
            //        public void run()
            //    {

            //        //TODO CALL NOTIFICATION FUNC
            //        YOURNOTIFICATIONFUNCTION();

            //    }
            //});
        }
    }
}

