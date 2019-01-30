using System.Web.Mvc;
namespace NBL.Areas.Factory.Controllers
{

    [Authorize(Roles ="Factory")]
    public class HomeController : Controller
    {
        // GET: Factory/Home
        public ActionResult Home()
        {

            Session.Remove("BranchId");
            Session.Remove("Branch");
            return View();
        }

    }
}