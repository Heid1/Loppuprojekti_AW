using System;
using System.Collections.Generic;

#nullable disable

namespace Loppuprojekti_AW.Models
{
    public partial class Message
    {
        public int Messageid { get; set; }
        public int Senderid { get; set; }
        public int? Receiverid { get; set; }
        public DateTime Sendtime { get; set; }

        public virtual Enduser Receiver { get; set; }
        public virtual Enduser Sender { get; set; }
    }
}
