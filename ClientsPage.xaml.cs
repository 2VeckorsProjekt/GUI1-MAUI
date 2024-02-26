using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic; // Add this to use List

namespace GUI1;

public partial class ClientsPage : ContentPage
{
    HubConnection hubConnection;
    List<string> clientList = new List<string>();

    public ClientsPage(HubConnection connection)
    {
        InitializeComponent();
        hubConnection = connection;

        hubConnection.On<string>("ReceiveClientList", (message) =>
        {
            string[] clients = message.Split(';');

            foreach (var item in clients)
            {
                clientList.Add(item);
            }

            UpdateClientList();

        });
        hubConnection.On<string>("ReceiveClientUpdate", (message) =>
        {
            string[] client = message.Split(';');

            if (client[0] == "Disconnected")
            {
                clientList.Remove(client[1]);
            }
            else if (client[0] == "Connected")
            {
                clientList.Add(client[1]);
            } 
            
            UpdateClientList();
        });

        PopulateChatRoomList();
    }

    // This method will be called when the page appears
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    void UpdateClientList()
    {
        MainThread.BeginInvokeOnMainThread(() => {
            MessagesStack.Clear();
            foreach (var item in clientList)
            {
                MessagesStack.Children.Add(new Label { Text = item });
            }
        });
        
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
        chatRoomButton.TextColor = Color.FromRgb(255, 51, 51);
        ChatroomStack.Children.Add(chatRoomButton);
    }
}