using CommunityToolkit.Maui.Views;
using System.Xml.Linq;

namespace ERP_APP.Controls;

public partial class ProductPopup : Popup
{
	public ProductPopup()
	{
		InitializeComponent();
	}


    public ProdutsSavedEventHandler ProductsSaved;

    public delegate void ProdutsSavedEventHandler(string name, Decimal price, Decimal purchasePrice, int unit);

    private void OnSaveClicked(object sender, EventArgs e)
    {
        string name = txtName.Text?.Trim(); // Delete extra spaces

        if (string.IsNullOrWhiteSpace(name))
        {
            Application.Current.MainPage.DisplayAlert("Error", "The product name cannot be empty.", "OK");
            return;
        }

        if (!decimal.TryParse(txtPrice.Text, out decimal price))
        {
            Application.Current.MainPage.DisplayAlert("Error", "The entered price is not valid.", "OK");
            return;
        }

        if (!decimal.TryParse(txtPurchasePrice.Text, out decimal purchasePrice))
        {
            Application.Current.MainPage.DisplayAlert("Error", "The entered purchase price is not valid.", "OK");
            return;
        }

        if (!int.TryParse(txtUnit.Text, out int unit))
        {
            Application.Current.MainPage.DisplayAlert("Error", "The entered units are not valid.", "OK");
            return;
        }

        // If everything is correct, trigger the event.
        ProductsSaved?.Invoke(name, price, purchasePrice, unit);

        Close();
    }

}