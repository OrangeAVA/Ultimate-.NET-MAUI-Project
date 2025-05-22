using EducationalApp.ViewModels;

namespace EducationalApp.Views;

public partial class WishListView : ContentPage
{
	public WishListView(WishListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (this.BindingContext is WishListViewModel viewModel)
        {
            viewModel.GetData();
        }
    }
}