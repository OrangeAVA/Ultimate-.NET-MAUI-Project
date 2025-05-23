
using EcommerceApp.ViewModels;

namespace EcommerceApp.Views;

public partial class MenuView : ContentPage
{
	public MenuView(MenuViewModel viewModel)
	{
		InitializeComponent();
        this.BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (this.BindingContext is MenuViewModel viewModel)
        {
            viewModel.OnAppearing();
        }
    }
}