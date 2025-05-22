using EducationalApp.ViewModels;

namespace EducationalApp.Views;

public partial class MyCoursesView : ContentPage
{
	public MyCoursesView(MyCoursesViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (this.BindingContext is MyCoursesViewModel viewModel)
        {
            viewModel.GetData();
        }
    }
}