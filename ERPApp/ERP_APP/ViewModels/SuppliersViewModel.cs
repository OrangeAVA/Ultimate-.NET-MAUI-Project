
using ERP_APP.Models;
using ERP_APP.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ERP_APP.ViewModels;

public partial class SuppliersViewModel : BaseViewModel
{
    private DatabaseContext _databaseContext;

    [ObservableProperty]
    private bool isVisible = false;

	[ObservableProperty]
	public string? searchText = string.Empty;

    [ObservableProperty]
    public bool cancelIsVisible = false;

    [ObservableProperty]
    public ObservableCollection<Suppliers> items;

    private List<Suppliers> _items;

	public SuppliersViewModel(DatabaseContext databaseContext)
	{
        _items = new List<Suppliers>();
        _databaseContext = databaseContext;
    }

    public async Task OnAppearing()
    {
        IsVisible = true;
        SearchText = string.Empty;
        Items = new ObservableCollection<Suppliers>();
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
            var result = (await _databaseContext.GetAllAsync<Suppliers>())
                        .Where(x => x.Id == id && x.Active == true).FirstOrDefault();
            if (result != null)
            {
                result.Active = false;
                if (await _databaseContext.UpdateItemAsync(result))
                {
                    Items.Remove(rm);
                }
            }
        }
        IsVisible = false;
    }

    [RelayCommand]
    public void TapButtonCancel()
    {
        SearchText = string.Empty;
    }


    [RelayCommand]
    public async Task TapButtonAddNewRecords() => await Shell.Current.GoToAsync($"FormPage?typeForm=Supplier");

    private void SearchAsync(string query)
    {
        var items = _items.Where(item =>
                (!string.IsNullOrEmpty(item.Name) && item.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(item.Email) && item.Email.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(item.Phone) && item.Phone.Contains(query, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        this.FillItems(items);
    }

    private void FillItems(List<Suppliers> items)
    {        
        Items.Clear();
        foreach (var item in items)
        {
            Items.Add(new Suppliers { Email = item.Email, Phone = item.Phone, Name = item.Name, Id = item.Id });
        }
    }

    private  async Task GetData()
    {
        var messge = string.Empty;
        try
        {
            var result = (await _databaseContext.GetAllAsync<Suppliers>())
                  .Where(x => x.Active == true);
            if (result.Any())
            {
                _items.Clear();
                _items = result.ToList();
                return;
            }
            messge = "I couldn't see the information, have you already tried to create a new record?";
        }
        catch (Exception ex)
        {
            messge = ex.Message;
        }
        Application.Current.MainPage.DisplayAlert("Supplers Page", messge, "OK");
    }
}