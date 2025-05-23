using ERP_APP.ViewModels;

namespace ERP_APP.Views;

public partial class PurchaseView : ContentPage
{
	public PurchaseView(PurchaseViewModel viewmodel)
	{
		InitializeComponent();
        this.BindingContext = viewmodel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (this.BindingContext is PurchaseViewModel viewModel)
        {
            _ = viewModel.OnAppearing();
        }
    }
}