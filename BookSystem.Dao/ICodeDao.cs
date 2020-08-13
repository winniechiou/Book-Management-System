using System.Collections.Generic;
using System.Web.Mvc;

namespace BookSystem.Dao
{
    public interface ICodeDao
    {
        List<SelectListItem> GetBookClass();
        List<SelectListItem> GetStatus(string Status);
        List<SelectListItem> GetUser();
    }
}