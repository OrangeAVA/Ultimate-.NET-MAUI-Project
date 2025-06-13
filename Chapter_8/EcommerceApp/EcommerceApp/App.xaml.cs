using EcommerceApp.Views;

namespace EcommerceApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if(Application.Current != null)
                Application.Current.UserAppTheme = AppTheme.Light;

            MainPage = new AppShell();
            //MainPage = new ProductDescriptionView();
        }
    }
}
