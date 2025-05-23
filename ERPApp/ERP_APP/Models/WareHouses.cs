using SQLite;

namespace ERP_APP.Models
{
    public class WareHouses
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int fk_Shopping { get; set; }
        public string? Name { get; set; }
        public int Unit { get; set; }
        public Decimal PurchasePrice { get; set; }
        public Decimal Price { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
