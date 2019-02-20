﻿
using System;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Admin.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Admin.Controllers
{
  
    public class HomeController : Controller
    {

        private readonly IClientManager _iClientManager;
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IInventoryManager _iInventoryManager;

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IEmployeeManager iEmployeeManager,IInventoryManager iInventoryManager,IInvoiceManager iInvoiceManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iEmployeeManager = iEmployeeManager;
            _iInventoryManager = iInventoryManager;
            _iInvoiceManager = iInvoiceManager;
        }

        [Authorize]
        // GET: Admin/Home
        public ActionResult Home()
        {
            SummaryModel model = new SummaryModel();
            if (User.Identity.Name.Contains("nbl_sales"))
            {
                Session.Remove("BranchId");
                Session.Remove("Branch");
            }
            else
            {
                var branchId = Convert.ToInt32(Session["BranchId"]);
                var companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iInvoiceManager.GetAllInvoicedOrdersByCompanyId(companyId).ToList().FindAll(n => n.BranchId == branchId).ToList();
                var pendingOrders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).ToList().FindAll(n => n.Status == 1).ToList();
                var clients = _iClientManager.GetAllClientDetailsByBranchId(branchId).ToList();
                var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
                var delayedOrders = _iOrderManager.GetDelayedOrdersToAdminByBranchAndCompanyId(branchId, companyId);
                var verifiedOrders = _iOrderManager.GetVerifiedOrdersByBranchAndCompanyId(branchId, companyId);
                 model = new SummaryModel
                {
                    InvoicedOrderList = orders,
                    PendingOrders = pendingOrders,
                    Clients = clients,
                    Products = products,
                    DelayedOrders = delayedOrders,
                    VerifiedOrders = verifiedOrders
                };
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public PartialViewResult ViewClient()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var clients = _iClientManager.GetClientByBranchId(branchId).ToList();
            return PartialView("_ViewClientPartialPage",clients);

        }

        [Authorize(Roles = "Admin")]
        public PartialViewResult ViewClientProfile(int id)
        {
            var client = _iClientManager.GetClientDeailsById(id);
            return PartialView("_ViewClientProfilePartialPage",client);
        }
        [Authorize(Roles = "Admin")]

        public PartialViewResult ViewEmployee()
        {
            var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo().ToList();
            return PartialView("_ViewEmployeePartialPage",employees);

        }
        [Authorize(Roles = "Admin")]

        public PartialViewResult ViewEmployeeProfile(int id)
        {
            var employee = _iEmployeeManager.GetEmployeeById(id);
            return PartialView("_ViewEmployeeProfilePartialPage",employee);

        }
        [Authorize(Roles = "Admin")]

        public PartialViewResult Stock() 
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            return PartialView("_ViewStockProductInBranchPartialPage",products);

        }
        [Authorize(Roles = "Admin")]
        public PartialViewResult ViewBranch()
        {
            var branches = _iBranchManager.GetAllBranches().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }

        [Authorize(Roles = "Admin")]
        public PartialViewResult VerifyingOrders()
        {
            int branchId = Convert.ToInt32(Session["branchId"]);
            int companyId = Convert.ToInt32(Session["companyId"]);
            var verifiedOrders = _iOrderManager.GetVerifiedOrdersByBranchAndCompanyId(branchId, companyId);
            return PartialView("_ViewVerifyingOrdersPartialPage",verifiedOrders);
        }
    }
}