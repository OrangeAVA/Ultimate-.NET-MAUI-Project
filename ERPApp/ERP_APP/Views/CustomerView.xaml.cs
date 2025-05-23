using ERP_APP.ViewModels;

namespace ERP_APP.Views;

public partial class CustomerView : ContentPage
{
	public CustomerView(CustomerViewModel viewmodel)
	{
		InitializeComponent();
        this.BindingContext = viewmodel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (this.BindingContext is CustomerViewModel viewModel)
        {
            viewModel.OnAppearing();
        }
    }
}