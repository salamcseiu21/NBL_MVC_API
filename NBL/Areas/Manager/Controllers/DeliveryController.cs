using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using NBL.Areas.Admin.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Invoices;
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
                int invoiceId = Convert.ToInt32(collection["InvoiceId"]);
                var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
                var deliveredQty = _iInvoiceManager.GetDeliveredProductsByInvoiceRef(invoice.InvoiceRef).Count;
                string fileName = "Ordered_Product_List_For_" + invoiceId;
                var filePath = Server.MapPath("~/Files/" + fileName);
                    //if the file is exists read the file
                var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                int quantity = deliveredQty + barcodeList.Count;
                int invoiceStatus = Convert.ToInt32(InvoiceStatus.PartiallyDelivered);
                int orderStatus = Convert.ToInt32(OrderStatus.PartiallyDelivered);
                if (invoice.Quantity == quantity)
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
                string result = _iInventoryManager.Save(barcodeList, aDelivery, invoiceStatus,orderStatus);
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
                int productId = Convert.ToInt32(scannedBarCode.Substring(0, 3));
                string fileName = "Ordered_Product_List_For_" + id;
                var filePath = Server.MapPath("~/Files/" + fileName);
                var barcodeList = _iProductManager.ScannedBarCodes(filePath);
                bool isScannedBefore = _iProductManager.IsScannedBefore(barcodeList, scannedBarCode);
                bool isSold = _iInventoryManager.IsThisProductSold(scannedBarCode);

                //------------Get invoced products-------------
                var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
                List<InvoiceDetails> list = new List<InvoiceDetails>();
                var deliveredProducts = _iInvoiceManager.GetDeliveredProductsByInvoiceRef(invoice.InvoiceRef);

                foreach (InvoiceDetails invoiceDetailse in invoicedOrders)
                {
                    var invoiceQty = invoiceDetailse.Quantity;
                    var deliveredQty = deliveredProducts.ToList().FindAll(n => n.ProductId == invoiceDetailse.ProductId).Count;
                    if (invoiceQty != deliveredQty)
                    {
                        invoiceDetailse.Quantity = invoiceQty - deliveredQty;
                        list.Add(invoiceDetailse);
                    }

                }
                bool isValied = list.Select(n => n.ProductId).Contains(productId);
                bool isScannComplete = list.ToList().FindAll(n=>n.ProductId==productId).Sum(n=>n.Quantity) == barcodeList.FindAll(n=>Convert.ToInt32(n.ProductCode.Substring(0,3))==productId).Count;

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

                if (isValied && !isScannedBefore && !isScannComplete && !isSold)
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
            var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
          

            List<ScannedProduct> barcodeList = new List<ScannedProduct>();
            string fileName = "Ordered_Product_List_For_" + invoiceId;
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
          
            var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
            List<InvoiceDetails> list=new List<InvoiceDetails>();

            var deliveredProducts = _iInvoiceManager.GetDeliveredProductsByInvoiceRef(invoice.InvoiceRef);

            foreach (InvoiceDetails invoiceDetailse in invoicedOrders)
            {
                var invoiceQty = invoiceDetailse.Quantity;
                var deliveredQty = deliveredProducts.ToList().FindAll(n => n.ProductId == invoiceDetailse.ProductId).Count;
                if (invoiceQty != deliveredQty)
                {
                    invoiceDetailse.Quantity = invoiceQty - deliveredQty;
                    list.Add(invoiceDetailse);
                } 
            }
            foreach (var item in list)
            {
                foreach (var code in barcodeList.FindAll(n => Convert.ToInt32(n.ProductCode.Substring(0, 3)) == item.ProductId))
                {
                    item.ScannedProductCodes += code.ProductCode + ",";

                }

            }

            return Json(list, JsonRequestBehavior.AllowGet);
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