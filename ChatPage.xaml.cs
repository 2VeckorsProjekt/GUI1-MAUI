using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic; // Add this to use List

namespace GUI1
{
    public partial class ChatPage : ContentPage
    {
        HubConnection hubConnection;

        public ChatPage(HubConnection connection)
        {
            InitializeComponent();
            hubConnection = connection;

            hubConnection.On<string>("ReceiveMessage", (message) =>
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

            PopulateChatRoomList();
        }


        private void PopulateChatRoomList()
        {
            ChatroomStack.Children.Clear();

            var chatRoomButton = new Button { Text = "Room 1" };
            chatRoomButton.Clicked += async (sender, e) =>
            {
                try { await Navigation.PushAsync(GlobalData.Page1); }
                catch { }
            };
            ChatroomStack.Children.Add(chatRoomButton);

            chatRoomButton = new Button { Text = "Room 2" };
            chatRoomButton.Clicked += async (sender, e) =>
            {
                try { await Navigation.PushAsync(GlobalData.Page2); }
                catch { }
            };
            ChatroomStack.Children.Add(chatRoomButton);

            chatRoomButton = new Button { Text = "Room 3" };
            chatRoomButton.Clicked += async (sender, e) =>
            {
                try { await Navigation.PushAsync(GlobalData.Page3); }
                catch { }
            };
            ChatroomStack.Children.Add(chatRoomButton);
        }

        private async void OnSendClicked(object sender, EventArgs e)
        {
            var message = MessageEntry.Text;
            if (!string.IsNullOrEmpty(message) && hubConnection.State == HubConnectionState.Connected)
            {
                await hubConnection.SendAsync("SendMessage", message);
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