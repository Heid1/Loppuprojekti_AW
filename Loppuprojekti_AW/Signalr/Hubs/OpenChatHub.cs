using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;

namespace Loppuprojekti_AW.Signalr.Hubs
{
    public class OpenChatHub: Hub
    {
        public async Task SendMessage(string senderName, string message)
        {
            var sendDate = DateTime.Now;
            var sendTime = sendDate.ToString("t");
            var sDate = sendDate.ToString("dd.M.yyyy");
            await Clients.All.SendAsync("ReceiveMessage", senderName, message, sendTime, sDate);
        }
    }
}
