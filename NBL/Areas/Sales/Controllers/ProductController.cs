﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Orders;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Productions;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles ="User")]
    public class ProductController : Controller
    {
        private readonly IProductManager _iProductManager;
        private readonly IInventoryManager _iInventoryManager;

        public ProductController(IInventoryManager iInventoryManager,IProductManager iProductManager)
        {
            _iInventoryManager = iInventoryManager;
            _iProductManager = iProductManager;
        }
     
        public PartialViewResult Stock()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            return PartialView("_ViewStockProductInBranchPartialPage", products);
        }

        [HttpGet]
        public ActionResult Transaction()
        {
            Session["transactions"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Transaction(FormCollection collection)
        {
            List<TransactionModel> transactions = (List<TransactionModel>)Session["transactions"];
            if (transactions != null)
            {

                var model = transactions.ToList().First();
                Random random = new Random();
                model.TransactionId = random.Next(1, 1000000);
                int rowAffected = _iProductManager.TransferProduct(transactions, model);
                if (rowAffected > 0)
                {
                    TempData["message"] = "Transferred Successfully !";
                }
                else
                {
                    TempData["message"] = "Failed to Transfer Product !";
                }

            }

            return View();
        }
        [HttpPost]
        public void TempTransaction(FormCollection collection)
        {
            try
            {
                // TODO: Add Transcition logic here

                int productId = Convert.ToInt32(collection["ProductId"]);
                var product = _iProductManager.GetAll().ToList().Find(n => n.ProductId == productId);
                int fromBranchId = Convert.ToInt32(Session["BranchId"]);
                int toBranchId = Convert.ToInt32(collection["BranchId"]);
                int quantiy = Convert.ToInt32(collection["Quantity"]);
                int userId = ((ViewUser)Session["user"]).UserId;
                DateTime date = Convert.ToDateTime(collection["TransactionDate"]);
                TransactionModel aModel = new TransactionModel
                {
                    ProductId = productId,
                    FromBranchId = fromBranchId,
                    ToBranchId = toBranchId,
                    Quantity = quantiy,
                    UserId = userId,
                    ProductName = product.ProductName,
                    TransactionDate = date

                };

                List<TransactionModel> transactions = (List<TransactionModel>)Session["transactions"];

                if (transactions != null)
                {
                    var order = transactions.Find(n => n.ProductId == aModel.ProductId);
                    if (order != null)
                    {
                        transactions.Remove(order);
                        transactions.Add(aModel);
                        Session["transactions"] = transactions;
                        ViewBag.Transactions = transactions;
                    }
                    else
                    {
                        transactions.Add(aModel);
                        Session["transactions"] = transactions;
                        ViewBag.Transactions = transactions;
                    }

                }
                else
                {
                    transactions = new List<TransactionModel> { aModel };
                    Session["transactions"] = transactions;
                    ViewBag.Transactions = transactions;
                }

                //return View();
            }
            catch
            {
                //return View();
            }
        }
        [HttpPost]
        public void Update(FormCollection collection)
        {
            try
            {
                List<TransactionModel> transactions = (List<TransactionModel>)Session["transactions"];

                int pid = Convert.ToInt32(collection["productIdToRemove"]);
                if (pid != 0)
                {

                    var transaction = transactions.Find(n => n.ProductId == pid);
                    transactions.Remove(transaction);
                    Session["transactions"] = transactions;
                    ViewBag.Orders = transactions;
                }
                else
                {
                    var collectionAllKeys = collection.AllKeys.ToList();
                    var productIdList = collectionAllKeys.FindAll(n => n.Contains("NewQuantity"));
                    foreach (string s in productIdList)
                    {
                        var value = s.Replace("NewQuantity_", "");
                        var user = (User)Session["user"];
                        int productId = Convert.ToInt32(collection["product_Id_" + value]);
                        int qty = Convert.ToInt32(collection["NewQuantity_" + value]);
                        var transaction = transactions.Find(n => n.ProductId == productId);


                        if (transaction != null)
                        {
                            transactions.Remove(transaction);
                            transaction.Quantity = qty;
                            transaction.UserId = user.UserId;
                            transactions.Add(transaction);
                            Session["transactions"] = transactions;
                            ViewBag.Orders = transaction;
                        }

                    }
                }


            }
            catch (Exception e)
            {

                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;

            }
        }

        public ActionResult Receive()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var result = _iInventoryManager.GetAllReceiveableListByBranchAndCompanyId(branchId,companyId).ToList(); 
            ViewBag.ProductList = result;
            return View(result);
        }
        public ActionResult ReceiveableDetails(long id)
        {
            ReceiveProductViewModel aModel = new ReceiveProductViewModel {DeliveryId = id};
            return View(aModel);
        }

        [HttpPost]
        public void SaveScannedBarcodeToTextFile(FormCollection collection)
        {
            var productCode = collection["ProductCode"];
            var id = Convert.ToInt64(collection["DeliveryId"]);
            int productId = Convert.ToInt32(productCode.Substring(0, 3));
            Product product = _iProductManager.GetProductByProductId(productId);
            if (product != null)
            {
                string fileName = "Received_Product_For_" + id;
                var filePath = Server.MapPath("~/Files/" + fileName);
               _iProductManager.AddProductToTextFile(productCode, filePath);
            }
        }

        [HttpPost]
        public ActionResult ReceiveProduct(ReceiveProductViewModel model)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var transactionModel = _iInventoryManager.GetTransactionModelById(model.DeliveryId);
            List<TransactionModel> receivesProductList = _iInventoryManager.GetAllReceiveableProductToBranchByDeliveryId(model.DeliveryId).ToList();
            foreach (var item in receivesProductList)
            {
                int oldQty = _iInventoryManager.GetStockQtyByBranchAndProductId(branchId, item.ProductId);
                item.StockQuantity = oldQty + item.Quantity;

            }
           
            int rowAffected = _iInventoryManager.ReceiveProduct(receivesProductList.ToList(), transactionModel);
            var result = _iInventoryManager.GetAllReceiveableListByBranchAndCompanyId(branchId, companyId).ToList();
            ViewBag.ProductList = result;
            return RedirectToAction("Receive");
        }
        public JsonResult GetTempTransaction()
        {
            if(Session["transactions"] != null)
            {
                IEnumerable<TransactionModel> transactions = ((List<TransactionModel>)Session["transactions"]).ToList();
                return Json(transactions, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Order>(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadReceiveableProduct(long deliveryId)
        {
            List<TransactionModel> receivesProductList = _iInventoryManager.GetAllReceiveableProductToBranchByDeliveryId(deliveryId).ToList();
            List<ScannedBarCode> barcodeList = new List<ScannedBarCode>();
            string fileName = "Received_Product_For_" + deliveryId;
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
            foreach (TransactionModel transactionModel in receivesProductList)
            {
                int productId = Convert.ToInt32(transactionModel.ProductId);
                foreach (ScannedBarCode barCode in barcodeList.FindAll(n => Convert.ToInt32(n.ProductCode.Substring(0, 3)).Equals(productId)))
                {
                    transactionModel.RecievedProductBarCodes += barCode.ProductCode + ",";
                }
            }
            return Json(receivesProductList, JsonRequestBehavior.AllowGet);
        }
    }
}