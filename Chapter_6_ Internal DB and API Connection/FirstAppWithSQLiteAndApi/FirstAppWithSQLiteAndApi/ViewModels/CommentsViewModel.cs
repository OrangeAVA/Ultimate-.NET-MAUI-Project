using System.Collections.ObjectModel;
using FirstAppWithSQLiteAndApi.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using FirstAppWithSQLiteAndApi.Services;



namespace FirstAppWithSQLiteAndApi.ViewModels
{
    public partial  class CommentsViewModel : ObservableObject
    {
        private readonly IDatabase _database;

        private readonly IApiConnection _connection;

        [ObservableProperty]
        private ObservableCollection<Post>? _posts;

        [ObservableProperty]
        private bool _activate;

        public CommentsViewModel(IDatabase database, IApiConnection connection)
        {
            _database = database;
            _connection = connection;
            Task task = LoadPost();
        }

        private async Task LoadPost()
        {
            Activate = true;
            try
            {
                var posts = await _connection.GetPostListAsync();
                foreach (var post in posts)
                {
                   await OnSavePost(post);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Activate = false;
        }

        private async Task OnSavePost(Post post)
        {
            await _database.SavePostAsync(post);
            var list = await _database.GetPostsAsync();
            Posts = new ObservableCollection<Post>(list);
        }
    }
}
