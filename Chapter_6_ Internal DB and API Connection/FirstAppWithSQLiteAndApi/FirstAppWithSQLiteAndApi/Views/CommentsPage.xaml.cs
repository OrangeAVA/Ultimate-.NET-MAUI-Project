using FirstAppWithSQLiteAndApi.ViewModels;
using FirstAppWithSQLiteAndApi.Services;
namespace FirstAppWithSQLiteAndApi.Views;

public partial class CommentsPage : ContentPage
{
    public CommentsPage(CommentsViewModel commentsViewModel)
    {
        InitializeComponent();
        this.BindingContext = commentsViewModel;
    }
}