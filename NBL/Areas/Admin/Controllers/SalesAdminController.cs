
using System.Web.Mvc;

namespace NBL.Areas.Admin.Controllers
{
    [Authorize(Roles = "SalesAdmin")]
    public class SalesAdminController : Controller
    {
        // GET: Admin/SalesAdmin
        public ActionResult Home() 
        {
            return View();
        }
    }
}