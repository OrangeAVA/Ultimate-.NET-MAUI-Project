
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ERP_APP.Models;

namespace ERP_APP.ViewModels;

public partial class DashBoardViewModel : BaseViewModel
{
	[ObservableProperty]
	ObservableCollection<DashBoard> optionDashBoard;

    public void OnAppearing()
	{
        OptionDashBoard = new ObservableCollection<DashBoard>()
        {
            new DashBoard { Title = "Supplers", Image = "supplier.png", Page = "SupplersPage"},
            new DashBoard { Title = "Sale", Image = "sales.png", Page = "SalePage"},
            new DashBoard { Title = "Purchase", Image = "shop.png", Page = "PurchasePage"},
            new DashBoard { Title = "Customer", Image = "customers.png", Page = "CustomerPage"},
            new DashBoard { Title = "Warehouse", Image = "store.png", Page = "WarehousePage"},
        };
    }


    [RelayCommand]
	public async Task TapButtonOption(string page) =>  await Shell.Current.GoToAsync($"{page}");

}
