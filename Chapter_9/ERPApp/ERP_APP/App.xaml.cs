
using ERP_APP.ViewModels;
using ERP_APP.Data;

namespace ERP_APP
{
    public partial class App : Application
    {
        public static MainViewModel ViewModel { get; private set; }
        public App()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = AppTheme.Light;
            ViewModel = new MainViewModel(new DatabaseContext());
            MainPage = new AppShell();
        }
    }
}
