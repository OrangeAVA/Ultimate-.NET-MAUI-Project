using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EducationalApp.Data;
using EducationalApp.Models;

namespace EducationalApp.ViewModels;

public partial class AccountViewModel : BaseViewModel
{

    [ObservableProperty]
    public string? title;

    [ObservableProperty]
    public string? subTitle;

    private DatabaseContext _databaseContext;

    private Users? _userLogin;

    public AccountViewModel(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task OnAppearingAsync()
    {
        try
        {

            _userLogin = (await _databaseContext.GetAllAsync<Users>())
                .Where(x => x.ActiveSession == true).FirstOrDefault();

            if (_userLogin != null)
            {
                Title = _userLogin.Name;
                SubTitle = _userLogin.Email;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in {nameof(OnAppearingAsync)}: {ex}");
        }
    }

    [RelayCommand]
    public async Task SignOut()
    {
        try
        {
            if (_userLogin != null)
            {
                _userLogin.ActiveSession = false;
                if (await _databaseContext.UpdateItemAsync(_userLogin))
                {
                    await Shell.Current.GoToAsync("//LoginView");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}