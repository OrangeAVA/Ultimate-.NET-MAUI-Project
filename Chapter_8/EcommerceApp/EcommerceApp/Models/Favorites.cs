using SQLite;
using System.Linq;

namespace EcommerceApp.Models
{
    public class Favorites
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Product_Id { get; set; }
    }
}
