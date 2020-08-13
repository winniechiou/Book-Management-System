using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookSystem.Dao
{
    public class TestCodeDao : ICodeDao
    {
        public List<SelectListItem> GetBookClass()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetStatus(string Status)
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetUser()
        {
            throw new NotImplementedException();
        }
    }
}
