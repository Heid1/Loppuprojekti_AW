using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Web;
using Microsoft.AspNetCore.Http;
using Loppuprojekti_AW.Models;

namespace Loppuprojekti_AW.Signalr.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string senderId, string senderName, string message)
        {
            var sendDate = DateTime.Now;
            var sendTime = sendDate.ToString("t");
            var sDate = sendDate.ToString("dd.M.yyyy");
     
            await Clients.All.SendAsync("ReceiveMessage", senderName, message, sendTime, sDate);
        }
    }
}
