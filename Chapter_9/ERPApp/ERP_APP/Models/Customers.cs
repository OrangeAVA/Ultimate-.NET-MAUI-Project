using SQLite;
namespace ERP_APP.Models
{
    public class Customers
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool Active { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
