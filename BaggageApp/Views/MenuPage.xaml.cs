using BaggageApp.ViewModels;

namespace BaggageApp.Views;

public partial class MenuPage : ContentPage
{
	public MenuPage()
	{
		InitializeComponent();
		this.BindingContext = new MenuPageViewModel();
	}
}