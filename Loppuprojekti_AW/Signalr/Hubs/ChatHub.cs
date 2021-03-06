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



        //public void saveMessagetoDB(string userId, string message)
        //{
        //    var ctx = new TestEntities1();

        //    var msg = new tbl_Conversation { Msg = message };
        //    ctx.tbl_Conversation.Add(msg);
        //    ctx.SaveChanges();
        //}

     

        //private void GetHistory(string userId)
        //{
        //    // Get Chat History from DB. You got to create a DB class to handle this.

        //    string History = DB.GetChatHistory(userId);

        //    // Send Chat History to Client. You got to create chatHistory handler in Client side.
        //    Clients.Caller.chatHistory(History);
        //}

        //// This method is to be called by Client 
        //public void Chat(string Message)
        //{
        //    // Get UserID. Assumed the user is logged before connecting to chat and userid is saved in session.
        //    string UserID = (string)HttpContext.Current.Session["userid"];

        //    // Save Chat in DB. You got to create a DB class to handle this
        //    DB.SaveChat(UserID, Message);

        //    // Send Chat Message to All connected Clients. You got to create chatMessage handler in Client side.
        //    Clients.All.chatMessage(Message);
        //}
    }
}
