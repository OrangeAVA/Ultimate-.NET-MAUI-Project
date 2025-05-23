
using ERP_APP.ViewModels;

namespace ERP_APP.Views;

public partial class SuppliersView : ContentPage
{
	public SuppliersView(SuppliersViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (this.BindingContext is SuppliersViewModel viewModel)
        {
            _ = viewModel.OnAppearing();
        }
    }
}