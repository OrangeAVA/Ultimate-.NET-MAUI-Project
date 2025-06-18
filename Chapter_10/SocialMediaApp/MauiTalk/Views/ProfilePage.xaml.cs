using MauiTalk.ViewModels;

namespace MauiTalk.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}