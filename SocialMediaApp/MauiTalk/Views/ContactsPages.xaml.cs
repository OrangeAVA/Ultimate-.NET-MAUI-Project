
using MauiTalk.ViewModels;

namespace MauiTalk.Views;

public partial class ContactsPages : ContentPage
{
	public ContactsPages(ContactsViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (this.BindingContext is ContactsViewModel viewModel)
        {
            _ = viewModel.OnAppearing();
        }
    }
}