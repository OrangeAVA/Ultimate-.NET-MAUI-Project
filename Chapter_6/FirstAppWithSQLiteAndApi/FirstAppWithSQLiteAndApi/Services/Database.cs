using Microsoft.Data.Sqlite;
using FirstAppWithSQLiteAndApi.Models;

namespace FirstAppWithSQLiteAndApi.Services
{
    public interface IDatabase
    {
        Task<List<Post>> GetPostsAsync();
        Task<int> SavePostAsync(Post post);
        Task<int> DeletePostAsync(int id);
    }

    public class Database : IDatabase
    {
        private readonly string _dbPath;

        public Database(string dbPath)
        {
            _dbPath = dbPath;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            //DropPostTable();
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Post (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    Title TEXT NOT NULL,
                    Body TEXT NOT NULL
                );";
                command.ExecuteNonQuery();
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            var posts = new List<Post>();
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Post";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var nota = new Post
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        Title = reader.GetString(2),
                        Body = reader.GetString(3),
                    };
                    posts.Add(nota);
                }
            }
            return posts;
        }

        public async Task<int> SavePostAsync(Post post)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            await connection.OpenAsync();
            var command = connection.CreateCommand();

           if(post.Id == 0)
            {
                // Insert
                command.CommandText = @"
                    INSERT INTO Post (UserId, Title, Body) 
                    VALUES ($userId, $title, $body); 
                ";
                command.Parameters.AddWithValue("$userId", post.UserId);
                command.Parameters.AddWithValue("$title", post.Title);
                command.Parameters.AddWithValue("$body", post.Body);
                return await command.ExecuteNonQueryAsync();
            }
            else
            {
                // Update
                command.CommandText = @"UPDATE Post SET 
                    UserId = $userId, Title = $title, Body = $body WHERE Id = $id;";
                command.Parameters.AddWithValue("$userId", post.UserId);
                command.Parameters.AddWithValue("$title", post.Title);
                command.Parameters.AddWithValue("$body", post.Body);
                command.Parameters.AddWithValue("$id", post.Id);
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> DeletePostAsync(int id)
        {
            //Deleted
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Post WHERE Id = $id;";
            command.Parameters.AddWithValue("$id", id);
            return command.ExecuteNonQuery();
        }

        private void DropPostTable()
        {
            //Drop Table
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "DROP TABLE IF EXISTS Post;";
            command.ExecuteNonQuery();
        }
    }
}
