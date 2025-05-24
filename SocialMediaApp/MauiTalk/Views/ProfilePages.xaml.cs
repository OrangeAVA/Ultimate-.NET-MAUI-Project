using MauiTalk.ViewModels;

namespace MauiTalk.Views;

public partial class ProfilePages : ContentPage
{
	public ProfilePages(ProfileViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}