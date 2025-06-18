using MauiTalk.ViewModels;

namespace MauiTalk.Views;

public partial class MessagesPage : ContentPage
{
	public MessagesPage(MessagesViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
    }

}