
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Requisitions;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Requisitions;

namespace NBL.Areas.Admin.Controllers
{
    [Authorize(Roles = "SalesAdmin")]
    public class RequisitionController : Controller
    {

        private readonly IProductManager _iProductManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IBranchManager _iBranchManager;
        public RequisitionController(IProductManager iProductManager,IInventoryManager iInventoryManager,IBranchManager iBranchManager)
        {
            _iProductManager = iProductManager;
            _iInventoryManager = iInventoryManager;
            _iBranchManager = iBranchManager;
        }
        // GET: Admin/Requisition
        public ActionResult All()
        {
            IEnumerable<ViewRequisitionModel> requisitions = _iProductManager.GetRequsitions();
            return View(requisitions);
        }

        [HttpGet]
        public ActionResult Create()
        {
            
            return View();
        } 

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var filePath = Server.MapPath("~/Files/" + "Requisition_Products.xml");
            List<RequisitionModel> productList = GetProductFromXmalFile(filePath).ToList();

            if (productList.Count != 0)
            {
                var xmlData = XDocument.Load(filePath);
                int toBranchId = Convert.ToInt32(collection["ToBranchId"]);
                var user = (ViewUser)Session["user"];
                ViewRequisitionModel aRequisitionModel = new ViewRequisitionModel 
                {
                    Products = productList,
                    ToBranchId = toBranchId,
                    RequisitionByUserId = user.UserId,
                    RequisitionDate = Convert.ToDateTime(collection["RequisitionDate"])
                };
                int rowAffected = _iProductManager.SaveRequisitionInfo(aRequisitionModel);
                if (rowAffected > 0)
                {
                    xmlData.Root?.Elements().Remove();
                    xmlData.Save(filePath);
                    TempData["message"] = "Requisition Create  Successfully!";
                }
                else
                {
                    TempData["message"] = "Failed to create Requisition!";
                }

            }

            return View();
            
        }
        [HttpPost]
        public JsonResult AddRequisitionProductToXmlFile(FormCollection collection)
        {

            SuccessErrorModel msgSuccessErrorModel = new SuccessErrorModel();
            try
            {

                int productId = Convert.ToInt32(collection["ProductId"]);
                int toBranchId = Convert.ToInt32(collection["ToBranchId"]);
                var id=toBranchId.ToString("D2") + productId;
                var product = _iProductManager.GetAll().ToList().Find(n => n.ProductId == productId);
                int quantity = Convert.ToInt32(collection["Quantity"]);
                var filePath = Server.MapPath("~/Files/" + "Requisition_Products.xml");
                var xmlDocument = XDocument.Load(filePath);
                xmlDocument.Element("Products")?.Add(
                    new XElement("Product", new XAttribute("Id", id),
                        new XElement("ProductId", product.ProductId),
                        new XElement("ProductName", product.ProductName),
                        new XElement("Quantity", quantity),
                        new XElement("ToBranchId", toBranchId)
                    ));
                xmlDocument.Save(filePath);
            }
            catch (Exception exception)
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
                var filePath = Server.MapPath("~/Files/" + "Requisition_Products.xml");
                var xmlData = XDocument.Load(filePath);
                var branchIdProductId=  collection["productIdToRemove"];
                if (!branchIdProductId.Equals("0"))
                {
                    xmlData.Root?.Elements().Where(n => n.Attribute("Id")?.Value == branchIdProductId.ToString()).Remove();
                    xmlData.Save(filePath);
                }
               
                else
                {
                    var collectionAllKeys = collection.AllKeys.ToList();
                    var productIdList = collectionAllKeys.FindAll(n => n.Contains("NewQuantity"));
                    foreach (string s in productIdList)
                    {
                        var value = s.Replace("NewQuantity_", "");
                      
                        int qty = Convert.ToInt32(collection["NewQuantity_" + value]);
                       

                        xmlData.Element("Products")?
                               .Elements("Product")?
                               .Where(n => n.Attribute("Id")?.Value == value).FirstOrDefault()
                               ?.SetElementValue("Quantity", qty);
                        xmlData.Save(filePath);

                    }
                }


            }
            catch (Exception e)
            {

                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException?.Message;

            }
        }

        public JsonResult GetTempToRequsitionList()
        {
            var filePath = Server.MapPath("~/Files/" + "Requisition_Products.xml");

            if (System.IO.File.Exists(filePath))
            {
                //if the file is exists read the file
                IEnumerable<RequisitionModel> productList = GetProductFromXmalFile(filePath);
                return Json(productList, JsonRequestBehavior.AllowGet);
            }
            //if the file does not exists create the file
            System.IO.File.Create(filePath).Close();
            return Json(new List<RequisitionModel>(), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<RequisitionModel> GetProductFromXmalFile(string filePath)
        {
            List<RequisitionModel> products = new List<RequisitionModel>();
            var xmlData = XDocument.Load(filePath).Element("Products")?.Elements();
            foreach (XElement element in xmlData)
            {
                RequisitionModel aProduct = new RequisitionModel();
                var elementFirstAttribute = element.FirstAttribute.Value;
                aProduct.Serial = elementFirstAttribute;
                var elementValue = element.Elements();
                var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                aProduct.ProductId = Convert.ToInt32(xElements[0].Value);
                aProduct.ProductName = xElements[1].Value;
                aProduct.Quantity = Convert.ToInt32(xElements[2].Value);
                aProduct.ToBranchId = Convert.ToInt32(xElements[3].Value);
                aProduct.ToBranch = _iBranchManager.GetById(aProduct.ToBranchId);
                products.Add(aProduct);
            }

            return products;
        }


        public PartialViewResult ViewRequisitionDetails(long requisitionId)
        {
            var requisitions = _iProductManager.GetRequsitionDetailsById(requisitionId);
            return PartialView("_ViewRequisitionDetailsPartialPage", requisitions);
        }
        private void CreateXmlFile()
        {

            var filePath = Server.MapPath("~/Files/" + "Requisition_Products.xml");
            
            XDocument xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),

                new XComment("Creating an XML Tree using LINQ to XML"),

                new XElement("Students",

                    new XElement("Student", new XAttribute("Id", 101),
                        new XElement("Name", "Mark"),
                        new XElement("Gender", "Male"),
                        new XElement("TotalMarks", 800))
                    ));

            xmlDocument.Save(@"C:\Demo\Demo\Data.xml");
        }
    }
}