using System;
using System.Collections.Generic;

#nullable disable

namespace Loppuprojekti_AW.Models
{
    public partial class Enduser
    {
        public Enduser()
        {
            Attendees = new HashSet<Attendee>();
            MessageReceivers = new HashSet<Message>();
            MessageSenders = new HashSet<Message>();
            UsersSports = new HashSet<UsersSport>();
        }

        public int Userid { get; set; }
        public string Username { get; set; }
        public DateTime? Age { get; set; }
        public int Userrole { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
        public string Club { get; set; }

        public virtual ICollection<Attendee> Attendees { get; set; }
        public virtual ICollection<Message> MessageReceivers { get; set; }
        public virtual ICollection<Message> MessageSenders { get; set; }
        public virtual ICollection<UsersSport> UsersSports { get; set; }
    }
}
