using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic; // Add this to use List

namespace GUI1
{
    public partial class ChatPage2 : ContentPage
    {
        HubConnection hubConnection;

        public ChatPage2(HubConnection connection)
        {
            InitializeComponent();
            hubConnection = connection;

            hubConnection.On<string>("ReceiveMessage2", (message) =>
            {
                Dispatcher.Dispatch(() =>
                {
                    MessagesStack.Children.Add(new Label { Text = message });
                });
            });

            PopulateChatRoomList();
        }

        // This method will be called when the page appears
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void PopulateChatRoomList()
        {

            var chatRoomButton = new Button { Text = "Room 1" };
            chatRoomButton.Clicked += async (sender, e) =>
            {
                Application.Current.MainPage = GlobalData.Page1;
            };
            ChatroomStack.Children.Add(chatRoomButton);

            chatRoomButton = new Button { Text = "Room 2" };
            chatRoomButton.Clicked += async (sender, e) =>
            {
                Application.Current.MainPage = GlobalData.Page2;
            };
            chatRoomButton.TextColor = Color.FromRgb(255, 51, 51);
            ChatroomStack.Children.Add(chatRoomButton);

            chatRoomButton = new Button { Text = "Room 3" };
            chatRoomButton.Clicked += async (sender, e) =>
            {
                Application.Current.MainPage = GlobalData.Page3;
            };
            ChatroomStack.Children.Add(chatRoomButton);

            chatRoomButton = new Button { Text = "Clients" };
            chatRoomButton.Clicked += async (sender, e) =>
            {
                Application.Current.MainPage = GlobalData.ConnectedClients;
            };
            ChatroomStack.Children.Add(chatRoomButton);
        }

        private async void OnSendClicked(object sender, EventArgs e)
        {
            var message = MessageEntry.Text;
            if (!string.IsNullOrEmpty(message) && hubConnection.State == HubConnectionState.Connected)
            {
                await hubConnection.SendAsync("SendMessage2", message);
                Dispatcher.Dispatch(() =>
                {
                    MessagesStack.Children.Add(new Label { Text = $"Me: {message}" });
                });
                MessageEntry.Text = string.Empty; // Clear the text box after sending the message
            }
        }
     
        private void DiscButtonClicked(object sender, EventArgs e)
        {

        }
    }
}