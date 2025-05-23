using EcommerceApp.ViewModels;

namespace EcommerceApp.Views;

public partial class HomeView : ContentPage
{
	public HomeView(HomeViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (this.BindingContext is HomeViewModel viewModel)
        {
            viewModel.GetData();
        }
    }
}