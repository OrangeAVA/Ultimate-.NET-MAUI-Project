using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EducationalApp.Data;
using EducationalApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalApp.ViewModels;

public partial class AppShellViewModel : BaseViewModel
{
    private DatabaseContext _databaseContext;

    [ObservableProperty]
    private string? userName;

    [ObservableProperty]
    private string? userEmail;

    public AppShellViewModel(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    [RelayCommand]
    public async Task TapButtonSingOut()
    {
        try
        {
            var result = (await _databaseContext.GetAllAsync<Users>())
                  .Where(x => x.ActiveSession == true).FirstOrDefault();
            if (result != null)
            {
                result.ActiveSession = false;
                var update = (await _databaseContext.UpdateItemAsync(result));
                if (update)
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                }
            }
        }
        catch (Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}

