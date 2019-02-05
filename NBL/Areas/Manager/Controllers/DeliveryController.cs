using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using NBL.Areas.Admin.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.Products;
using NBL.Models.Enums;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Productions;

namespace NBL.Areas.Manager.Controllers
{
    [Authorize(Roles = "Distributor")]
    public class DeliveryController : Controller
    {
       
        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IProductManager _iProductManager;
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IClientManager _iClientManager;

        public DeliveryController(IDeliveryManager iDeliveryManager,IInventoryManager iInventoryManager,IProductManager iProductManager,IClientManager iClientManager,IInvoiceManager iInvoiceManager)
        {
            _iDeliveryManager = iDeliveryManager;
            _iInventoryManager = iInventoryManager;
            _iProductManager = iProductManager;
            _iClientManager = iClientManager;
            _iInvoiceManager = iInvoiceManager;
        }
        public ActionResult OrderList()
        {
            SummaryModel model=new SummaryModel();
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var invoicedOrders = _iInvoiceManager.GetAllInvoicedOrdersByBranchAndCompanyId(branchId, companyId).ToList();
            foreach (var invoice in invoicedOrders)
            {
                invoice.Client = _iClientManager.GetById(invoice.ClientId);
            }
            model.InvoicedOrderList = invoicedOrders;
            return View(model);
        }
       
        [HttpGet]
        public ActionResult Delivery(int id)
        {

            var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(id);
            var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
            var model=new ViewDeliveryModel
            {
                Client = _iClientManager.GetById(invoice.ClientId),
                InvoiceDetailses = invoicedOrders,
                Invoice = invoice
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Delivery(FormCollection collection)
        {
            try
            {


                int deliverebyUserId = ((ViewUser)Session["user"]).UserId;
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int invoiceId = Convert.ToInt32(collection["InvoiceId"]);
                var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
                var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
                int invoiceQty = invoicedOrders.Sum(n => n.InvoicedQuantity);

                List<InvoiceDetails> invoiceList = new List<InvoiceDetails>();

                string fileName = "Ordered_Product_List_For_" + invoiceId;
                var filePath = Server.MapPath("~/Files/" + fileName);
                    //if the file is exists read the file
                var barcodeList = _iProductManager.GetScannedBarcodeListFromTextFile(filePath).ToList();

                foreach (var item in invoicedOrders)
                {
                    ProductDetails aProduct = _iProductManager.GetProductDetailsByProductId(item.ProductId);
                    item.UnitPrice = aProduct.UnitPrice;
                    var productWishCodeList = barcodeList.ToList().FindAll(n => Convert.ToInt32(n.ProductCode.Substring(0, 3)) == item.ProductId).Select(n => n.ProductCode); 
                    int qty = productWishCodeList.ToList().Count;
                    foreach (var code in productWishCodeList)
                    {
                        item.ScannedProductCodes += code + ",";
                    }
                    item.ScannedProductCodes=item.ScannedProductCodes.TrimEnd(',');
                    if (qty > 0)
                    {
                        item.Quantity = qty;
                        invoiceList.Add(item);
                    }

                }
                int diliveryQty =invoicedOrders.Sum(n=>n.DeliveredQuantity)+invoiceList.Sum(n => n.Quantity);
                int invoiceStatus = Convert.ToInt32(InvoiceStatus.PartiallyDelivered);
                int orderStatus = Convert.ToInt32(OrderStatus.PartiallyDelivered);
                if (invoiceQty == diliveryQty)
                {
                    invoiceStatus = Convert.ToInt32(InvoiceStatus.Delivered);
                    orderStatus = Convert.ToInt32(OrderStatus.Delivered);
                }
                

                var aDelivery = new Delivery
                {
                    TransactionRef = invoice.TransactionRef,
                    InvoiceRef = invoice.InvoiceRef,
                    DeliveredByUserId = deliverebyUserId,
                    Transportation = collection["Transportation"],
                    DriverName = collection["DriverName"],
                    DriverPhone = collection["DriverPhone"],
                    TransportationCost = Convert.ToDecimal(collection["TransportationCost"]),
                    VehicleNo = collection["VehicleNo"],
                    DeliveryDate = Convert.ToDateTime(collection["DeliveryDate"]).Date,
                    CompanyId = invoice.CompanyId,
                    ToBranchId = invoice.BranchId,
                    FromBranchId = invoice.BranchId
                };
                string result = _iInventoryManager.Save(invoiceList, aDelivery, invoiceStatus,orderStatus);
                if (result.StartsWith("S"))
                {
                    return RedirectToAction("OrderList");
                }
                return View();
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
                //return View("Delivery");
                throw new Exception();
            }
        }

        [HttpPost]
        public JsonResult SaveScannedBarcodeToTextFile(FormCollection collection)
        {
            SuccessErrorModel model = new SuccessErrorModel();
            try
            {

                var id = Convert.ToInt32(collection["InvoiceId"]);
                var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(id);
                string scannedBarCode = collection["ProductCode"];
                string fileName = "Ordered_Product_List_For_" + id;
                var filePath = Server.MapPath("~/Files/" + fileName);
                var barcodeList = _iProductManager.ScannedBarCodes(filePath);
                var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
                bool isScannedBefore = _iProductManager.IsScannedBefore(barcodeList, scannedBarCode);
                int productId = Convert.ToInt32(scannedBarCode.Substring(0, 3));
                bool isValied = invoicedOrders.Select(n => n.ProductId).Contains(productId);
                bool isScannComplete = invoicedOrders.ToList().FindAll(n=>n.ProductId==productId).Sum(n => n.Quantity) == barcodeList.FindAll(n=>Convert.ToInt32(n.ProductCode.Substring(0,3))==productId).Count;

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
        public ActionResult ViewInvoiceIdOrderDetails(int invoiceId)
        {
            var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
            return PartialView("_ModalOrderDeliveryPartialPage",invoice);
        }

        [HttpGet]
        public JsonResult LoadDeliverableProduct(int invoiceId)
        {
            List<ScannedBarCode> barcodeList = new List<ScannedBarCode>();
            string fileName = "Ordered_Product_List_For_" + invoiceId;
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
            var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
            var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
           // var products = _iProductManager.GetIssuedProductListById(invoiceId);
            foreach (var item in invoicedOrders)
            {
                foreach (var code in barcodeList.FindAll(n => Convert.ToInt32(n.ProductCode.Substring(0, 3)) == item.ProductId))
                {
                    item.ScannedProductCodes += code.ProductCode + ",";

                }

            }

            return Json(invoicedOrders, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Calan(int id)
        {
            var chalan = _iDeliveryManager.GetChalanByDeliveryId(id);
            return View(chalan);

        }
        public ActionResult DeleveredOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var user = (ViewUser) Session["user"];
            var orders = _iDeliveryManager.GetAllDeliveredOrdersByBranchCompanyAndUserId(branchId,companyId,user.UserId).ToList();
            return View(orders);
        }
    }
}