

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTalk.Models;
using MauiTalk.Data;
using System.Collections.ObjectModel;
using System.Text.Json;
using System;

namespace MauiTalk.ViewModels;

public partial class MessagesViewModel : BaseViewModel, IQueryAttributable
{
    private SignalRService _signalRService;

    private DatabaseContext _databaseContext;

   private UserModel _uSer;
    private int _idContact;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("Contact") && query["Contact"] is Contact contact)
        {
            DisplayName = contact.DisplayName ?? "?????";
            PhoneNumber = contact.Phones?.FirstOrDefault();
        }
        else if (query.ContainsKey("Open") && query["Open"] is ContactData chat)
        {
            if(chat != null && chat.PhoneNumer != null)
            {
                DisplayName = chat.DisplayName ?? "?????";
                PhoneNumber = new ContactPhone(){ PhoneNumber = chat.PhoneNumer ?? string.Empty} ;
            }
        }
        Messages.Clear();
        _idContact = 0;
        _ = ItsContact();
    }

    [ObservableProperty]
    public string? messageText;

    [ObservableProperty]
    public string? displayName;

    [ObservableProperty]
    public ContactPhone? phoneNumber;

    [ObservableProperty]
    public ObservableCollection<MessageModel> messages;

    public MessagesViewModel(SignalRService signalRService, DatabaseContext databaseContext)
    {
        Messages = new ObservableCollection<MessageModel>();
        _databaseContext = databaseContext;
        MessageText = string.Empty;
        _signalRService = signalRService;

        // Connects to the server
        Task.Run(async () => await _signalRService.ConnectAsync());

        // Subscribe to incoming messages
        _signalRService.OnMessageReceived += (user, message) =>
        {
            if (_uSer?.UserName == null || user == _uSer.UserName || user.Contains("Post:")) return;

            var newMsg = new MessageModel { Text = $"{user}: {message}", IsMine = false };
            string json = JsonSerializer.Serialize(newMsg);
            _ = Task.Run(async () =>
            {
                try
                {
                    await SavedMsg(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving message: {ex.Message}");
                }
            });

            if (user == displayName)
            {
                MainThread.BeginInvokeOnMainThread(() => Messages.Add(newMsg));
            }
        };

    }

    private bool cont = true;

    [RelayCommand]
    public async Task SendMessage()
    {
        try
        {
            if (_idContact == 0)
            {
                Application.Current.MainPage.DisplayAlert("Warning", "The user is not registered", "OK");
                return;
            }
            if (!string.IsNullOrWhiteSpace(MessageText))
            {
                if (_signalRService == null)
                {
                    Console.WriteLine("SignalRService is not initialized.");
                    return;
                }
                await _signalRService.SendMessage(_uSer.UserName, MessageText);
                var newMsg = new MessageModel { Text = $"Me: {MessageText}", IsMine = true };
                Messages.Add(newMsg);
                string json = JsonSerializer.Serialize(newMsg);
                await SavedMsg(json);
                MessageText = string.Empty;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task SavedMsg(string json)
    {
        try
        {
            if (await _databaseContext.AddItemAsync(new MessagesModel()
            {
                FkContactApp = _idContact,
                Viewed = false,
                Message = json,
                Date = DateTime.Now
            }))
            {
                Console.WriteLine("Msg successfully created!");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private async Task ItsContact()
    {
        try
        {

            _uSer = (await _databaseContext.GetAllAsync<UserModel>())
               .Where(x => x.IsActive == true).FirstOrDefault();

            var contact = (await _databaseContext.GetAllAsync<ContactsAppModel>())
                .Where(x => x.PhoneNumber == PhoneNumber?.PhoneNumber && x.FkUser == _uSer.Id).FirstOrDefault();
            //If I can't find the contactAPP, I add it to the contact list
            if (contact == null)
            {
                var newContact = new ContactsAppModel()
                {
                    FkUser = _uSer.Id,
                    Name = DisplayName ?? "user",
                    PhoneNumber = PhoneNumber?.PhoneNumber ?? "0"
                };

                if (await _databaseContext.AddItemAsync(newContact))
                {
                    _idContact = newContact.Id;
                    Console.WriteLine("Contact successfully created! -");
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Warning", "invalid operation", "OK");
                    return;
                }
            }
            else
            {
                _idContact = contact.Id;
            }

            int newContactId = (await _databaseContext.GetAllAsync<ContactsAppModel>())
                              .Where(x => x.PhoneNumber == PhoneNumber?.PhoneNumber && 
                              x.FkUser == _uSer.Id).Select(x => x.Id).FirstOrDefault();

            await LoadSavedMessages();

        }
        catch (Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("Exception", ex.Message, "OK");

        }
    }
    private async Task LoadSavedMessages()
    {
        try
        {
            var oldMsg = (await _databaseContext.GetAllAsync<MessagesModel>())
               .Where(x => x.FkContactApp == _idContact).OrderBy(m => m.Date);

            if (oldMsg == null) return;

            foreach(var msg in oldMsg)
            {
                if(msg != null)
                {
                    if(!msg.Viewed)
                    {
                        msg.Viewed = true;
                        if(!await _databaseContext.UpdateItemAsync(msg))
                        {
                            Console.WriteLine($"outdated message: {msg.Id}");
                        }
                    }
                    MessageModel message = JsonSerializer.Deserialize<MessageModel>(msg.Message);
                    var newMsg = new MessageModel { Text = message.Text, IsMine = message.IsMine };
                    Messages.Add(newMsg);
                }
            }
        }
        catch(Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("Exception", ex.Message, "OK");
        }
    }


}