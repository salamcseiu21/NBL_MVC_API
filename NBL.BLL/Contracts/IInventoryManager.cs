﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Productions;

namespace NBL.BLL.Contracts
{
   public interface IInventoryManager
   {
       IEnumerable<ViewProduct> GetStockProductByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<ViewProduct> GetStockProductByCompanyId(int companyId);
       IEnumerable<TransactionModel> GetAllReceiveableListByBranchAndCompanyId(int branchId, int companyId);
       int ReceiveProduct(List<ScannedProduct> receiveProductList, TransactionModel model);
       int GetStockQtyByBranchAndProductId(int branchId, int productId);
       string Save(List<ScannedProduct> scannedProducts, Delivery aDelivery, int invoiceStatus, int orderStatus);
       string GenerateDeliveryReference(int maxRefNo);
       IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef);
       ICollection<TransactionModel> GetAllReceiveableProductToBranchByDeliveryId(long id);
       TransactionModel GetTransactionModelById(long id);
       int SaveScannedProductToFactoryInventory(List<ScannedProduct> scannedProducts,int userId);
       bool IsThisProductSold(string scannedBarCode);
   }
}
