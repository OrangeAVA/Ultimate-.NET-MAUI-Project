using ERP_APP.ViewModels;

namespace ERP_APP.Views;

public partial class FormView : ContentPage
{
	public FormView(FormViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}