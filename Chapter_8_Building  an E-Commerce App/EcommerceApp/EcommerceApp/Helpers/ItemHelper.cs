using EcommerceApp.Models;
namespace EcommerceApp.Helpers
{
    public class ItemHelper
    {
        public static Item CreateItemFromProduct(Product product)
        {
            return new Item
            {
                Id = Guid.NewGuid(),
                Product_Id = product.Id.ToString(),
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Stock,
                Image = product.Image
            };
        }
    }
}
