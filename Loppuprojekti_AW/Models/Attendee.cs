using System;
using System.Collections.Generic;

#nullable disable

namespace Loppuprojekti_AW.Models
{
    public partial class Attendee
    {
        public int Attendeeid { get; set; }
        public int Postid { get; set; }
        public int Userid { get; set; }
        public bool Organiser { get; set; }

        public virtual Post Post { get; set; }
        public virtual Enduser User { get; set; }
    }
}
