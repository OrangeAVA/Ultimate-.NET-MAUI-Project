using SQLite;

namespace ERP_APP.Models
{
    public class Shopping
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string guid { get; set; }
        public int fk_supplier { get; set; }
        public string? Total { get; set; }
        public DateTime Date { get; set; }
    }
}
