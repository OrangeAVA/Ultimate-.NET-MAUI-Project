using ERP_APP.ViewModels;

namespace ERP_APP.Views;

public partial class DashBoardView : ContentPage
{
	public DashBoardView(DashBoardViewModel bindigViewModel)
	{
		InitializeComponent();
		this.BindingContext = bindigViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (this.BindingContext is DashBoardViewModel viewModel)
        {
            viewModel.OnAppearing();
        }
    }
}