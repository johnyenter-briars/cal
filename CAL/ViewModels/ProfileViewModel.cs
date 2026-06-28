using CAL.Client;
using CAL.Client.Models.Cal;
using CAL.Managers;

namespace CAL.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    private CalUser calUser;
    public CalUser  CalUser
    {
        get => calUser;
        set => SetProperty(ref calUser, value);
    }

    public ProfileViewModel()
    {
        Title = "Profile";
    }
    public async void Refresh()
    {
        try
        {
            var userId = PreferencesManager.GetUserId();

            var (calUserResponse, success) = await Fallback(() => CalClientSingleton.GetCalUserAsync(new Guid(userId)));

            if (!success)
            {
                return;
            }

            CalUser = calUserResponse.User;
        }
        catch (Exception ex)
        {
            await FailsafeService.ShowFailureAlert("Network Error", ex);
        }
    }
}
