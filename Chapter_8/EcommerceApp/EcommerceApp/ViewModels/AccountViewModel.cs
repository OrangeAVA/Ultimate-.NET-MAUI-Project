using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EcommerceApp.Models;
using EcommerceApp.Data;
using EcommerceApp.Services;
using System.Collections.ObjectModel;
using EcommerceApp.Helpers;

namespace EcommerceApp.ViewModels;

public partial class AccountViewModel : BaseViewModel
{
	private readonly DatabaseContext _dateBaseContext;

    private readonly IProductsServices _productsService;

	[ObservableProperty]
	public ObservableCollection<Item> items;

    [ObservableProperty]
    public string? title;

    [ObservableProperty]
    public string? subTitle;

	public AccountViewModel(DatabaseContext dateBaseContext, IProductsServices productsService)
	{
        _dateBaseContext = dateBaseContext;
        _productsService = productsService;
        Items = new ObservableCollection<Item>();
    }

    public async void OnAppearingAsync()
    {
        try
        {
            Items.Clear();
            Title = "Guest";
            SubTitle = "Without logging in";

            var favorities = await _dateBaseContext.GetAllAsync<Favorites>();

            if (favorities == null || !favorities.Any())
                return;

            var products = await _productsService.List();
            if (products == null)
                return;

            foreach (var favorite in favorities)
            {
                if(!string.IsNullOrWhiteSpace(favorite.Product_Id))
                {
                    var id = new Guid(favorite.Product_Id);
                    var product = products.FirstOrDefault(x => x.Id == id);
                    if (product != null)
                    {
                        var item = ItemHelper.CreateItemFromProduct(product);
                        Items.Add(item);
                    }
                }
            }

            //data user
            var result = (await _dateBaseContext.GetAllAsync<Users>())
               .Where(x => x.ActiveSession == true).FirstOrDefault();

            if(result != null)
            {
                Title = result.Name ?? Title;
                SubTitle = result.Email ?? SubTitle;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in {nameof(OnAppearingAsync)}: {ex}");
        }
    }



}