using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalApp.Models
{
    public class Favorite
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Course_Id { get; set; }
    }
}
