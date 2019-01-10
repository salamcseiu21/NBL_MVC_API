using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using NBL.DAL;
using NBL.Models;

namespace NBL.Controllers
{
    public class HomeController : Controller
    {
        OrderGateway orderGateway=new OrderGateway();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            TempData["userName"] = User.Identity.Name;
            //var identityIsAuthenticated = User.Identity.IsAuthenticated;
            //var isInRole = User.IsInRole("Admin");
            //var version = typeof(Controller).Assembly.GetName().Version;
            return View();
        }
       
       // [Authorize(Roles ="Admin")]
        public ActionResult Contact()
        {
            ViewBag.Message = "This can be viewed only by users in Admin role only";
            return View();
        }



    }
}