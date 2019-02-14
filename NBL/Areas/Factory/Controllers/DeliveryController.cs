using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Validators;
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
        private readonly ICommonManager _iCommonManager;
        private readonly IInventoryManager _iInventoryManager;
        // GET: Factory/Delivery
        public DeliveryController(IProductManager iProductManager,IFactoryDeliveryManager iFactoryDeliveryManager,IBranchManager iBranchManager,IInventoryManager iInventoryManager,ICommonManager iCommonManager)
        {
            _iProductManager = iProductManager;
            _iFactoryDeliveryManager = iFactoryDeliveryManager;
            _iBranchManager = iBranchManager;
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
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
            var stock=_iInventoryManager.GetStockProductInFactory();
            Session["Factory_Stock"] = stock;
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
                var products = (List<ViewFactoryStockModel>) Session["Factory_Stock"];
                string scannedBarCode = collection["ProductCode"];
                int productId = Convert.ToInt32(scannedBarCode.Substring(0, 3));
                var transferIssueId = Convert.ToInt32(collection["TransferIssueId"]);
                string fileName = "Deliverd_Issued_Product_For_" + transferIssueId;
                var filePath = Server.MapPath("~/Files/" + fileName);
                var barcodeList = _iProductManager.ScannedProducts(filePath);
                if (barcodeList.Count != 0)
                {
                    foreach (ScannedProduct scannedProduct in barcodeList)
                    {
                        var p = products.Find(n => n.ProductBarCode.Equals(scannedProduct.ProductCode));
                        products.Remove(p);
                        Session["Factory_Stock"] = products;
                    }
                }

                bool exists = barcodeList.Select(n=>n.ProductCode).Contains(scannedBarCode);
                bool isDeliveredBefore = _iInventoryManager.IsThisProductDispachedFromFactory(scannedBarCode);

                DateTime date = _iCommonManager.GenerateDateFromBarCode(scannedBarCode);
                var oldestProducts = products.ToList().FindAll(n=>n.ProductionDate<date && n.ProductId==productId).ToList();
                var issuedProducts = _iProductManager.GetTransferIssueDetailsById(transferIssueId);
               

                var isValied = Validator.ValidateProductBarCode(scannedBarCode);

                

                bool isContains = issuedProducts.Select(n => n.ProductId).Contains(productId);
                bool isScannComplete = issuedProducts.ToList().FindAll(n=>n.ProductId==productId).Sum(n => n.Quantity) == barcodeList.FindAll(n=>Convert.ToInt32(n.ProductCode.Substring(0,3))==productId).Count;

                if (!isContains)
                {
                    model.Message = "<p style='color:red'> Invalid Product Scanned.....</p>";
                    return Json(model, JsonRequestBehavior.AllowGet);
                }

                if (isScannComplete)
                {
                    model.Message = "<p style='color:green'> Scan Completed.</p>";
                   return Json(model, JsonRequestBehavior.AllowGet);
                }

                if (oldestProducts.Count > 0)
                {
                    model.Message = "<p style='color:red'>There are total "+oldestProducts.Count+" Old product of this type .Please deliver those first .. </p>";
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                if (isValied && !exists && !isDeliveredBefore)
                {
                   var result= _iProductManager.AddProductToTextFile(scannedBarCode, filePath);
                    if (result.Contains("Added"))
                    {
                        var p=  products.Find(n => n.ProductBarCode.Equals(scannedBarCode));
                        products.Remove(p);
                        Session["Factory_Stock"] = products;
                    }
                }
            }
                catch (FormatException exception)
            {
              model.Message = "<p style='color:red'>Invalid Barcode</p>";
              return  Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                
                    model.Message = "<p style='color:red'>" + exception.Message + "</p>";
              return  Json(model, JsonRequestBehavior.AllowGet);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaveDispatchInformation(FormCollection collection) 
        {
            int transferIssueId = Convert.ToInt32(collection["TransferIssueId"]);
            var products = _iProductManager.GetIssuedProductListById(transferIssueId);
            string fileName = "Deliverd_Issued_Product_For_" + transferIssueId;
            var filePath = Server.MapPath("~/Files/" + fileName);
            var scannedProducts = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();

            int deliverebyUserId = ((ViewUser)Session["user"]).UserId;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TransferIssue transferIssue = _iProductManager.GetDeliverableTransferIssueById(transferIssueId);
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

            if (scannedProducts.Count == products.Sum(n=>n.Quantity))
            {
                string result = _iFactoryDeliveryManager.SaveDeliveryInformation(aDelivery, scannedProducts);
                if (result.StartsWith("Sa"))
                {
                    System.IO.File.Create(filePath).Close();
                    //---------------Send mail to branch before redirect--------------
                    return RedirectToAction("DeliverableTransferIssueList");
                }
                return RedirectToAction("Delivery", new { id = transferIssueId });
            }
            TempData["QuantityNotSame"]= "Issued Quantity and Scanned Quantity Should be same";
           
            return RedirectToAction("Delivery",new {id= transferIssueId });
        }

        public PartialViewResult ViewOrderDetails(int transferIssueId) 
        {

            TransferIssue model = _iProductManager.GetDeliverableTransferIssueById(transferIssueId);
            return PartialView("_ViewDeliveryModalPartialPage", model);
        }


        public JsonResult LoadDeliverableProduct(int issueId)
        {
            List<ScannedProduct> barcodeList = new List<ScannedProduct>();
            string fileName = "Deliverd_Issued_Product_For_" + issueId;
            var filePath = Server.MapPath("~/Files/" + fileName);
            if (System.IO.File.Exists(filePath))
            {
                //if the file is exists read the file
                barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
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