using BaggageApp.Services;

namespace BaggageApp;

public partial class App : Application
{
    public static BpmApiManager BpmApiManager { get; private set; }
    public static MicrosoftGraphApiManager GraphApiManager { get; private set; }

    public App()
	{
		InitializeComponent();

        MainPage = new AppShell();
        BpmApiManager = new BpmApiManager(new BpmApiService());
        GraphApiManager = new MicrosoftGraphApiManager(new MicrosoftGraphApiService());
    }
}
