using CommunityToolkit.Mvvm.ComponentModel;
using EcommerceApp.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using EcommerceApp.Views;
using System.Text.Json;
namespace EcommerceApp.Services
{
    public class ProductsService : IProductsServices
    {

        private List<Product>  products = new List<Product>();
        public async Task<List<Product>?> List()
        {
            await this.GetData();
            return products;
        }

        private async Task GetData()
        {
            products = new List<Product>();
            try
            {
                string fileName = "Products.json";

                // Open the JSON file
                using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                // Set options for the deserializer
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ignore case in properties
                };

                // Deserialize the JSON
                products = JsonSerializer.Deserialize<List<Product>>(json, options) ?? new List<Product>();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    public interface IProductsServices
    {
        Task<List<Product>?> List();
    }
}
