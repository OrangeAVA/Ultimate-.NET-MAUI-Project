using SQLite;

namespace MauiTalk.Models
{
    public class NewsModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int FK_User { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; }
    }

    public class NewsAppModel
    {
        public int id { get; set; }
        public string? UserName { get; set; }
        public string? Post { get; set; }
        public DateTime Date { get; set; }
    }
}
