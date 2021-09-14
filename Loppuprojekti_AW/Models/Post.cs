using System;
using System.Collections.Generic;

#nullable disable

namespace Loppuprojekti_AW.Models
{
    public partial class Post
    {
        public Post()
        {
            AttendeesNavigation = new HashSet<Attendee>();
        }

        public int Postid { get; set; }
        public string Postname { get; set; }
        public int Sportid { get; set; }
        public string Description { get; set; }
        public int? Attendees { get; set; }
        public string Posttype { get; set; }
        public string Place { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public int? Privacy { get; set; }
        public decimal? Price { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }

        public virtual Sport Sport { get; set; }
        public virtual ICollection<Attendee> AttendeesNavigation { get; set; }
    }
}
