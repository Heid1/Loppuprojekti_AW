using System;
using System.Collections.Generic;

#nullable disable

namespace Loppuprojekti_AW.Models
{
    public partial class UsersSport
    {
        public int Userssportid { get; set; }
        public int Sportid { get; set; }
        public int Userid { get; set; }
        public int? Experience { get; set; }
        public string Level { get; set; }
        public string Accredit { get; set; }

        public virtual Sport Sport { get; set; }
        public virtual Enduser User { get; set; }
    }
}
