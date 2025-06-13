
using MyFirstApplicationWithMVVM.ViewModels;

namespace MyFirstApplicationWithMVVM.Page;

public partial class SumOfQuantitiesPage : ContentPage
{
	public SumOfQuantitiesPage()
	{
		InitializeComponent();
		BindingContext = new SumOfQuantitiesViewModel();

    }
}