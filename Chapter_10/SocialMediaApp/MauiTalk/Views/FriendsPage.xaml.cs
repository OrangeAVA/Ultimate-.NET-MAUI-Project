using CommunityToolkit.Mvvm.Input;
using MauiTalk.ViewModels;

namespace MauiTalk.Views;

public partial class FriendsPage : ContentPage
{
	public FriendsPage(FriendsViewModel viewmodel)
	{
		InitializeComponent();
		this.BindingContext = viewmodel;
	}
}