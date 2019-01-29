using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Productions;
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
            var model=new ViewTransferIssueModel();
            foreach (var issue in issueList)
            {
                model.FromBranch = _iBranchManager.GetById(issue.FromBranchId);
                model.ToBranch = _iBranchManager.GetById(issue.ToBranchId);
                model.TransferIssues = issueList.ToList();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delivery(int id)
        {

            var transferIssue = _iProductManager.GetDeliverableTransferIssueById(id);
             var model = new ViewTransferIssueModel
                {
                    FromBranch = _iBranchManager.GetById(transferIssue.FromBranchId),
                    ToBranch = _iBranchManager.GetById(transferIssue.ToBranchId),
                    TransferIssue = transferIssue
                };
            return View(model);
        }

        [HttpPost]
        public void SaveScannedBarcodeToTextFile(FormCollection collection)
        {
            var id = Convert.ToInt32(collection["TransferIssueId"]);
            string code = collection["ProductCode"];
            List<ScannedBarCode> barcodeList = new List<ScannedBarCode>();
            string fileName = "Deliverd_Issued_Product_For_" + id;
            var filePath = Server.MapPath("~/Files/" + fileName);
            if (System.IO.File.Exists(filePath))
            {
                //if the file is exists read the file
                barcodeList = _iProductManager.GetScannedBarcodeListFromTextFile(filePath).ToList();
            }

            else
            {
                //if the file does not exists create the file
                System.IO.File.Create(filePath).Close();
            }

            int productId = Convert.ToInt32(code.Substring(0, 3));
            Product product = _iProductManager.GetProductByProductId(productId);
            var scannedBarCode = barcodeList.ToList().Find(n => n.ProductCode.Equals(code));
            if (product != null && scannedBarCode == null)
            {
               _iProductManager.AddProductToTextFile(code, filePath);
            }
            
        }


        public ActionResult SaveProductToFactoryInventory(FormCollection collection)
        {
            int transferIssueId = Convert.ToInt32(collection["TransferIssueId"]);
            string fileName = "Deliverd_Issued_Product_For_" + transferIssueId;
            var filePath = Server.MapPath("~/Files/" + fileName);
            var products = _iProductManager.GetScannedBarcodeListFromTextFile(filePath).ToList();
            int deliverebyUserId = ((ViewUser)Session["user"]).UserId;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TransferIssue transferIssue = _iProductManager.GetDeliverableTransferIssueById(transferIssueId);
            IEnumerable<TransferIssueDetails> issueDetails = _iProductManager.GetTransferIssueDetailsById(transferIssueId);

            foreach (var detail in issueDetails)
            {
                detail.BarCodes = products.ToList()
                    .FindAll(n => Convert.ToInt32(n.ProductCode.Substring(0, 3)) == detail.ProductId).ToList();
                foreach (var product in products.ToList().FindAll(n=>Convert.ToInt32(n.ProductCode.Substring(0,3)) == detail.ProductId).ToList())
                {
                    detail.ProductBarCodes += product.ProductCode + ",";
                }
            }

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
                System.IO.File.Create(filePath).Close();
                //---------------Send mail to branch before redirect--------------
                return RedirectToAction("DeliverableTransferIssueList");
            }
            return RedirectToAction("Delivery",new {id= transferIssueId });
        }

        public PartialViewResult ViewOrderDetails(int transferIssueId) 
        {

            TransferIssue model = _iProductManager.GetDeliverableTransferIssueById(transferIssueId);
            return PartialView("_ViewDeliveryModalPartialPage", model);
        }


        public JsonResult LoadDeliverableProduct(int issueId)
        {
            List<ScannedBarCode> barcodeList = new List<ScannedBarCode>();
            string fileName = "Deliverd_Issued_Product_For_" + issueId;
            var filePath = Server.MapPath("~/Files/" + fileName);
            if (System.IO.File.Exists(filePath))
            {
                //if the file is exists read the file
                barcodeList = _iProductManager.GetScannedBarcodeListFromTextFile(filePath).ToList();
            }
            else
            {
                //if the file does not exists create the file
                System.IO.File.Create(filePath).Close();
            }
            var products = _iProductManager.GetIssuedProductListById(issueId);
            foreach (Product product in products)
            {
                foreach (ScannedBarCode code in barcodeList.FindAll(n => Convert.ToInt32(n.ProductCode.Substring(0, 3)) == product.ProductId))
                {
                    product.ScannedProductCodes += code.ProductCode+",";
                }
            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}