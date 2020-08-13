using System.Collections.Generic;
using System.Web.Mvc;

namespace BookSystem.Service
{
    public interface ICodeService
    {
        List<SelectListItem> GetBookClass();
        List<SelectListItem> GetStatus(string Status);
        List<SelectListItem> GetUser();
    }
}