using EducationalApp.ViewModels;

namespace EducationalApp.Views;

public partial class CourseDescriptionView : ContentPage
{
	public CourseDescriptionView(CourseDescriptionViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (this.BindingContext is CourseDescriptionViewModel viewModel)
        {
            viewModel.ConsultCourse();
        }
    }
}