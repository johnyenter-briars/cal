using System.Diagnostics;

namespace CAL.Services
{
    public class FailsafeService
    {
        private readonly AlertService alertService = new AlertService();

        public async Task<(TResult ResultObject, bool Success)> Fallback<TResult>(
            Func<Task<TResult>> callback,
            TResult defaultValueIfFailed = default,
            string title = null)
        {
            try
            {
                TResult result = await callback();

                if (result == null)
                {
                    return (result, false);
                }

                return (result, true);
            }
            catch (Exception ex)
            {
                await ShowFailureAlert(title, ex);
                return (defaultValueIfFailed, false);
            }
        }

        public async Task<bool> Fallback(Func<Task> callback, string title = null)
        {
            try
            {
                await callback();
                return true;
            }
            catch (Exception ex)
            {
                await ShowFailureAlert(title, ex);
                return false;
            }
        }

        public async Task ShowFailureAlert(string title, Exception ex)
        {
            Debug.WriteLine(ex);
            var details = string.IsNullOrWhiteSpace(ex.Message)
                ? ex.GetType().Name
                : ex.Message;

            await alertService.ShowAlertAsync(
                title ?? "Network Error",
                $"Unable to reach the calendar server. Please check your connection and try again.\n\nDetails: {details}");
        }
    }
}
