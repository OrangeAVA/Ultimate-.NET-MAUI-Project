
using EcommerceApp.ViewModels;
using EcommerceApp.Data;

namespace EcommerceApp.Views;

public partial class ProductDescriptionView : ContentPage
{
	public ProductDescriptionView(ProductDescriptionViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (this.BindingContext is ProductDescriptionViewModel viewModel)
        {
            viewModel.ConsultProduct(); 
        }
    }
}