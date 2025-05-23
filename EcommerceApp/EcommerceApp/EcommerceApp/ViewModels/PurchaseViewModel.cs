using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EcommerceApp.Data;
using EcommerceApp.Models;
using System.Text.Json;

namespace EcommerceApp.ViewModels;

public partial class PurchaseViewModel : BaseViewModel
{
    private DatabaseContext _databaseContext;

    [ObservableProperty]
    public string total;

    [ObservableProperty]
    private string recipientName;

    [ObservableProperty]
    private string street;

    [ObservableProperty]
    private string exteriorNumber;

    [ObservableProperty]
    private string interiorNumber;

    [ObservableProperty]
    private string neighborhood;

    [ObservableProperty]
    private string city;

    [ObservableProperty]
    private string state;

    [ObservableProperty]
    private string postalCode;

    [ObservableProperty]
    private string phone;

    private List<Item> _items;

    public PurchaseViewModel (DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task OnAppearing()
    {
        try
        {
            decimal count = 0;
            _items = (await _databaseContext.GetAllAsync<Item>())
                .Where(x => x.IsBuy == false).ToList();
            if (_items != null)
            {
                foreach (var item in _items)
                {
                    if (item.Quantity.HasValue)
                    {
                        count += (decimal)item.Price * (decimal)item.Quantity;
                        item.IsBuy = true;
                    }
                }
                Total = count.ToString();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    [RelayCommand]
    public async Task TapBuy()
    {
        var message = string.Empty;
        // Validate if all fields are filled
        if (string.IsNullOrWhiteSpace(RecipientName) ||
            string.IsNullOrWhiteSpace(Street) ||
            string.IsNullOrWhiteSpace(ExteriorNumber) ||
            string.IsNullOrWhiteSpace(Neighborhood) ||
            string.IsNullOrWhiteSpace(City) ||
            string.IsNullOrWhiteSpace(State) ||
            string.IsNullOrWhiteSpace(PostalCode) ||
            string.IsNullOrWhiteSpace(Phone))
        {
            message = "incomplete form";
        }
        else
        {
            var result = (await _databaseContext.GetAllAsync<Users>())
              .Where(x => x.ActiveSession == true).FirstOrDefault();
            if (result != null)
            {
                decimal parsedTotal;
                if (!decimal.TryParse(Total, out parsedTotal))
                {
                    message = "Invalid total value";
                    Application.Current.MainPage.DisplayAlert("Success", message, "OK");
                    return;
                }

                var newBought = new ShopCart()
                {
                    Id = Guid.NewGuid(),
                    User_Id = result.Id.ToString(),
                    Items = JsonSerializer.Serialize(_items),
                    SubTotal = parsedTotal,
                    Tax = 0.16m,
                    Total = 0.16m * parsedTotal,
                    Created_at = DateTime.UtcNow
                };


                if (await _databaseContext.AddItemAsync<ShopCart>(newBought))
                {
                    foreach (var item in _items)
                    {
                        await _databaseContext.UpdateItemAsync(item);
                    }
                    message = "purchase made successfully";
                    await Shell.Current.GoToAsync("../");
                }
                else
                {
                    message = "The purchase could not be completed correctly";
                }
            }
            else
            {
                message = "The purchase could not be completed correctly";
            }
        }
        Application.Current.MainPage.DisplayAlert("Success", message, "OK");
    }
}

