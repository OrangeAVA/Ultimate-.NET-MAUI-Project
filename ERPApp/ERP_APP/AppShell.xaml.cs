
using ERP_APP.Views;

namespace ERP_APP
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = App.ViewModel;
        }
    }
}
