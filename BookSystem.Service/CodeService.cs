using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookSystem.Service
{
    public class CodeService : ICodeService
    {
        private BookSystem.Dao.ICodeDao codeDao { get; set; }

        public List<SelectListItem> GetBookClass()
        {
            return codeDao.GetBookClass();
        }

        public List<SelectListItem> GetUser()
        {
            return codeDao.GetUser();
        }
        public List<SelectListItem> GetStatus(string Status)
        {
            return codeDao.GetStatus(Status);
        }

    }

}
