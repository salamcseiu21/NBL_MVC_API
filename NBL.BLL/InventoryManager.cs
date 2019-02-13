using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Enums;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Productions;

namespace NBL.BLL
{
    public class InventoryManager:IInventoryManager
    {

        private readonly  IInventoryGateway _iInventoryGateway;

        public InventoryManager(IInventoryGateway iInventoryGateway)
        {
            _iInventoryGateway = iInventoryGateway;
        }
        readonly CommonGateway _commonGateway=new CommonGateway();
        public IEnumerable<ViewProduct> GetStockProductByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iInventoryGateway.GetStockProductByBranchAndCompanyId(branchId, companyId);
        }
        public IEnumerable<ViewProduct> GetStockProductByCompanyId(int companyId)
        {
            return _iInventoryGateway.GetStockProductByCompanyId(companyId);
        }

        public IEnumerable<TransactionModel> GetAllReceiveableListByBranchAndCompanyId(int branchId,int companyId)
        {
            return _iInventoryGateway.GetAllReceiveableListByBranchAndCompanyId(branchId,companyId); 
        }

        public int ReceiveProduct(List<ScannedProduct> receiveProductList, TransactionModel model) 
        {
            
            return _iInventoryGateway.ReceiveProduct(receiveProductList, model);
        }

        public IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef)
        {
            return _iInventoryGateway.GetAllReceiveableProductToBranchByDeliveryRef(deliveryRef);
        }
        public int GetStockQtyByBranchAndProductId(int branchId, int productId)
        {
            return _iInventoryGateway.GetStockQtyByBranchAndProductId(branchId,productId);
        }

        public string Save(List<ScannedProduct> scannedProducts, Delivery aDelivery,int invoiceStatus,int orderStatus)
        {

            int maxRefNo = _iInventoryGateway.GetMaxDeliveryRefNoOfCurrentYear();
            aDelivery.DeliveryRef = GenerateDeliveryReference(maxRefNo);
            int rowAffected = _iInventoryGateway.Save(scannedProducts, aDelivery, invoiceStatus, orderStatus);
            return rowAffected > 0 ? "Saved Successfully!" : "Failed to Save";
        }

        public string GenerateDeliveryReference(int maxRefNo)
        {

            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id.Equals(Convert.ToInt32(ReferenceType.Distribution))).Code;
            string temp = (maxRefNo + 1).ToString();
            string reference =DateTime.Now.Year.ToString().Substring(2,2)+refCode+ temp;
            return reference;
        }

        public ICollection<TransactionModel> GetAllReceiveableProductToBranchByDeliveryId(long id)
        {
            return _iInventoryGateway.GetAllReceiveableProductToBranchByDeliveryId(id);
        }

        public TransactionModel GetTransactionModelById(long id)
        {
            return _iInventoryGateway.GetTransactionModelById(id);
        }

        public int SaveScannedProductToFactoryInventory(List<ScannedProduct> scannedProducts,int userId)
        {
            return _iInventoryGateway.SaveScannedProductToFactoryInventory(scannedProducts, userId);
        }

        public bool IsThisProductSold(string scannedBarCode)
        {
            var scannedProduct = _iInventoryGateway.IsThisProductSold(scannedBarCode);
            return scannedProduct != null;
        }

        public ICollection<ViewProduct> OldestProductByBarcode(string scannedBarCode)
        {
            return _iInventoryGateway.OldestProductByBarcode(scannedBarCode);
        }

        public bool IsThisProductDispachedFromFactory(string scannedBarCode)
        {
            var scannedProduct = _iInventoryGateway.IsThisProductDispachedFromFactory(scannedBarCode); 
            return scannedProduct != null;
        }
    }
}