using System;
using System.Web.Mvc;
using NBL.BLL.Contracts;

namespace NBL.Areas.Factory.Controllers
{

    [Authorize(Roles ="Factory")]
    public class HomeController : Controller
    {

        private readonly IInventoryManager _iInventoryManager;

        public HomeController(IInventoryManager iInventoryManager)
        {
            _iInventoryManager = iInventoryManager;
        }
        // GET: Factory/Home
        public ActionResult Home()
        {
            Session.Remove("BranchId");
            Session.Remove("Branch");
            return View();
        }

        [HttpGet]
        public PartialViewResult Stock()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var stock = _iInventoryManager.GetStockProductByCompanyId(companyId);
            return PartialView("_ViewStockProductInBranchPartialPage",stock);
        }
    }
}