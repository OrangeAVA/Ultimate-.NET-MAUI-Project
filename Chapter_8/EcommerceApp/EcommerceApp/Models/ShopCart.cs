using SQLite;

namespace EcommerceApp.Models
{
    public class ShopCart
    {
        [PrimaryKey]
        public Guid Id {  get; set; }
        public string? User_Id { get; set; }
        public string? Items { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public DateTime Created_at {  get; set; }
    }

    public class Item
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Product_Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public int? Quantity { get; set; }
        public string? Image { get; set; }
        public bool IsBuy { get; set; }
    }
}
