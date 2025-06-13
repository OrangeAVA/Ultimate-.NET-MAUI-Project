using ERP_APP.ViewModels;

namespace ERP_APP.Views;

public partial class SaleView : ContentPage
{
	public SaleView(SaleViewModel viewmodel)
	{
		InitializeComponent();
		this.BindingContext = viewmodel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (this.BindingContext is SaleViewModel viewModel)
        {
            viewModel.OnAppearing();
        }
    }
}