using Microsoft.AspNetCore.SignalR.Client;

public class SignalRService
{
    private HubConnection _connection;

    public event Action<string, string> OnMessageReceived;

    public SignalRService()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("http://192.168.1.73:5005/chatHub") // use your correct local IP
            .WithAutomaticReconnect()
            .Build();

        // Listen to messages from the server
        _connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            OnMessageReceived?.Invoke(user, message);
        });
    }

    public async Task ConnectAsync()
    {
        if (_connection.State != HubConnectionState.Connected)
        {
            await _connection.StartAsync();
        }
    }

    public async Task SendMessage(string user, string message)
    {
        if (_connection.State == HubConnectionState.Connected)
        {
            await _connection.InvokeAsync("SendMessage", user, message);
        }
    }
}
