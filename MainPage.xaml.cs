using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;

namespace GUI1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            // Initierar gränssnittskomponenter
            InitializeComponent();
        }

        private async void OnConnectClicked(object sender, EventArgs e)
        {
            string ip = IpEntry.Text;
            string port = PortEntry.Text;
            string endpoint = EndpointEntry.Text;
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                username = "Guest";
            }

            string url = $"https://{ip}:{port}/{endpoint}";

            // Konfigurera anslutningen med SignalR
            GlobalData.connection = new HubConnectionBuilder().WithUrl(url, (opts) =>
            {
                opts.HttpMessageHandlerFactory = (message) =>
                {
                    if (message is HttpClientHandler clientHandler)
                        clientHandler.ServerCertificateCustomValidationCallback += // Metod för att fulhacka SSL-verifiering
                            (sender, certificate, chain, sslPolicyErrors) => { return true; };  // TODO: Fixa fungerande på servern
                    return message;
                };
            }).Build();

            try
            {
                await GlobalData.connection.StartAsync();

                await GlobalData.connection.InvokeAsync("Login", username, password);

                GlobalData.Page1 = new ChatPage(GlobalData.connection);
                GlobalData.Page2 = new ChatPage2(GlobalData.connection);
                GlobalData.Page3 = new ChatPage3(GlobalData.connection);
                GlobalData.ConnectedClients = new ClientsPage(GlobalData.connection);
                GlobalData.privates = new PMPage(GlobalData.connection);

                GlobalData.connection.InvokeAsync("SendLoggedInList");

                Application.Current.MainPage = GlobalData.Page1;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Connection failed", ex.Message, "OK");
            }
        }
    }
}
