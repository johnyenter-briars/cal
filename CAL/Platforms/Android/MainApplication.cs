using Android.App;
using Android.Runtime;
using CAL.Platforms.Android;

namespace CAL;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
		DependencyService.RegisterSingleton<INotificationManager>(new AndroidNotificationManager());
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
