using CommunityToolkit.Mvvm.ComponentModel;
using EcommerceApp.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using EcommerceApp.Views;
using System.Text.Json;
using EcommerceApp.Services;

namespace EcommerceApp.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    [ObservableProperty]
    public List<Product> productsList;

    [ObservableProperty]
    public ObservableCollection<Product> filteredProductsList;


    [ObservableProperty]
    public ObservableCollection<Category> categories;

    [ObservableProperty]
    public bool isLoading = true;

    [ObservableProperty]
    public string? searchText;

    private readonly IProductsServices _productsServices;

    public HomeViewModel(IProductsServices productsServices)
    {
        ProductsList = new List<Product>();
        FilteredProductsList = new ObservableCollection<Product>();
        Categories = new ObservableCollection<Category>()
        { 
            new Category {Id=Guid.NewGuid(), Image ="house.png", Title = "Home" },
            new Category {Id=Guid.NewGuid(), Image ="tecno.png", Title = "Electronics" },
            new Category {Id=Guid.NewGuid(), Image ="music.png", Title = "Entertainment" },
            new Category {Id=Guid.NewGuid(), Image ="phone.png", Title = "Electronics" },
            new Category {Id=Guid.NewGuid(), Image ="car.png", Title = "Automotive" },
            new Category {Id=Guid.NewGuid(), Image ="clothes.png", Title = "Apparel" }
        };
        _productsServices = productsServices;
    }

    public async void GetData()
    {
        IsLoading = true;
        try
        {
            FilteredProductsList.Clear();
            ProductsList = await _productsServices.List() ?? new List<Product>();

            if (ProductsList != null)
            {
                foreach (var item in ProductsList)
                {
                    FilteredProductsList.Add(item);
                }
            }             

        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"JSON Error: {jsonEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General Error: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task Description(Guid Id)
    {
        await Shell.Current.GoToAsync($"ProductPage?productId={Id}");
    }

    partial void OnSearchTextChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            // If there is no text, show all products
            FilteredProductsList.Clear();
            foreach (var product in ProductsList)
            {
                FilteredProductsList.Add(product);
            }
        }
        else
        {
            // Filter products by entered text
            var filtered =  ProductsList.Where(p =>
                (p.Name?.Contains(value, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.Description?.Contains(value, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();

            FilteredProductsList.Clear();
            foreach (var product in filtered)
            {
                FilteredProductsList.Add(product);
            }
        }
    }

    [RelayCommand]
    public void SearchByCategory(string value)
    {
        if(!string.IsNullOrWhiteSpace(value))
        {
            var filtered = ProductsList.Where(p => p.Category == value).ToList() ?? new List<Product>();

            FilteredProductsList.Clear();

            foreach (var product in filtered)
            {
                FilteredProductsList.Add(product);
            }
        }
    }

}
