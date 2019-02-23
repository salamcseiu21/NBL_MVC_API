
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.Areas.Factory.Controllers
{
    [Authorize(Roles = "Factory")]
    public class TransferController : Controller
    {
        private readonly IProductManager _iProductManager;
        private readonly IInventoryManager _iInventoryManager;

        public TransferController(IProductManager iProductManager,IInventoryManager iInventoryManager)
        {
            _iProductManager = iProductManager;
            _iInventoryManager = iInventoryManager;
        }
        // GET: Factory/Transfer
        [HttpGet]
        public ActionResult Issue()
        {
            Session["factory_transfer_product_list"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Issue(FormCollection collection)
        {
            List<Product> productList = (List<Product>)Session["factory_transfer_product_list"]; 
            if (productList != null)
            {

                int fromBranchId = 9;
                int toBranchId = Convert.ToInt32(collection["ToBranchId"]);
                var user =(ViewUser)Session["user"];
                TransferIssue aTransferIssue = new TransferIssue
                {
                    Products = productList,
                    FromBranchId = fromBranchId,
                    ToBranchId = toBranchId,
                    IssueByUserId = user.UserId,
                    TransferIssueDate = Convert.ToDateTime(collection["TransactionDate"])
                };
                int rowAffected = _iProductManager.IssueProductToTransfer(aTransferIssue);
                if (rowAffected > 0)
                {
                    Session["factory_transfer_product_list"] = null;
                    TempData["message"] = "Transfer Issue  Successful!";
                }
                else
                {
                    TempData["message"] = "Failed to Issue Transfer!";
                }

            }

            return View();
        }

        [HttpPost]
        public JsonResult TempTransferIssue(CreateTransferIssueViewModel model) 
        {
            SuccessErrorModel msgSuccessErrorModel=new SuccessErrorModel();
            try
            {
                List<Product> productList = (List<Product>)Session["factory_transfer_product_list"]; 
                var product = _iProductManager.GetAll().ToList().Find(n => n.ProductId == model.ProductId);
                product.Quantity = model.Quantity;

                if(productList!=null)
                {
                    productList.Add(product);
                }
                else
                {
                    productList = new List<Product> { product };
                }
                
                Session["factory_transfer_product_list"] = productList;
            }
            catch(Exception exception)
            {
                msgSuccessErrorModel.Message = "<p style='colore:red'>" + exception.Message + "</p>";
            }
            return Json(msgSuccessErrorModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Update(FormCollection collection)
        {
            try
            {
                List<Product> productList = (List<Product>)Session["factory_transfer_product_list"];
                int productId = Convert.ToInt32(collection["productIdToRemove"]);
                if (productId != 0)
                {
                    var product = productList.Find(n => n.ProductId == productId);
                    productList.Remove(product);
                    Session["factory_transfer_product_list"] = productList;
                }
                else
                {
                    var collectionAllKeys = collection.AllKeys.ToList();
                    var productIdList = collectionAllKeys.FindAll(n => n.Contains("NewQuantity"));
                    foreach (string s in productIdList)
                    {
                        var value = s.Replace("NewQuantity_", "");
                        int productIdToUpdate = Convert.ToInt32(value);
                        int qty = Convert.ToInt32(collection["NewQuantity_" + value]);
                        var product = productList.Find(n => n.ProductId == productIdToUpdate); 

                        if (product != null)
                        {
                            productList.Remove(product);
                            product.Quantity = qty;
                            productList.Add(product);
                            Session["factory_transfer_product_list"] = productList;
                        }

                    }
                }


            }
            catch (Exception e)
            {

                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException?.Message;

            }
        }
        [HttpPost]
        public JsonResult ProductNameAutoComplete(string prefix) 
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var products = _iInventoryManager.GetStockProductByCompanyId(companyId).ToList();
            var productList = (from c in products
                               where c.ProductName.ToLower().Contains(prefix.ToLower())
                               select new
                               {
                                   label = c.ProductName,
                                   val = c.ProductId
                               }).ToList();

            return Json(productList);
        }

        //----------------------Get Stock Quantiy  By  product Id----------
        public JsonResult GetProductQuantityInStockById(int productId)
        {
            StockModel stock = new StockModel();
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var qty = _iInventoryManager.GetStockProductByCompanyId(companyId).ToList().Find(n=>n.ProductId==productId).StockQuantity;
            stock.StockQty = qty;
            return Json(stock, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTempTransferIssueProductList()
        {
            if (Session["factory_transfer_product_list"] != null)
            {
                IEnumerable<Product> productList = ((List<Product>)Session["factory_transfer_product_list"]).ToList();
                return Json(productList, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Product>(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Transactions()
        {
            var transactions = _iInventoryManager.GetAllProductTransactionFromFactory();
            return View(transactions);
        }

        [HttpGet]
        public ActionResult CreateTrip()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTrip(FormCollection collection)
        {
            return View();
        }

        public JsonResult AddRequistionToTripXmlFile(FormCollection collection)
        {
           SuccessErrorModel model=new SuccessErrorModel();
            try
            {
                int requisitionRef = Convert.ToInt32(collection["RequisitionRef"]);
                int requisitionId = Convert.ToInt32(collection["RequisitionId"]);
                var requisition = _iProductManager.GetRequsitionsByStatus(0).ToList().Find(n=>n.RequisitionId==requisitionId);
                var filePath = Server.MapPath("~/Files/" + "Create_Trip_File.xml");
                var xmlDocument = XDocument.Load(filePath);
                xmlDocument.Element("Requisitions")?.Add(
                    new XElement("Requisition", new XAttribute("Id", requisitionId),
                        new XElement("RequisitionRef", requisitionRef),
                        new XElement("Quantity", requisition.Quantity)
                    ));
                xmlDocument.Save(filePath);
            }
            catch (Exception e)
            {
                model.Message = "<p style='color:red'> Failed to add requisition ref to List"+e.Message+"</p>";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequisitionRefeAutoComplete(string prefix)
        {
           
            var requisitions = (from c in _iProductManager.GetRequsitionsByStatus(0)
                where c.RequisitionRef.ToLower().Contains(prefix.ToLower())
                select new
                {
                    label = c.RequisitionRef,
                    val = c.RequisitionId
                }).ToList();

            return Json(requisitions);
        }

        public JsonResult GetRequisitionById(long requisitionId)
        {
            var requisition = _iProductManager.GetRequsitionsByStatus(0).ToList()
                .Find(n => n.RequisitionId == requisitionId);
            return Json(requisition, JsonRequestBehavior.AllowGet);
        }
    }
}