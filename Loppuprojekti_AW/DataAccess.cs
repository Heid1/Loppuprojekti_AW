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
