using Android.App;
using Android.Runtime;
using BaggageApp.Helpers;
using BaggageApp.Platforms.Android.Helpers;

namespace BaggageApp;

//[Application]
[Application(UsesCleartextTraffic = true)]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
        DependencyService.Register<IFileHelper, FileHelper>();
    }

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
