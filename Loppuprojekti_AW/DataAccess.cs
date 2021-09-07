using Loppuprojekti_AW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loppuprojekti_AW
{
    public class DataAccess
    {
        public MoveoContext db { get; set; }
        public DataAccess(MoveoContext data)
        {
            db = data;
        }

        public static Enduser GetUserById(int Identity)
        {
            MoveoContext db = new MoveoContext();

            var Enduser = db.Endusers.Find(Identity);

            return Enduser;
        }

        public static void EditUser(Enduser Eu)
        {
            MoveoContext db = new MoveoContext();

            var muokattava = db.Endusers.Find(Eu.Userid);

            muokattava.Userid = Eu.Userid;
            muokattava.Username = Eu.Username;
            muokattava.Birthday = Eu.Birthday;
            muokattava.Userrole = Eu.Userrole;
            muokattava.Description = Eu.Description;
            muokattava.UsersSports = Eu.UsersSports;
            muokattava.Club = Eu.Club;
            muokattava.Photo = Eu.Photo;

            db.SaveChanges();
        }

        /// <summary>
        /// Returns given user's messages with all other users as a dict where int is
        /// the other user's id and value is the list of messages with the other user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Dictionary<int, List<Message>> GetMessagesOfUser(int userId)
        {
            MoveoContext db = new MoveoContext();
            var messagesToIds = db.Messages.Where(u => u.Senderid == userId).Select(u => (int)u.Receiverid).ToList();
            var messagesFromIds = db.Messages.Where(u => u.Receiverid == userId).Select(u => u.Senderid).ToList();
            var messageswithIds = messagesToIds.Union(messagesFromIds).ToList();
            return null;

        }

        /// <summary>
        /// Returns messages between two users bases on their ids.
        /// </summary>
        /// <param name="userId1">Id of first user..</param>
        /// <param name="userId2">Id of second user.</param>
        /// <returns>list of messages between the two users.</returns>
        public static List<Message> GetMessagesBetweenUsers(int userId1, int userId2)
        {
            MoveoContext db = new MoveoContext();
            var messages = db.Messages.Where(u => u.Receiverid == userId1 && u.Senderid == userId2 || u.Receiverid == userId2 && u.Senderid == userId1).OrderBy(m => m.Sendtime).ToList();
            return messages;
        }


    }
}
