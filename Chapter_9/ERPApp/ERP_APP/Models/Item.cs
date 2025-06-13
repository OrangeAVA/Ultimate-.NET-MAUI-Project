
namespace ERP_APP.Models
{
    public class Item
    {
        public string? Id { get; set; }
        public string? Name { get; set; }    

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }

    public class ItemSupplier
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Total { get; set; }
        public string? Date { get; set; }
    }

    public class ItemSales
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Unit { get; set; }
        public decimal Total { get; set; }
    }
}
