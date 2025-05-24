using CommunityToolkit.Mvvm.Input;
using MauiTalk.ViewModels;

namespace MauiTalk.Views;

public partial class FriendsPages : ContentPage
{
	public FriendsPages(FriendsViewModel viewmodel)
	{
		InitializeComponent();
		this.BindingContext = viewmodel;
	}
}