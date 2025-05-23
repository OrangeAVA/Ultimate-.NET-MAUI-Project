
using ERP_APP.Models;
using ERP_APP.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static SQLite.SQLite3;

namespace ERP_APP.ViewModels;

public partial class UserViewModel : BaseViewModel
{
    private DatabaseContext _databaseContext;

    private int _idUser;

    [ObservableProperty]
    public string userName;

    [ObservableProperty]
    public string email;

    [ObservableProperty]
    public string pass;

    [ObservableProperty]
    public bool createSession;

    [ObservableProperty]
    public bool isVisibleButtonLogin;

    [ObservableProperty]
    public string? textButtonSaved;

    public UserViewModel(DatabaseContext databaseContext)
	{
        _databaseContext = databaseContext;
        UserName = string.Empty;
        Email = string.Empty;
        Pass = string.Empty;
        CreateSession = false;
        IsVisibleButtonLogin = true;
        TextButtonSaved = "Create Session";
    }

    public async Task OnAppearing()
    {
        try
        {
            UserName = string.Empty;
            Email = string.Empty;
            Pass =string.Empty;
            _idUser = 0;

            var result = (await _databaseContext.GetAllAsync<Users>())
                .Where(x => x.ActiveSession == true).FirstOrDefault();

            if (result != null)
            {
                TextButtonSaved = "Edit";
                UserName = result.Name ?? string.Empty;
                Email = result.Email ?? string.Empty;
                Pass = result.Password ?? string.Empty;
                _idUser = result.Id;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    [RelayCommand]
    public async Task TapSaved()
    {
        try
        {
            var user = string.IsNullOrWhiteSpace(UserName);
            var email = string.IsNullOrWhiteSpace(Email);
            var pass = string.IsNullOrWhiteSpace(Pass);

            if (!user && !email && !pass)
            {
                var newUser = new Users()
                {
                    Name = UserName,
                    Email = Email,
                    Password = Pass,
                    ActiveSession = TextButtonSaved == "Edit" ? true : false
                };

                if (TextButtonSaved != "Edit")
                {
                    var insert = (await _databaseContext.AddItemAsync(newUser));
                    if(insert)
                    {
                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                    else
                    {
                        Application.Current.MainPage.DisplayAlert("Create User", "Faild create user", "OK");
                    }
                }
                else
                {
                    newUser.Id = _idUser;
                    var update = (await _databaseContext.UpdateItemAsync(newUser));
                    if (update)
                    {
                        Application.Current.MainPage.DisplayAlert("Update", "Update succefull", "OK");
                        App.ViewModel.UserName = newUser.Name;
                        App.ViewModel.UserEmail = newUser.Email;
                    }
                    else
                    {//
                        Application.Current.MainPage.DisplayAlert("Update", "Error Update", "OK");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }

}