using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic; // Add this to use List

namespace GUI1
{
    public partial class PMPage : ContentPage
    {
        HubConnection hubConnection;
        Dictionary<string, List<string>> messages = new Dictionary<string, List<string>>();
        string current = "";

        public PMPage(HubConnection connection)
        {
            InitializeComponent();
            hubConnection = connection;

            hubConnection.On<string>("ReceiveClientList", (message) =>
            {
                string[] clients = message.Split(';');

                foreach (var client in clients)
                {
                    if (client != string.Empty)
                    {
                        
                        messages[client]= new List<string>();

                        var userButton = new Button { Text = client };
                        userButton.Clicked += async (sender, e) =>
                        {
                            current = client;
                            UpdateMessegeField();
                        };
                        ConvoStack.Children.Add(userButton);
                    }
                }
            });
            hubConnection.On<string>("ReceiveClientUpdate", (message) =>
            {
                string[] client = message.Split(';');
                string action = client[0];
                string user = client[1];
                
                if (action == "Connected")
                {
                    MainThread.BeginInvokeOnMainThread(() => {
                        messages[user] = new List<string>();

                        var userButton = new Button { Text = user };
                        userButton.Clicked += async (sender, e) =>
                        {
                            current = user;
                            UpdateMessegeField();
                        };
                        ConvoStack.Children.Add(userButton);
                    });

                }                
            });

            hubConnection.On<string>("ReceivePM", (message) =>
            {
                string[] mess = message.Split(';');
                string sender = mess[0];
                string content = mess[1];

                // Lägg till i DS
                messages[sender].Add($"{sender}: {content}");

                // Uppdatera display

                UpdateMessegeField();
            });

            PopulateChatRoomList();
        }

        void UpdateMessegeField()
        {
            MainThread.BeginInvokeOnMainThread(() => {
                MessagesStack.Clear();
                MessagesStack.Children.Add(new Label { Text = $"Chat history with {current}" });
                foreach (var item in messages[current])
                {
                    MessagesStack.Children.Add(new Label { Text = item });
                }
            });
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

            chatRoomButton = new Button { Text = "PMs" };
            chatRoomButton.Clicked += async (sender, e) =>
            {
                Application.Current.MainPage = GlobalData.privates;
            };
            chatRoomButton.TextColor = Color.FromRgb(255, 51, 51);
            ChatroomStack.Children.Add(chatRoomButton);

           
        }

        private async void OnSendClicked(object sender, EventArgs e)
        {
            

            var input = MessageEntry.Text;
            if (!string.IsNullOrEmpty(input) && hubConnection.State == HubConnectionState.Connected)
            {
                string messageToServer = $"{current};{input}";
                await hubConnection.SendAsync("RelayPM", messageToServer);

                MainThread.BeginInvokeOnMainThread(() => {

                    MessagesStack.Children.Add(new Label { Text = $"Me: {input}" });
                    messages[current].Add($"Me: {input}");

                    MessageEntry.Text = string.Empty; // Clear the text box after sending the message
                });

                
            }
        }
        private void DiscButtonClicked(object sender, EventArgs e)
        {

        }
    }
}