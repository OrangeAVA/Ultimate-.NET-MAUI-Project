using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using EcommerceApp.Models;
using EcommerceApp.Data;
using CommunityToolkit.Mvvm.Input;

namespace EcommerceApp.ViewModels;

public partial class ShoppingCartViewModel : BaseViewModel
{
    [ObservableProperty]
    public ObservableCollection<Item>? items;

    private readonly DatabaseContext _dateBaseContext;

    public ShoppingCartViewModel(DatabaseContext datebaseContext)
    {
        Items = new ObservableCollection<Item>();
        _dateBaseContext = datebaseContext;
    }
    //Event when loading the page
    public async void OnAppearing()
	{
        try
        {
            Items = new ObservableCollection<Item>();
            var data = (await _dateBaseContext.GetAllAsync<Item>())
                .Where(i => i.IsBuy == false).ToList();
            if(data != null && data.Any())
            {
               foreach(var i in data)
               {
                    Items.Add(i);
               }
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
       
    }
    //Event for the buy button
    [RelayCommand]
    public async Task BuyProccess()
    {
        var result = (await _dateBaseContext.GetAllAsync<Users>())
               .Any(x => x.ActiveSession == true);
        if(result)
        {
            if (Items == null || !Items.Any())
            {
                Application.Current.MainPage.DisplayAlert("Success", "there are no products", "OK");
                return;
            }
            await Shell.Current.GoToAsync("PurchasePage");
        }
        else
        {
            await Shell.Current.GoToAsync("UserPage");
        }
    }
    //Delete item from shopping cart and from the table
    [RelayCommand]
    public async Task DeletedItem(Item item)
    {
        try
        {
            if (item is null || Items is null) return;
            var id = item.Id;
            if (await _dateBaseContext.DeleteItemByKeyAsync<Item>(id))
            {
                Items.Remove(item);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}