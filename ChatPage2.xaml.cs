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

            // No need to call PopulateContactsSidebar here since it's called in OnAppearing
            // PopulateContactsSidebar();

            hubConnection.On<string>("ReceiveMessage2", (message) =>
            {
                Dispatcher.Dispatch(() =>
                {
                    MessagesStack.Children.Add(new Label { Text = message });
                });
            });


        }

        // This method will be called when the page appears
        protected override void OnAppearing()
        {
            base.OnAppearing();
            PopulateContactsSidebar(); // Ensure the sidebar is populated when the page appears
            PopulateChatRoomList();
            //InitializeHubConnection();
        }

        private void PopulateContactsSidebar()
        {
            // Dummy data for contacts, replace this with real data from your application context
            var contacts = new List<string> { "Alice", "Bob", "Charlie" };

            ContactsStack.Children.Clear();

            foreach (var contactName in contacts)
            {
                var contactButton = new Button { Text = contactName };
                contactButton.Clicked += async (sender, e) =>
                {
                    // Navigate to the chat detail for this contact
                    // Implement ChatDetailPage and its navigation logic as needed
                    await Navigation.PushAsync(new ChatDetailPage(contactName));
                };
                ContactsStack.Children.Add(contactButton);
            }
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
                await hubConnection.SendAsync("SendMessage2", message);
                Dispatcher.Dispatch(() =>
                {
                    MessagesStack.Children.Add(new Label { Text = $"Me: {message}" });
                });
                MessageEntry.Text = string.Empty; // Clear the text box after sending the message
            }
        }


        /*
        private async void InitializeHubConnection()
        {
            if (hubConnection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await hubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    // Handle exceptions from attempting to start the hub connection
                    await DisplayAlert("Connection Error", $"Unable to connect: {ex.Message}", "OK");
                }
            }
        }
        

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            if (hubConnection.State == HubConnectionState.Connected)
            {
                await hubConnection.StopAsync();
            }
        }
        */
        private void DiscButtonClicked(object sender, EventArgs e)
        {

        }
    }
}