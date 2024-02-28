// ChatDetailPage.xaml.cs
using Microsoft.Maui.Controls;
using System;

namespace GUI1
{
    public partial class ChatDetailPage : ContentPage
    {
        public ChatDetailPage(string contactName)
        {
            InitializeComponent();
            this.Title = contactName; // Directly set the title of the page to the contact's name
            // Set any other properties or UI elements as needed
        }
    }
}
