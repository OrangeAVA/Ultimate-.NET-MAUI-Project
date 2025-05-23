using EcommerceApp.ViewModels;
using EcommerceApp.Data;

namespace EcommerceApp.Views;

public partial class ShoppingCartView : ContentPage
{
	public ShoppingCartView()
	{
		InitializeComponent();
		var _dataBaseContext = new DatabaseContext(); 
		this.BindingContext = new ShoppingCartViewModel(_dataBaseContext);
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		if (this.BindingContext is ShoppingCartViewModel viewModel)
		{
			viewModel.OnAppearing();
		}
    }
}