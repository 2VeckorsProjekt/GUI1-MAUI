﻿using Microsoft.AspNetCore.SignalR.Client;
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

            string url = $"wss://{ip}:{port}/{endpoint}";
            var connection = new HubConnectionBuilder().WithUrl(url, (opts) =>
            {
                opts.HttpMessageHandlerFactory = (message) =>
                {
                    if (message is HttpClientHandler clientHandler)
                        // always verify the SSL certificate
                        clientHandler.ServerCertificateCustomValidationCallback += // Metod för att fulhacka SSL-verifiering
                            (sender, certificate, chain, sslPolicyErrors) => { return true; };  // TODO: Fixa fungerande på servern
                    return message;
                };
            }).Build();

            try
            {
                await connection.StartAsync();

                // Enter the username and password after starting the connection
                await connection.InvokeAsync("Login", username, password);


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
