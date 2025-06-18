using MauiTalk.ViewModels;

namespace MauiTalk.Views;

public partial class ChatPage : ContentPage
{
	public ChatPage(ChatViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}