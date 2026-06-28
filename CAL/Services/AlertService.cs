namespace CAL.Services
{
    public class AlertService
    {
        public Task ShowAlertAsync(string title, string message, string cancel = "OK")
        {
            return MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (Application.Current?.MainPage == null)
                {
                    return;
                }

                await Application.Current.MainPage.DisplayAlert(title, message, cancel);
            });
        }
    }
}
