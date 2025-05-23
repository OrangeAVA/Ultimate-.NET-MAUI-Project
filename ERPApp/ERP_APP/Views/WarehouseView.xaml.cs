using ERP_APP.ViewModels;

namespace ERP_APP.Views;

public partial class WarehouseView : ContentPage
{
	public WarehouseView(WarehouseViewModel viewmodel)
	{
		InitializeComponent();
        this.BindingContext = viewmodel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (this.BindingContext is WarehouseViewModel viewModel)
        {

            _ = viewModel.OnAppearing();
        }
    }
}