

using ERP_APP.Models;
using ERP_APP.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using System.Numerics;
using System.Globalization;

namespace ERP_APP.ViewModels;

public partial class SaleViewModel : BaseViewModel, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        int id = 0;
        string prod = string.Empty;
        string unit = string.Empty;
        decimal purchasePrice = 0m;

        if (query == null || query.Count == 0)
        {
            Console.WriteLine("No parameters received, exiting the method.");
            return;
        }

        var parameters = new (string Key, Func<string, bool> TryParse)[]
        {
        ("id", (string v) => int.TryParse(v, out id)),
        ("Prod", (string v) => { prod = v; return true; }),
        ("Unit", (string v) => { unit = v; return true; }),
        ("PurchasePrice", (string v) => decimal.TryParse(v, out purchasePrice))
        };

        // Try to get the values ​​and validate
        bool allValid = parameters.All(param =>
            query.TryGetValue(param.Key, out var value) && param.TryParse(value?.ToString() ?? string.Empty)
        );

        //If any conversion failed, you can handle it here
        if (allValid)
        {
            // Here you already have the values ​​correctly assigned
            Console.WriteLine($"ID: {id}, Product: {prod}, Unit: {unit}, Price: {purchasePrice}");
            var total = ((int.Parse(unit))*purchasePrice);
            _items.Add(new ItemSales {Id = id, Name = prod, Unit = unit, Total = total});
            this.FillItems(_items);
            Total = _items.Sum(x => x.Total);
        } 
    }


    private DatabaseContext _databaseContext;

    private List<ItemSales> _items;

    //CustomSearchControl 
    [ObservableProperty]
    public string? searchText = string.Empty;

    [ObservableProperty]
    public bool cancelIsVisible = false;

    //Picker
    [ObservableProperty]
    public Customers selectedOption;

    [ObservableProperty]
    public ObservableCollection<Customers> options;

    //Items
    [ObservableProperty]
    public ObservableCollection<ItemSales> items;

    [ObservableProperty]
    public decimal total;

    [ObservableProperty]
    public bool isCkecked;


    public SaleViewModel(DatabaseContext databaseContext)
	{
        _databaseContext = databaseContext;
        _items = new List<ItemSales>();
    }

    public async void OnAppearing()
    {
        Items = new ObservableCollection<ItemSales>();
        this.Options = new ObservableCollection<Customers>();
        this.FillItems(_items);
        await this.GetData();
        IsCkecked = false;
    }

    partial void OnSearchTextChanged(string? value)
    {
        CancelIsVisible = string.IsNullOrWhiteSpace(value) ? false : true;
        if (!string.IsNullOrWhiteSpace(value))
        {
            this.SearchAsync(value);
        }
        else
        {
            this.FillItems(_items);
        }
    }


    [RelayCommand]
    public async Task TapButtonItem(int id)
    {
        _items.RemoveAll(i => i.Id == id);
        var rm = Items.FirstOrDefault(i => i.Id == id);
        if (rm != null)
        {
            Items.Remove(rm);
        }
    }


    [RelayCommand]
    public async Task TapButtonAddProduct() => await Shell.Current.GoToAsync($"FormPage?typeForm=Sale");

    [RelayCommand]
    public async Task TapButtonAddNewRecords()
    {
        try
        {
            var op = (SelectedOption?.Name ?? null);
            if (IsCkecked && !string.IsNullOrWhiteSpace(op) && Items.Any())
            {
                Sales newSales = new Sales()
                {
                    fk_Customer = SelectedOption.Id,
                    PurchaseDate = DateTime.Now,
                    Total = Total
                };
                if(await _databaseContext.AddItemAsync(newSales))
                {
                    var id = (await _databaseContext.GetAllAsync<Sales>())
                        .Where(x => x.fk_Customer == newSales.fk_Customer && x.Total == newSales.Total)
                        .Select(x => x.Id).FirstOrDefault();
                    foreach(var item in _items)
                    {
                        ProductsSold newProd = new ProductsSold()
                        {
                            fk_Sales = id,
                            Quantity = int.Parse(item.Unit ?? "0"),
                            Total = item.Total,
                            fk_warehouses = item.Id,
                        };
                        if (await _databaseContext.AddItemAsync(newProd))
                        {
                            var update = (await _databaseContext.GetAllAsync<WareHouses>())
                            .Where(x => x.Id == item.Id).FirstOrDefault();
                            if(update != null)
                            {
                                update.Unit = update.Unit - newProd.Quantity;
                                if (await _databaseContext.UpdateItemAsync(update))
                                {
                                    Application.Current.MainPage.DisplayAlert("status", "Successful sale", "OK");
                                    _items.Clear();
                                    Items.Clear();
                                    IsCkecked = false;
                                    return;
                                }
                            }                   
                        }
                    }
                }
            }
            Application.Current.MainPage.DisplayAlert("status", "The sale could not be completed successfully.", "OK");
        }
        catch(Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("status", ex.Message, "OK");
        }
    }

    private void FillItems(List<ItemSales> items)
    {
        Items.Clear();
        foreach (var item in items)
        {
            Items.Add(new ItemSales { Unit = item.Unit, Name = item.Name, Id = item.Id, Total = item.Total });
        }
    }


    //CustomSearchControl 
    [RelayCommand]
    public void TapButtonCancel()
    {
        SearchText = string.Empty;
    }
    private void SearchAsync(string query)
    {
        var items = _items.Where(item =>
                (!string.IsNullOrEmpty(item.Name) && item.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        this.FillItems(items);
    }

    public async Task GetData()
    {
        try
        {
            var customers = (await _databaseContext.GetAllAsync<Customers>()).Where(x => x.Active == true).Select(x => new { Id = x.Id, Name = x.Name });
            if (customers.Any())
            {
                foreach (var item in customers)
                {
                    Options.Add(new Customers { Id = item.Id, Name = item.Name });
                }
            }
            else
            {
                 Application.Current.MainPage.DisplayAlert("status", "There are no registered clients yet. Please add them.", "OK");

            }
        }
        catch (Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("status", ex.Message, "OK");
        }
    }

}