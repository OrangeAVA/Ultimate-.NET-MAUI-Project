using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Communication = Microsoft.Maui.ApplicationModel.Communication;
using CommunityToolkit.Mvvm.Input;

namespace MauiTalk.ViewModels;
public partial class ContactsViewModel : BaseViewModel
{
    [ObservableProperty]
    public ObservableCollection<Contact?> contactsList;

    [ObservableProperty]
    public string messeger;

    [ObservableProperty]
    public bool isVisible;

    public ContactsViewModel()
    {
        ContactsList = new ObservableCollection<Contact?>();
        Messeger = String.Empty;
        IsVisible = false;
    }

    public async Task OnAppearing() => await LoadContactsAsync();

    public async Task LoadContactsAsync()
    {
        try
        {
            if (ContactsList.Any())
                return;

            if (await Permissions.RequestAsync<Permissions.ContactsRead>() != PermissionStatus.Granted)
                return;

            IsVisible = true;
            await Task.Run(async () =>
            {
                var contacts = await Communication.Contacts.Default.GetAllAsync();

                if (contacts == null)
                    return;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (var contact in contacts)
                        ContactsList.Add(contact);
                });

            });               
        }
        catch (Exception ex)
        {
            Messeger = ex.Message;
        }
        finally
        {
            IsVisible = false;
        }
    }

    [RelayCommand]
    public async Task TapContact(Contact? contact)
    {
        if(contact == null) 
            return;

        var navigationParameter = new Dictionary<string, object>
        {
            { "Contact", contact }
        };

        await Shell.Current.GoToAsync("MessagesPage", navigationParameter);
    }
}