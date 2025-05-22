using EducationalApp.ViewModels;

namespace EducationalApp.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel viewmodel)
	{
		InitializeComponent();
        this.BindingContext = viewmodel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (this.BindingContext is LoginViewModel viewModel)
        {
            viewModel.OnAppearing();
        }
    }
}