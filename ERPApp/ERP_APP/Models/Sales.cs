using SQLite;

namespace ERP_APP.Models
{
    public class Sales
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int fk_Customer { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Decimal Total { get; set; }
    }
}
