using System;
using System.Collections.Generic;

#nullable disable

namespace Loppuprojekti_AW.Models
{
    public partial class Sport
    {
        public Sport()
        {
            Posts = new HashSet<Post>();
            UsersSports = new HashSet<UsersSport>();
        }

        public int Sportid { get; set; }
        public string Sportname { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<UsersSport> UsersSports { get; set; }
    }
}
