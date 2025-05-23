
using EcommerceApp.ViewModels;

namespace EcommerceApp.Views;

public partial class UserView : ContentPage
{
	public UserView(UserViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (this.BindingContext is UserViewModel viewModel)
        {
            viewModel.OnAppearing();
        }
    }
}