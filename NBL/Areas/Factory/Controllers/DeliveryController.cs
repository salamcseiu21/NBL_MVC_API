using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models;
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
            var transferIssues = issueList as TransferIssue[] ?? issueList.ToArray();
            foreach (var issue in transferIssues)
            {
                model.FromBranch = _iBranchManager.GetById(issue.FromBranchId);
                model.ToBranch = _iBranchManager.GetById(issue.ToBranchId);
                model.TransferIssues = transferIssues.ToList();
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
        public JsonResult SaveScannedBarcodeToTextFile(FormCollection collection)
        {
            SuccessErrorModel model = new SuccessErrorModel();
            try
            {

                var id = Convert.ToInt32(collection["TransferIssueId"]);
                string scannedBarCode = collection["ProductCode"];

                string fileName = "Deliverd_Issued_Product_For_" + id;
                var filePath = Server.MapPath("~/Files/" + fileName);
                var barcodeList = _iProductManager.ScannedBarCodes(filePath);


                var issuedProducts = _iProductManager.GetTransferIssueDetailsById(id);
                bool isScannedBefore = _iProductManager.IsScannedBefore(barcodeList, scannedBarCode);
                int productId = Convert.ToInt32(scannedBarCode.Substring(0, 3));
                bool isValied = issuedProducts.Select(n => n.ProductId).Contains(productId);
                bool isScannComplete = issuedProducts.Sum(n => n.Quantity) == barcodeList.Count;

                if (scannedBarCode.Length != 13)
                {
                    model.Message = "<p style='color:red'> Invalid Barcode</p>";
                }
                if (isScannedBefore)
                {
                    model.Message = "<p style='color:red'> Already Scanned</p>";
                }
                if (isScannComplete)
                {
                    model.Message = "<p style='color:green'> Scan Completed.</p>";
                }

                if (isValied && !isScannedBefore && scannedBarCode.Length == 13 && !isScannComplete)
                {
                    _iProductManager.AddProductToTextFile(scannedBarCode, filePath);
                }
            }
            catch (FormatException exception)
            {
                model.Message = "<p style='color:red'>" + exception.GetType() + "</p>";
            }
            catch (Exception exception)
            {
                
                    model.Message = "<p style='color:red'>" + exception.Message + "</p>";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
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

            var transferIssueDetailses = issueDetails as TransferIssueDetails[] ?? issueDetails.ToArray();  
            foreach (var detail in transferIssueDetailses)
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

            string result = _iFactoryDeliveryManager.SaveDeliveryInformation(aDelivery, transferIssueDetailses);
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
            foreach (var product in products)
            {
                foreach (var code in barcodeList.FindAll(n => Convert.ToInt32(n.ProductCode.Substring(0, 3)) == product.ProductId))
                {
                    product.ScannedProductCodes += code.ProductCode+",";
                   
                }
               
            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}