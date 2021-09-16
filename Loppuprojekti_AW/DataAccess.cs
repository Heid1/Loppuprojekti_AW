using Google.Maps;
using Google.Maps.Geocoding;
using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        public bool CheckUserAuthority(string username, string password)
        {
            if (db.Endusers.Where(k => k.Username == username).FirstOrDefault() != null && db.Endusers.Where(k => k.Username == username).FirstOrDefault().Password == password)
            {
                return true;
            }
            return false;
        }

        public void CreateUser(Enduser enduser)
        {
            db.Endusers.Add(enduser);
            db.SaveChanges();
        }

        public bool GetUsersByEmail(string email)
        {
            if (db.Endusers.Where(eu => eu.Email == email).FirstOrDefault() != null) //jos kannasta löytyy vastaavaavuus eli ei null
            {
                return true; //palauta true, löytyy
            }
            return false;
        }

        public Enduser GetUserById(int? userid)
        {
            var Enduser = db.Endusers.Find(userid);
            return Enduser;
        }

        public void EditUser(Enduser enduser)
        {
            var edit = db.Endusers.Find(enduser.Userid);
            edit.Userid = enduser.Userid;
            edit.Username = enduser.Username;
            edit.Birthday = enduser.Birthday;
            //edit.Userrole = Eu.Userrole;
            edit.Description = enduser.Description;
            edit.UsersSports = enduser.UsersSports;
            edit.Club = enduser.Club;
            edit.Photo = enduser.Photo;
            db.SaveChanges();
        }

        public void DeleteUser(Enduser enduser)
        {
            var Userdelete = db.Endusers.Find(enduser.Userid);
            db.Remove(Userdelete);
            db.SaveChanges();
        }

        public string GetCurrentPhotoUrl(int userid)
        {
            return db.Endusers.Find(userid).Photo;
        }

        public Dictionary<int, string> GetAllUsers()
        {
            List<Enduser> users = db.Endusers.ToList();
            Dictionary<int, string> usersDict = new Dictionary<int, string>();
            foreach (var user in users)
            {
                usersDict.Add(user.Userid, user.Username);
            }
            return usersDict;
        }

        // ----------------------- POSTS ----------------------------------------------

        public List<Post> GetAllPosts()
        {
            return db.Posts.Include(s => s.Sport).Include(a => a.AttendeesNavigation).OrderBy(p => p.Date).ToList();
        }

        //hae yleisimmät lajit post määrän mukaisesti (tämä on sanapilveä varten)
        //public List<Sport> GetSportsByPrevalence()
        //{
        //    var prevalencelist = (from t in db.Sports
        //                         group t.Posts by t.Sportname into g
        //                         select t).ToList();

        //    return prevalencelist;
        //}

        //hae hakusanalla posteja(tämä varsinaista hakua varten)
        public List<Post> GetPostsByCriteria(string criteria)
        {
            var postlist = db.Posts
                .Include(s => s.Sport)
                .Include(a => a.AttendeesNavigation)
                .Where(p => p.Description.ToLower().Contains(criteria.ToLower())
                || p.Postname.ToLower().Contains(criteria.ToLower())
                || p.Place.ToLower().Contains(criteria.ToLower())
                || p.Date.ToString().Contains(criteria)
                || p.Sport.Sportid.ToString().Contains(criteria.ToLower())
                || p.Sport.Sportname.ToLower().Contains(criteria.ToLower())
                || p.Sport.Description.ToLower().Contains(criteria.ToLower()))
                .OrderBy(p=>p.Date)
               .ToList();
            return postlist;
        }

        public List<Post> GetPostsByFavouriteSport(int userid)
        {
            var postlist = (from p in db.Posts
                           join s in db.Sports on p.Sportid equals s.Sportid
                           join us in db.UsersSports on s.Sportid equals us.Sportid
                           where us.Userid == userid
                           select p)
                           .Include(s => s.Sport)
                           .Include(a => a.AttendeesNavigation)
                           .OrderBy(p => p.Date)
                           .ToList();
                return postlist;
        }

        /// <summary>
        /// Hakee kaikki ilmoitukset, jotka käyttäjä on luonut tai liittynyt parametrien arvojen mukaan.
        /// </summary>
        /// <param name="userid">käyttäjä</param>
        /// <param name="organiser">järjestäjä=true, ilmoittautunut=false</param>
        /// <returns>Lista ilmoituksista</returns>
        public List<Post> GetPostsByAttendance(int userid, bool organiser)
        {
            var posts = (from p in db.Posts
                         join a in db.Attendees on p.Postid equals a.Postid
                         where a.Userid == userid && a.Organiser == organiser
                         select p)
                         .Include(s => s.Sport)
                         .Include(a => a.AttendeesNavigation)
                         .OrderBy(p => p.Date)
                         .ToList();
            return posts;
        }

        public Post GetPostById(int postid)
        {
            return db.Posts.Find(postid);
        }

        public List<Post> GetUserPosts(int? userid)
        {
            DateTime now = DateTime.Now;
            DateTime future = DateTime.Now.AddHours(24);
            bool organiser = true;
            var organizingtoday = (from p in db.Posts
                                  join s in db.Sports on p.Sportid equals s.Sportid
                                  join a in db.Attendees on p.Postid equals a.Postid
                                  where a.Userid == userid && a.Organiser == organiser
                                  where p.Date <= future && p.Date >= now
                                  select p)
                                  .Include(s => s.Sport)
                                  .Include(a => a.AttendeesNavigation)
                                  .OrderBy(p => p.Date)
                                  .ToList();
            return organizingtoday;
        }

        public List<Post> GetOtherPostsByAttendanceToday(int? userid)
        {
            DateTime now = DateTime.Now;
            DateTime future = DateTime.Now.AddHours(24);
            bool organiser = false;
            var attendingtoday = (from p in db.Posts
                                  join a in db.Attendees on p.Postid equals a.Postid
                                  join s in db.Sports on p.Sportid equals s.Sportid
                                  where a.Userid == userid && a.Organiser == organiser
                                  where p.Date <= future && p.Date >= now
                                  select p)
                                  .Include(s => s.Sport)
                                  .Include(a=>a.AttendeesNavigation)
                                  .OrderBy(p => p.Date)
                                  .ToList();
            return attendingtoday;
        }

        public void CreatePost(int userid, Post post)
        {
            db.Posts.Add(post);
            db.SaveChanges();
            Attendee attendee = new(userid, post.Postid, true);
            db.Attendees.Add(attendee);
            db.SaveChanges();
        }

        public void EditPost(Post post)
        {
            var postid = post.Postid;
            db.Posts.Find(postid).Postname = post.Postname;
            db.Posts.Find(postid).Sportid = post.Sportid;
            db.Posts.Find(postid).Description = post.Description;
            db.Posts.Find(postid).Attendees = post.Attendees;
            //db.Posts.Find(postid).Posttype = post.Posttype;
            db.Posts.Find(postid).Place = post.Place;
            db.Posts.Find(postid).Date = post.Date;
            db.Posts.Find(postid).Duration = post.Duration;
            //db.Posts.Find(postid).Privacy = post.Privacy;
            db.Posts.Find(postid).Price = post.Price;
            db.Posts.Find(postid).Latitude = ReturnCoordinates(db.Posts.Find(postid).Place, true);
            db.Posts.Find(postid).Longitude = ReturnCoordinates(db.Posts.Find(postid).Place, false);
            db.SaveChanges();
        }

        public void DeletePost(int postid)
        {
            foreach (var a in db.Attendees.Where(p => p.Postid == postid))
            {
                db.Attendees.Remove(a);
            }
            var post = db.Posts.Find(postid);
            db.Posts.Remove(post);
            db.SaveChanges();
        }

        // ----------------------- ATTENDING ----------------------------------------------

        public void AttendPost(int userid, int postid)
        {
            Attendee attendee = new();
            attendee.Userid = userid;
            attendee.Postid = postid;
            attendee.Organiser = false;
            db.Attendees.Add(attendee);
            db.SaveChanges();
        }

        public void CancelAttendance(int userid, int postid)
        {
            var attendee = db.Attendees.Where(a => a.Userid == userid && a.Postid == postid).FirstOrDefault();
            db.Attendees.Remove(attendee);
            db.SaveChanges();
        }

        public List<Attendee> GetAttendances(int? userid)
        {
            if (userid==null)
            {
                return null;
            }
            return db.Attendees.Where(u => u.Userid == userid).ToList();
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

        public void EditSport(Sport sport)
        {
            var sportid = sport.Sportid;
            db.Sports.Find(sportid).Sportname = sport.Sportname;
            db.Sports.Find(sportid).Description = sport.Description;
            db.SaveChanges();
        }

        public void AddSportToFavourites(UsersSport userssport)
        {
            db.UsersSports.Add(userssport);
            db.SaveChanges();
        }

        public void RemoveSportFromFavourites(int userid, int sportid)
        {
            var userssport = db.UsersSports.Where(us => us.Userid == userid && us.Sportid == sportid).FirstOrDefault();
            db.UsersSports.Remove(userssport);
            db.SaveChanges();
        }

        public List<UsersSport> FindUsersSports(int? userid)
        {
            if (userid == null)
            {
                return null;
            }
            return db.UsersSports.Where(s => s.Userid == userid).ToList();
        }

        // ----------------------- MESSAGES ----------------------------------------------

        /// <summary>
        /// Returns given user's messages with all other users as a dict where int is
        /// the other user's id and value is the list of messages with the other user.
        /// If the user has no messages empty dict is returned.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>dict of the other party's id as key and as value list of message objects </returns>
        public Dictionary<int, List<Message>> GetMessagesOfUser(int userId)
        {
            Dictionary<int, List<Message>> usersMessages = new Dictionary<int, List<Message>>();
 
            var messagesSentToIds = db.Messages.Where(u => u.Senderid == userId).Select(u => (int)u.Receiverid).ToList();
            var messagesReceivedFromIds = db.Messages.Where(u => u.Receiverid == userId).Select(u => u.Senderid).ToList();
            
            if(messagesSentToIds !=  null || messagesReceivedFromIds != null)
            {
                var messagesWithIds = messagesSentToIds.Union(messagesReceivedFromIds).ToList();
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
        public List<Message> GetMessagesBetweenUsers(int userId1, int userId2)
        {
      
            var messages = db.Messages.Where(u => (u.Receiverid == userId1 && u.Senderid == userId2) || (u.Receiverid == userId2 && u.Senderid == userId1)).OrderBy(m => m.Sendtime).ToList();
            return messages;
        }

        /// <summary>
        /// Returns a list of enduser objects the user specified by the userid has messaged with
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Enduser> GetUsersMessagedWith(int userId)
        {

            var messagesSentToIds = db.Messages.Where(u => u.Senderid == userId).Select(u => (int)u.Receiverid).ToList();
            var messagesReceivedFromIds = db.Messages.Where(u => u.Receiverid == userId).Select(u => u.Senderid).ToList();
            List<int> messagesWithIds = new List<int>();
            List<Enduser> messagesWithUsers = new List<Enduser>();

            if (messagesSentToIds != null || messagesReceivedFromIds != null)
            {
                messagesWithIds = messagesSentToIds.Union(messagesReceivedFromIds).ToList();
            }

            messagesWithUsers = db.Endusers.Where(u => messagesWithIds.Contains(u.Userid)).ToList();
            return messagesWithUsers;
        }

        // ----------------------- MAP ----------------------------------------------

        public decimal ReturnCoordinates(string address, bool lat)
        {
            var request = new GeocodingRequest();
            request.Address = address;
            var response = new GeocodingService().GetResponse(request);

            if (response.Status == ServiceResponseStatus.Ok && response.Results.Count() > 0)
            {
                var result = response.Results.First();
                decimal latitude = (decimal)result.Geometry.Location.Latitude;
                decimal longitude = (decimal)result.Geometry.Location.Longitude;
                if (lat == true)
                {
                    return latitude;
                }
                else
                {
                    return longitude;
                }
            }
            else
            {
                return 0;
            }
        }

        public string ReturnPostObjects()
        {
            var postobject = (from p in db.Posts
                              join a in db.Attendees
                              on p.Postid equals a.Postid
                              join s in db.Sports
                              on p.Sportid equals s.Sportid
                              join u in db.Endusers
                              on a.Userid equals u.Userid
                              where a.Organiser == true
                              where p.Date > DateTime.Now
                              select new
                              {
                                  Postname = p.Postname,
                                  Description = p.Description,
                                  ImgUrl = u.Photo,
                                  Duration = p.Duration,
                                  Latitude = p.Latitude,
                                  Longitude = p.Longitude,
                                  Address = p.Place,
                                  Sport = s.Sportname,
                                  Organiser = u.Username,
                                  Date = p.Date,
                                  Category = s.Category
                            
                              }).ToList();

            string objects = JsonConvert.SerializeObject(postobject);

            return objects;


        }

        //public void AddNewMessageToDatabase(Message msg)
        //{

        //    db.Messages.Add(msg);
        //    db.SaveChanges();
        //}
    }
}
