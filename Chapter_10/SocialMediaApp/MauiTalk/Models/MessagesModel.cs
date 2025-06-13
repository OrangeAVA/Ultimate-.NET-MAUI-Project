using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTalk.Models
{
    public class MessagesModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int FkContactApp { get; set; }
        public bool Viewed { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; }
    }
}
