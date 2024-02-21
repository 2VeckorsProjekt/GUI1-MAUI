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
                // Optionally, indicate to the user that they're being logged in as a guest
                // or leave blank and let the server handle it as shown in the Hub modification.
                username = "Guest";


            }
            else
            {
                // Save credentials securely
                await SecureStorage.SetAsync("username", username);
                await SecureStorage.SetAsync("password", password);
            }

            string url = $"wss://{ip}:{port}/{endpoint}";
            var connection = new HubConnectionBuilder().WithUrl(url).Build();

            try
            {
                await connection.StartAsync();

                // Navigate to chat page
                await Navigation.PushAsync(new ChatPage(connection));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Connection failed", ex.Message, "OK");
            }
        }
    }
}
