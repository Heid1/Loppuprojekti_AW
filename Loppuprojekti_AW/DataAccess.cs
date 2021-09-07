using Loppuprojekti_AW.Models;
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

        public Enduser GetUserById(int Identity)
        {
            MoveoContext db = new MoveoContext();

            var Enduser = db.Endusers.Find(Identity);

            return Enduser;
        }

        public void EditUser(Enduser Eu)
        {
            MoveoContext db = new MoveoContext();


            db.SaveChanges();
        }
    }
}
