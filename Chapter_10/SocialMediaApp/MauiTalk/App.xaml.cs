namespace MauiTalk
{
    public partial class App : Application
    {
        private SignalRService _signalRService;
        public App(SignalRService signalRService)
        {
            InitializeComponent();
            MainPage = new AppShell();
            _signalRService = signalRService;
            this.startChat();
        }

        private void startChat()
        {
            _ = _signalRService.ConnectAsync();
        }
    }
}
