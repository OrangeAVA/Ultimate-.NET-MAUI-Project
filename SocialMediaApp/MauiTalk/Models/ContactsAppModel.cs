using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTalk.Models
{
    public class ContactsAppModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int FkUser { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        //public List<MessagesModel>? Messages { get; set; }
    }

}
