using SQLite;

namespace ERP_APP.Models
{
    public class ProductsSold
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int fk_Sales { get; set; }
        public int fk_warehouses { get; set; }
        public int Quantity { get; set; }
        public Decimal Total { get; set; }
    }
}
