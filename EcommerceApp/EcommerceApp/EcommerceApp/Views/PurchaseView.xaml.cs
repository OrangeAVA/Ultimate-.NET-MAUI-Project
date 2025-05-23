using EcommerceApp.ViewModels;

namespace EcommerceApp.Views;

public partial class PurchaseView : ContentPage
{
	public PurchaseView(PurchaseViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (this.BindingContext is PurchaseViewModel viewModel)
        {
            viewModel.OnAppearing();
        }
    }
}