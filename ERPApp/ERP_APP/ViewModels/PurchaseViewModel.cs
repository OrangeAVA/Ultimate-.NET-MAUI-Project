

using ERP_APP.Models;
using ERP_APP.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ERP_APP.ViewModels;

public partial class PurchaseViewModel : BaseViewModel
{
    private DatabaseContext _databaseContext;

    private List<ItemSupplier> _items;

    [ObservableProperty]
    public string? searchText = string.Empty;

    [ObservableProperty]
    public bool cancelIsVisible = false;

    [ObservableProperty]
    public ObservableCollection<ItemSupplier> items;

    [ObservableProperty]
    public bool isVisible = false;


    public PurchaseViewModel(DatabaseContext databaseContext)
	{
        _databaseContext = databaseContext;
        _items = new List<ItemSupplier>();
        IsVisible = false;
    }

    public async Task OnAppearing()
    {
        IsVisible = true;
        Items = new ObservableCollection<ItemSupplier>();
        await this.GetData();
        this.FillItems(_items);
        IsVisible = false;
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
        IsVisible = true;
        _items.RemoveAll(i => i.Id == id);
        var rm = Items.FirstOrDefault(i => i.Id == id);
        if (rm != null)
        {
            Items.Remove(rm);
        }
        IsVisible = false;
    }

    [RelayCommand]
    public void TapButtonCancel()
    {
        SearchText = string.Empty;
    }

    [RelayCommand]
    public async Task TapButtonAddNewRecords() => await Shell.Current.GoToAsync($"FormPage?typeForm=Purchase");

    private void SearchAsync(string query)
    {
        var items = _items.Where(item =>
                (!string.IsNullOrEmpty(item.Name) && item.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(item.Date) && item.Date.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(item.Total) && item.Total.Contains(query, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        this.FillItems(items);
    }

    private void FillItems(List<ItemSupplier> items)
    {
        Items.Clear();
        foreach (var item in items)
        {
            Items.Add(new ItemSupplier { Name = item.Name, Total = item.Total, Date = item.Date, Id = item.Id });
        }
    }

    private async Task GetData()
    {
        try
        {
            _items.Clear();
            var shoppings = (await _databaseContext.GetAllAsync<Shopping>());
            if (shoppings.Any())
            {
                foreach (var item in shoppings)
                {
                    var supplier = (await _databaseContext.GetAllAsync<Suppliers>())
                        .Where(x => x.Id == item.fk_supplier).FirstOrDefault();
                   if(supplier != null)
                    {
                        _items.Add(new ItemSupplier
                        {
                            Name = supplier.Name,
                            Total = item.Total ?? "$0.00",
                            Date = item.Date.ToString("dd-mm-yyyy"),
                            Id = supplier.Id
                        });
                    }
                }
                return;
            }
        }
        catch(Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("Forms", ex.Message, "OK");
        }
    }
}