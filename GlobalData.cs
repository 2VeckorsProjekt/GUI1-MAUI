using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI1;

public static class GlobalData
{
    public static HubConnection connection;

    public static ChatPage Page1;
    public static ChatPage2 Page2;
    public static ChatPage3 Page3;

}
