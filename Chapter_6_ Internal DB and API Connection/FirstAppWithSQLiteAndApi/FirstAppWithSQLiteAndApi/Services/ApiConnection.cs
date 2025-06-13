using FirstAppWithSQLiteAndApi.Models;
using Refit;

namespace FirstAppWithSQLiteAndApi.Services
{
    public class ApiConnection : IApiConnection
    {
        private readonly IApiConnection _apiService;

        public ApiConnection()
        {
            var url = "https://jsonplaceholder.typicode.com";
            _apiService = RestService.For<IApiConnection>(url);
        }

        public async Task<List<Post>> GetPostListAsync()
        {
            return await _apiService.GetPostListAsync();
        }

    }
}
