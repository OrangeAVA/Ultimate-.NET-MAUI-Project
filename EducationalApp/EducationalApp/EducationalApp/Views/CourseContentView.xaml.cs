using EducationalApp.ViewModels;

namespace EducationalApp.Views;

public partial class CourseContentView : ContentPage
{
	public CourseContentView(CourseContentViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (this.BindingContext is CourseContentViewModel viewModel)
        {
            viewModel.ConsultCourse();
        }
    }
}