using Microsoft.Maui.Controls;
using System;

namespace GUI1
{
    public partial class ChatDetailPage : ContentPage
    {
        public ChatDetailPage(string contactName)
        {
            InitializeComponent();
            this.Title = contactName;
        }
    }
}