using MauiTalk.ViewModels;

namespace MauiTalk.Views;

public partial class ChatPages : ContentPage
{
	public ChatPages(ChatViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}