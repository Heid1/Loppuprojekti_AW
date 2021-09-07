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
            muokattava.Age = Eu.Age;
            muokattava.Userrole = Eu.Userrole;
            muokattava.Description = Eu.Description;
            muokattava.UsersSports = Eu.UsersSports;
            muokattava.Club = Eu.Club;
            muokattava.Photo = Eu.Photo;

            db.SaveChanges();
        }

        public static void AddPost(Post post)
        {
            MoveoContext db = new MoveoContext();

           Attendee attend = CreateAttendee(post);


            var user = db.Attendees.Include(p => p.User).Where(p => p.Userid == 1);
           

            db.SaveChanges();
        }

        public static Attendee CreateAttendee(Post Post)
        {
            
            Attendee attend = new Attendee();

            //attend.User = Sessionista
            attend.Post = Post;

           // if (???) { attend.Organiser = true; }
            attend.Postid = Post.Postid;


            return attend;
        }

    }
}
