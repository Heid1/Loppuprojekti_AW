using Loppuprojekti_AW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loppuprojekti_AW.Models;

namespace Loppuprojekti_AW
{
    public class DataAccess
    {

        public Enduser GetUserById(int Identity)
        {
            MoveoContext db = new MoveoContext();

            var Enduser = db.Endusers.Find(Identity);

            return Enduser;
        }
        public MoveoContext db { get; set; }
        public DataAccess(MoveoContext data)
        {
            db = data;
        }


    }
}
