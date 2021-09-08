using Loppuprojekti_AW.Models;
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

        public static void DeleteProfile(Enduser Eu)
        {
            MoveoContext db = new MoveoContext();

            var muokattava = db.Endusers.Find(Eu.Userid);

            db.Remove(muokattava);
            db.SaveChanges();
        }

        // ----------------------- POSTS ----------------------------------------------

        //hae yleisimmät postit lajin mukaan (tämä on sanapilveä varten)
        public List<Sport> GetPostsByPrevalence()
        {
            var prevalencelist = db.Posts
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
    }
}
