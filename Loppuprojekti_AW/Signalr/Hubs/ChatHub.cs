using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;



using System.Web;
using Microsoft.AspNetCore.Http;

namespace Loppuprojekti_AW.Signalr.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override Task OnConnected()
        {
            // Get UserID. Assumed the user is logged before connecting to chat and userid is saved in session.
            string UserID = (string)HttpContext.Current.Session["userid"];
          
            // Get ChatHistory and call the client function. See below
            this.GetHistory(UserID);

            // Get ConnID
            string ConnID = Context.ConnectionId;

            // Save them in DB. You got to create a DB class to handle this. (Optional)
            DB.UpdateConnID(UserID, ConnID);
        }

        private void GetHistory(UserID)
        {
            // Get Chat History from DB. You got to create a DB class to handle this.
            string History = DB.GetChatHistory(UserID);

            // Send Chat History to Client. You got to create chatHistory handler in Client side.
            Clients.Caller.chatHistory(History);
        }

        // This method is to be called by Client 
        public void Chat(string Message)
        {
            // Get UserID. Assumed the user is logged before connecting to chat and userid is saved in session.
            string UserID = (string)HttpContext.Current.Session["userid"];

            // Save Chat in DB. You got to create a DB class to handle this
            DB.SaveChat(UserID, Message);

            // Send Chat Message to All connected Clients. You got to create chatMessage handler in Client side.
            Clients.All.chatMessage(Message);
        }
    }
}
