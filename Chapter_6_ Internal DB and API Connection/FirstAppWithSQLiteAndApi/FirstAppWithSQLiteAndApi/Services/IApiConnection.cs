using FirstAppWithSQLiteAndApi.Models;
using Refit;

namespace FirstAppWithSQLiteAndApi.Services
{
    public interface IApiConnection
    {
        [Get("/posts/")]
        Task<List<Post>> GetPostListAsync();
    }
}
