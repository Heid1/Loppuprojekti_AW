using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        // ----------------------- USER ----------------------------------------------
        public static Enduser GetUserById(int ?Identity)
        {
            MoveoContext db = new MoveoContext();

            var Enduser = db.Endusers.Find(Identity);

            return Enduser;
        }

        public static void EditUser(Enduser Eu)
        {
            MoveoContext db = new MoveoContext();
            var edit = db.Endusers.Find(Eu.Userid);

            edit.Userid = Eu.Userid;
            edit.Username = Eu.Username;
            edit.Birthday = Eu.Birthday;
            edit.Userrole = Eu.Userrole;
            edit.Description = Eu.Description;
            edit.UsersSports = Eu.UsersSports;
            edit.Club = Eu.Club;
            edit.Photo = Eu.Photo;

            db.SaveChanges();
        }

        public static void DeleteProfile(Enduser Eu)
        {
            MoveoContext db = new MoveoContext();

            var edit = db.Endusers.Find(Eu.Userid);

            db.Remove(edit);
            db.SaveChanges();
        }

        // ----------------------- POSTS ----------------------------------------------

        //hae yleisimmät postit lajin mukaan (tämä on sanapilveä varten)
        public List<Sport> GetPostsByPrevalence()
        {
            var prevalencelist = db.Posts
                                    .AsEnumerable()
                                    .GroupBy(q => q.Sport)
                                    .OrderByDescending(gp => gp.Count())
                                    .Take(10)
                                    .Select(g => g.Key).ToList();
            return prevalencelist;
        }

        //hae hakusanalla posteja(tämä varsinaista hakua varten)
        public List<Post> GetPostsByCriteria(string criteria)
        {
            var postlist = db.Posts.Include(
                   s => s.Sport).Where(
                   p => p.Description.ToLower().Contains(criteria.ToLower())
                || p.Postname.ToLower().Contains(criteria.ToLower())
                || p.Place.ToLower().Contains(criteria.ToLower())
                || p.Date.ToString().Contains(criteria)
                || p.Sport.Sportname.ToLower().Contains(criteria.ToLower())
                || p.Sport.Description.ToLower().Contains(criteria.ToLower())
               ).ToList();
            return postlist;
        }

        public Post GetPostById(int postid)
        {
            return db.Posts.Find(postid);
        }

        /// <summary>
        /// Hakee kaikki ilmoitukset, jotka käyttäjä on luonut tai liittynyt parametrien arvojen mukaan.
        /// </summary>
        /// <param name="userid">käyttäjä</param>
        /// <param name="organiser">järjestäjä=true, ilmoittautunut=false</param>
        /// <returns></returns>
        public List<Post> GetPostsByAttendance(int userid, bool organiser)
        {
            var attendees = db.Attendees.Where(a => a.Userid == userid && a.Organiser == organiser);
            var posts = db.Posts.Join(attendees, p => p.Postid, a => a.Postid, (p, a) => new Post()).ToList();
            return posts;
        }

        /// <summary>
        /// Luo ilmoituksen ja sen jälkeen Attendee-olion, joka yhdistää ilmoituksen ja ilmoituksen luoja
        /// ja määrittää käyttäjän sen järjestäjäksi.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="post"></param>

        public void CreatePost(int userid, Post post)
        {
            db.Posts.Add(post);
            db.SaveChanges();
            Attendee attendee = new(userid, post.Postid, true);
            db.Attendees.Add(attendee);
            db.SaveChanges();
        }

        public void EditPost(int postid, Post post)
        {
            db.Posts.Find(postid).Postname = post.Postname;
            db.Posts.Find(postid).Sportid = post.Sportid;
            db.Posts.Find(postid).Description = post.Description;
            db.Posts.Find(postid).Attendees = post.Attendees;
            db.Posts.Find(postid).Posttype = post.Posttype;
            db.Posts.Find(postid).Place = post.Place;
            db.Posts.Find(postid).Date = post.Date;
            db.Posts.Find(postid).Duration = post.Duration;
            db.Posts.Find(postid).Privacy = post.Privacy;
            db.Posts.Find(postid).Price = post.Price;
            db.SaveChanges();
        }

        /// <summary>
        /// Poistaa sekä ilmoituksen tekijän että osallistujat Attendeesta 
        /// ja sen jälkeen itse ilmoituksen.
        /// </summary>
        /// <param name="postid"></param>
        public void DeletePost(int postid)
        {
            foreach (var a in db.Attendees.Where(p => p.Postid == postid))
            {
                db.Attendees.Remove(a);
            }
            db.Posts.Remove(db.Posts.Find(postid));
            db.SaveChanges();
        }

        public void AttendPost(int userid, int postid)
        {
            Attendee attendee = new();
            attendee.Userid = userid;
            attendee.Postid = postid;
            attendee.Organiser = false;
            db.Attendees.Add(attendee);
            db.SaveChanges();
        }

        public void DeleteAttendingPost(int userid, int postid)
        {
            db.Attendees.Remove(db.Attendees.Where(a => a.Userid == userid && a.Postid == postid).FirstOrDefault());
            db.SaveChanges();
        }

        // ----------------------- SPORTS ----------------------------------------------

        public void CreateSport(Sport sport)
        {
            db.Sports.Add(sport);
            db.SaveChanges();
        }

        public Sport GetSportById(int sportid)
        {
            return db.Sports.Find(sportid);
        }

        public List<Sport> GetAllSports()
        {
            return db.Sports.ToList();
        }

        public void DeleteSport(int sportid)
        {
            db.Sports.Remove(db.Sports.Find(sportid));
            db.SaveChanges();
        }

        public void EditSport(int sportid, Sport sport)
        {
            db.Sports.Find(sportid).Sportname = sport.Sportname;
            db.Sports.Find(sportid).Description = sport.Description;
            db.SaveChanges();
        }


        // ----------------------- MESSAGES ----------------------------------------------

        /// <summary>
        /// Returns given user's messages with all other users as a dict where int is
        /// the other user's id and value is the list of messages with the other user.
        /// If the user has no messages empty dict is returned.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>dict of the other party's id as key and as value list of message objects </returns>
        public static Dictionary<int, List<Message>> GetMessagesOfUser(int userId)
        {
            Dictionary<int, List<Message>> usersMessages = new Dictionary<int, List<Message>>();
            MoveoContext db = new MoveoContext();
            var messagesToIds = db.Messages.Where(u => u.Senderid == userId).Select(u => (int)u.Receiverid).ToList();
            var messagesFromIds = db.Messages.Where(u => u.Receiverid == userId).Select(u => u.Senderid).ToList();
            if(messagesToIds !=  null || messagesFromIds != null)
            {
                var messagesWithIds = messagesToIds.Union(messagesFromIds).ToList();
                for(int i = 0; i < messagesWithIds.Count; i++)
                {
                    var messages = GetMessagesBetweenUsers(userId, messagesWithIds[i]);
                    usersMessages.Add(messagesWithIds[i], messages);
                }
            }
            return usersMessages;
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
            var messages = db.Messages.Where(u => u.Receiverid == userId1 && u.Senderid == userId2 ||
            u.Receiverid == userId2 && u.Senderid == userId1).OrderBy(m => m.Sendtime).ToList();
            return messages;
        }


    }
}
