
using ERP_APP.Models;
using ERP_APP.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using static SQLite.SQLite3;

namespace ERP_APP.ViewModels;

public partial class WarehouseViewModel : BaseViewModel
{
    private DatabaseContext _databaseContext;

    private List<WareHouses> _items;

    [ObservableProperty]
    public string? searchText = string.Empty;

    [ObservableProperty]
    public bool cancelIsVisible = false;

    [ObservableProperty]
    public bool isVisible = false;

    [ObservableProperty]
    public ObservableCollection<WareHouses> items;

    public WarehouseViewModel(DatabaseContext databaseContext)
	{
        _databaseContext = databaseContext;
        _items = new List<WareHouses>();
    }

    public async Task OnAppearing()
    {
        IsVisible = true;
        Items = new ObservableCollection<WareHouses>();
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
        try
        {
            _items.RemoveAll(i => i.Id == id);
            var rm = Items.FirstOrDefault(i => i.Id == id);
            if (rm != null)
            {
                if (await _databaseContext.DeleteItemByKeyAsync<WareHouses>(id))
                {
                    Items.Remove(rm);
                }
            }
        }
        catch (Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("WareHouses Pages", ex.Message, "OK");
        }
        IsVisible = false;
    }

    [RelayCommand]
    public void TapButtonCancel()
    {
        SearchText = string.Empty;
    }


    [RelayCommand]
    public async Task TapButtonAddNewRecords() => await Shell.Current.GoToAsync($"FormPage?typeForm=Warehouse");


    private void SearchAsync(string query)
    {
        var items = _items.Where(item =>
                (!string.IsNullOrEmpty(item.Name) && item.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        this.FillItems(items);
    }

    private void FillItems(List<WareHouses> items)
    {
        Items.Clear();
        foreach (var item in items)
        {
            Items.Add(new WareHouses { Name = item.Name, Unit = item.Unit, Id = item.Id });
        }
    }

    private async Task GetData()
    {
        try
        {
            _items.Clear();
            var result = (await _databaseContext.GetAllAsync<WareHouses>());
            if(result.Any())
            {
                foreach(var item in result)
                {
                    _items.Add(new WareHouses
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Unit = item.Unit
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("WareHouses Pages", ex.Message, "OK");
        }
    }
}