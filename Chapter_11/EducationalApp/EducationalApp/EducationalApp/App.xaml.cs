using EducationalApp.Data;
using EducationalApp.ViewModels;

namespace EducationalApp
{
    public partial class App : Application
    {
        public static AppShellViewModel ViewModel { get; private set; }

        public App()
        {
            InitializeComponent();
            ViewModel = new AppShellViewModel(new DatabaseContext());
            MainPage = new AppShell();
        }
    }
}
