
using ERP_APP.Models;
using ERP_APP.Controls;
using ERP_APP.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml.Linq;
using static Microsoft.Maui.ApplicationModel.Permissions;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using System;

namespace ERP_APP.ViewModels;

public partial class FormViewModel : BaseViewModel, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        IsPurchase = false;
        IsSale = false;
        IsCustomer = false;
        IsSupplier = false;
        _parameter = string.Empty;
        _formType = query["typeForm"] as string;
        TitleView = $"{_formType} Registration";
        if(!string.IsNullOrEmpty(_formType))
        {
            switch (_formType)
            {
                case "Supplier":
                    IsSupplier = true;
                    break;
                case "Purchase":
                    IsPurchase = true;
                    PurchaseDate = DateTime.Now;
                    PurchaseTotal = string.Empty;
                    Produts = string.Empty;
                    _wareHouses = new List<WareHouses>();
                    _ = GetSupplier();
                    break;
                case "Sale":
                    IsSale = true;
                    UnitSales = string.Empty;
                    SelectedWareHouses = new WareHouses();
                    OptionsWareHouses = new ObservableCollection<WareHouses>();
                    _ = GetProducts();
                    break;
                case "Customer":
                    IsCustomer = true;
                    break;
            }
        }
    }

    private DatabaseContext _databaseContext;

    private List<WareHouses> _wareHouses;

    private string? _formType = string.Empty;

    [ObservableProperty]
    private bool isSupplier = false;

    [ObservableProperty]
    private bool isPurchase = false;

    [ObservableProperty]
    private bool isSale = false;

    [ObservableProperty]
    private bool isCustomer = false;

    [ObservableProperty]
    private string titleView = string.Empty;

    // Supplier Properties
    [ObservableProperty]
    private string supplierName;

    [ObservableProperty]
    private string supplierPhone;

    [ObservableProperty]
    private string supplierEmail;

    // Customer Properties
    [ObservableProperty]
    private string customerName;

    [ObservableProperty]
    private string customerEmail;

    [ObservableProperty]
    private string customerPhone;

    // Purchase Properties

    [ObservableProperty]
    private Suppliers selectedSupplier;

    [ObservableProperty]
    private ObservableCollection<Suppliers> options;

    [ObservableProperty]
    private string purchaseTotal;

    [ObservableProperty]
    private DateTime purchaseDate;

    [ObservableProperty]
    private string produts;

    // Sale 
    [ObservableProperty]
    private string saleTotal;

    [ObservableProperty]
    private string unitSales;

    [ObservableProperty]
    private WareHouses selectedWareHouses;

    [ObservableProperty]
    private ObservableCollection<WareHouses> optionsWareHouses;

    private string _parameter = string.Empty;


    public FormViewModel(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

   [RelayCommand]
    public async Task SaveData(string form)
    {
        var message = "There are empty elements in the form";
        var status = "Success";
        switch (_formType)
        {
            case "Supplier":
                if (IsValid(SupplierName, SupplierPhone, SupplierEmail))
                {
                    message = await SupplierFunction();
                }
                break;
            case "Purchase":
                if (IsValid(SelectedSupplier.Name ?? string.Empty, PurchaseTotal, Produts))
                {
                    message = await PurchaseFunction();
                }
                break;
            case "Sale":
                if (IsValid(SelectedWareHouses.Name ?? string.Empty, UnitSales))
                {
                    message = await this.SalesFunction();
                    if(message == "product successfully added")
                        return;
                }
                break;
            case "Customer":
                if (IsValid(CustomerName, CustomerEmail, CustomerPhone))
                {
                    message = await CustomerFunction();
                }
                break;
            default:
                status = "Error";
                message = "Pagina no valida";
                break;
        }
        Application.Current.MainPage.DisplayAlert("status", message, "OK");
    }
    
    //Returns true if there are any empty or null fields, false if all are valid.
    private bool IsValid(params string[] fields) => fields.All(val => !string.IsNullOrEmpty(val));

    // Supplier Functions
    private async Task<string> SupplierFunction()
    {
        string message = string.Empty;
        try
        {
            var newSuppler = new Suppliers()
            {
                Name = SupplierName,
                Email = SupplierEmail,
                Phone = SupplierPhone,
                Active = true,
                RegistrationDate = DateTime.Now
            };
            var result = (await _databaseContext.GetAllAsync<Suppliers>())
                 .Where(x => x.Name == newSuppler.Name && x.Active == true).FirstOrDefault();

            if (result == null)
            {
                if(await _databaseContext.AddItemAsync(newSuppler))
                {
                    SupplierName = string.Empty;
                    SupplierEmail = string.Empty;
                    SupplierPhone = string.Empty;
                    return "Supplier Saved successfully";
                }
            }
            message = "This supplier is already registered";
        }
        catch(Exception ex)
        {
            message = ex.Message;
        }
        return message;
    }

    // Purchase Function
    private async Task<string> PurchaseFunction()
    {
        string message = string.Empty;
        try
        {
            var shopping = new Shopping()
            {
                fk_supplier = SelectedSupplier.Id,
                guid = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                Total = PurchaseTotal
            };
            if (await _databaseContext.AddItemAsync(shopping))
            {
                var idNewShopping = (await _databaseContext.GetAllAsync<Shopping>()).Where(x => x.guid == shopping.guid).Select(x => x.Id).FirstOrDefault();

                foreach(var item in _wareHouses)
                {
                    item.fk_Shopping = idNewShopping;
                    await _databaseContext.AddItemAsync(item);
                }

                PurchaseTotal = string.Empty;
                Produts = string.Empty;
                _wareHouses = new List<WareHouses>();

                return "Shopping Saved successfully";
            }
            message = "This Shopping is already registered";
        }
        catch (Exception ex)
        {
            message = ex.Message;
        }
        return message;
    }

    [RelayCommand]
    public async Task NewProduct()
    {
        decimal total = 0;
        var popup = new ProductPopup();
        popup.ProductsSaved += (name, price, purchasePrice, unit) =>
        {
            _wareHouses.Add(new WareHouses { Name = name, Unit = unit, Price = price, PurchasePrice = purchasePrice });            
            Produts += $"New product added: {name}.\n";
        };
        await App.Current.MainPage.ShowPopupAsync(popup);
        foreach(var item in _wareHouses)
        {
            total += (item.PurchasePrice * item.Unit);
        }
        PurchaseTotal = $"${total}";
    }

    private async Task GetSupplier()
    {
        string message = string.Empty;
        try
        {
            Options = new ObservableCollection<Suppliers>();
            var result = (await _databaseContext.GetAllAsync<Suppliers>()).Where(x => x.Active == true).Select(x => new { x.Id, x.Name });

            if (result.Any())
            {
                foreach(var item in result)
                {
                    Options.Add(new Suppliers { Id = item.Id, Name = item.Name });
                }
                return;
            }
            message = "Suppliers could not be consulted. Please check if there are records created.";
        }
        catch (Exception ex)
        {
            message = ex.Message;
        }
        Application.Current.MainPage.DisplayAlert("Forms", message, "OK");
    }

    // Customer Functions
    private async Task<string> CustomerFunction()
    {
        string message = string.Empty;
        try
        {
            var newCustomers = new Customers()
            {
                Name = CustomerName,
                Email = CustomerEmail,
                Phone = CustomerPhone,
                Active = true,
                RegistrationDate = DateTime.Now
            };
            var result = (await _databaseContext.GetAllAsync<Customers>())
                 .Where(x => x.Name == newCustomers.Name && x.Active == true).FirstOrDefault();

            if (result == null)
            {
                if (await _databaseContext.AddItemAsync(newCustomers))
                {
                    CustomerName = string.Empty;
                    CustomerEmail = string.Empty;
                    CustomerPhone = string.Empty;
                    return "Supplier Saved successfully";
                }
            }
            message = "This supplier is already registered";
        }
        catch (Exception ex)
        {
            message = ex.Message;
        }
        return message;
    }

    //Sales Functions
    private async Task GetProducts()
    {
        string message = string.Empty;
        try
        {
            OptionsWareHouses = new ObservableCollection<WareHouses>();
            var result = (await _databaseContext.GetAllAsync<WareHouses>());

            if (result.Any())
            {
                foreach (var item in result)
                {
                    OptionsWareHouses.Add(new WareHouses { Id = item.Id, Name = item.Name, Unit = item.Unit, PurchasePrice = item.PurchasePrice });
                }
                return;
            }
            message = "WareHouses could not be consulted. Please check if there are records created.";
        }
        catch (Exception ex)
        {
            message = ex.Message;
        }
        Application.Current.MainPage.DisplayAlert("Forms", message, "OK");
    }

    private async Task<string> SalesFunction()
    {
        string messege = "product successfully added";
        try
        {
            if(int.Parse(UnitSales) <= SelectedWareHouses.Unit)
            {
                _parameter= $"..?" +
                    $"id={SelectedWareHouses.Id}" +
                    $"&Prod={SelectedWareHouses.Name}" +
                    $"&Unit={UnitSales}" +
                    $"&PurchasePrice={SelectedWareHouses.PurchasePrice}";
                await Shell.Current.GoToAsync(_parameter, true);
            }
            else
            {
                messege = $"The added units are larger than those in the warehouse, in the warehouse there are: {SelectedWareHouses.Unit}";
            }
        }
        catch (Exception ex)
        {
            messege = ex.Message;
        }
        return messege;
    }
}