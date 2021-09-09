using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;



using System.Web;

namespace Loppuprojekti_AW.Signalr.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
