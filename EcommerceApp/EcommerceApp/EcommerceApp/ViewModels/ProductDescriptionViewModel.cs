

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EcommerceApp.Models;
using EcommerceApp.Data;
using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using EcommerceApp.Services;
using EcommerceApp.Helpers;

namespace EcommerceApp.ViewModels;

public partial class ProductDescriptionViewModel : BaseViewModel, IQueryAttributable
{

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _productId = query["productId"] as string;
    }

    private string? _productId { get; set; }

    [ObservableProperty]
	public ObservableCollection<QuantityItem> quantityItem;

    [ObservableProperty]
    public List<Product> products;

    [ObservableProperty]
    public QuantityItem selectedQuantity;

    [ObservableProperty]
    public string buttonText;

    [ObservableProperty]
    public Product product;

    [ObservableProperty]
    public string heartIcon;

    [ObservableProperty]
    public string productName;

    [ObservableProperty]
    public string productDescription;

    [ObservableProperty]
    public string productImage;

    [ObservableProperty]
    public double productPrice;

    [ObservableProperty]
    public bool isEnabledButton = false;

    private readonly DatabaseContext _dateBaseContext;
    private readonly IProductsServices _productsServices;

    public ProductDescriptionViewModel(DatabaseContext dateBaseContext, IProductsServices productsServices)
	{
        HeartIcon = "heart.png";
        ButtonText = "Add to cart";
        _dateBaseContext = dateBaseContext;
        _productsServices = productsServices;
    }

    public async void ConsultProduct()
    {
        IsEnabledButton = false;
        Products = await _productsServices.List() ?? new List<Product>();
        if (!string.IsNullOrEmpty(_productId) && Products != null)
        {
            var id = Guid.Parse(_productId);
            QuantityItem = new ObservableCollection<QuantityItem>();
            Product = Products.FirstOrDefault(x => x.Id == id) ?? new Product();
            ProductName = Product.Name ?? string.Empty;
            ProductDescription = Product.Description ?? string.Empty;
            ProductImage = Product.Image ?? string.Empty;
            ProductPrice = Product.Price;
            if(Product.Stock > 0)
            {
                for (int i = 1; i < Product.Stock; i++)
                {
                    QuantityItem.Add(new QuantityItem { Name = $"Amount {i}", Amount = i });
                }
            }
            else
            {
                QuantityItem.Add(new QuantityItem { Name = "Exhausted", Amount = 0 });
            }
            this.IsFavorite();
        }
    }

    public async void IsFavorite()
    {
        try
        {
            var idProd = Product.Id.ToString() ?? string.Empty;
            if(!string.IsNullOrWhiteSpace(idProd))
            {
                var favorite = (await _dateBaseContext.GetFileteredAsync<Favorites>(
                    x => x.Product_Id == idProd))
                    .Select(x => (Guid?)x.Id) // Convertir a Nullable<Guid>
                    .FirstOrDefault()?.ToString(); // Usar el operador de acceso condicional


                if (!string.IsNullOrWhiteSpace(favorite))
                {
                    HeartIcon = "heartselect.png";
                }
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message); 
        }
    }

    partial void OnSelectedQuantityChanged(QuantityItem value)
    {
        if (value != null )
        {
            if(SelectedQuantity.Amount != 0)
            IsEnabledButton = true;
        }
    }


    // To add or remove from favorites
    [RelayCommand]
    public async Task HeartIconTap()
    {
        await this.AddOrRemoveToFavorites();
    }
    private async Task AddOrRemoveToFavorites()
    {
        try
        {
            if(HeartIcon.Equals("heart.png"))
            {
                var favorite = new Favorites()
                { 
                    Id = Guid.NewGuid(),
                    Product_Id = Product.Id.ToString()
                };
                if (await _dateBaseContext.AddItemAsync(favorite))
                {
                    HeartIcon = "heartselect.png";
                }
            }
            else
            {
                var idProd = Product.Id.ToString();
                var favorites = (await _dateBaseContext.GetFileteredAsync<Favorites>
                            (x => x.Product_Id == idProd)).Select(x => x.Id).ToList();
                if (favorites is null) return;
                foreach (var item in favorites)
                {
                    if (await _dateBaseContext.DeleteItemByKeyAsync<Favorites>(item))
                    {
                        HeartIcon = "heart.png";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    // To add product to shopping cart
    [RelayCommand]
    public async void TapButton()
    {
        if (!ButtonText.Equals("remove from cart"))
        {
            await this.AddToCart();
        }
        else
        {
            await this.RemoveFromCart();
        }
    }

    private async Task AddToCart()
    {
        try
        {
            var item = ItemHelper.CreateItemFromProduct(Product);
            item.Quantity = SelectedQuantity.Amount;
            item.IsBuy = false;
            if (await _dateBaseContext.AddItemAsync(item))
            {
                ButtonText = "remove from cart";
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task RemoveFromCart()
    {
        try
        {
            var iSRemove = false;
            var idProd = Product.Id.ToString();
            var items = (await _dateBaseContext.GetFileteredAsync<Item>
                        (x => x.Product_Id == idProd)
                     ).Select(x => x.Id).ToList();
            if (items is null) return;
            foreach ( var item in items)
            {
                if(await _dateBaseContext.DeleteItemByKeyAsync<Item>(item))
                {
                    iSRemove = true; 
                }
            }
            if(iSRemove)
                ButtonText = "Add to cart";
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
