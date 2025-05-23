using SQLite;

namespace EcommerceApp.Models
{
    public class Users
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public bool ActiveSession { get; set; }
    }
}
