using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.Areas.Factory.Controllers
{
    [Authorize(Roles ="Factory")]
    public class DeliveryController : Controller
    {

        private readonly IProductManager _iProductManager;
        private readonly IFactoryDeliveryManager _iFactoryDeliveryManager;
        private readonly IBranchManager _iBranchManager;
        // GET: Factory/Delivery
        public DeliveryController(IProductManager iProductManager,IFactoryDeliveryManager iFactoryDeliveryManager,IBranchManager iBranchManager)
        {
            _iProductManager = iProductManager;
            _iFactoryDeliveryManager = iFactoryDeliveryManager;
            _iBranchManager = iBranchManager;
        }
        public ActionResult DeliverableTransferIssueList() 
        {
            IEnumerable<TransferIssue> issueList = _iProductManager.GetDeliverableTransferIssueList();
            ViewTransferIssueModel model=new ViewTransferIssueModel();
            foreach (var issue in issueList)
            {
                model.FromBranch = _iBranchManager.GetById(issue.FromBranchId);
                model.ToBranch = _iBranchManager.GetById(issue.ToBranchId);
                model.TransferIssues = issueList.ToList();
            }
            return View(model);
        }

        public ActionResult Delivery(int id)
        {
          
            var deliverable = _iProductManager.GetDeliverableTransferIssueById(id);
            IEnumerable<TransferIssueDetails> issueDetails = _iProductManager.GetTransferIssueDetailsById(id);
            ViewTransferIssueDetailsModel model =
                new ViewTransferIssueDetailsModel
                {
                    FromBranch = _iBranchManager.GetById(deliverable.FromBranchId),
                    ToBranch = _iBranchManager.GetById(deliverable.ToBranchId),
                    TransferIssue = deliverable,
                    TransferIssueDetailses = issueDetails.ToList()
                };
            return View(model);
        }

        [HttpPost]
        public ActionResult Delivery(int id,FormCollection collection)
        {
            int deliverebyUserId = ((ViewUser)Session["user"]).UserId;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TransferIssue transferIssue = _iProductManager.GetDeliverableTransferIssueById(id);
            IEnumerable<TransferIssueDetails> issueDetails = _iProductManager.GetTransferIssueDetailsById(id);
            ViewTransferIssueDetailsModel model = new ViewTransferIssueDetailsModel
            {
                TransferIssue = transferIssue,
                TransferIssueDetailses = issueDetails.ToList(),
                FromBranch = _iBranchManager.GetById(transferIssue.FromBranchId),
                ToBranch = _iBranchManager.GetById(transferIssue.ToBranchId)
            };
            Delivery aDelivery = new Delivery
            {
                TransactionRef = transferIssue.TransferIssueRef,
                DeliveredByUserId = deliverebyUserId,
                Transportation = collection["Transportation"],
                DriverName = collection["DriverName"],
                TransportationCost = Convert.ToDecimal(collection["TransportationCost"]),
                VehicleNo = collection["VehicleNo"],
                DeliveryDate = Convert.ToDateTime(collection["DeliveryDate"]).Date,
                CompanyId = companyId,
                ToBranchId = transferIssue.ToBranchId,
                FromBranchId = transferIssue.FromBranchId
            };

            string result = _iFactoryDeliveryManager.SaveDeliveryInformation(aDelivery, issueDetails);
            if (result.StartsWith("Sa"))
            {
                //---------------Send mail to branch before redirect--------------
                return RedirectToAction("DeliverableTransferIssueList");
            }

            ViewBag.Deliverable = transferIssue;
            return View(model);
        }
    }
}