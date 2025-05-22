using EducationalApp.ViewModels;

namespace EducationalApp.Views;

public partial class SearchView : ContentPage
{
	public SearchView(SearchViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (this.BindingContext is SearchViewModel viewModel)
        {
            viewModel.GetData();
        }
    }
}