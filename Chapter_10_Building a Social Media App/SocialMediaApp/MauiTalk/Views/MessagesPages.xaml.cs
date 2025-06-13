using MauiTalk.ViewModels;

namespace MauiTalk.Views;

public partial class MessagesPages : ContentPage
{
	public MessagesPages(MessagesViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
    }

}