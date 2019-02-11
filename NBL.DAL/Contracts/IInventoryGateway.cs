using System;
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

namespace NBL.DAL.Contracts
{
   public interface IInventoryGateway
   {
       IEnumerable<ViewProduct> GetStockProductByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<ViewProduct> GetStockProductByCompanyId(int companyId);
       int GetMaxDeliveryRefNoOfCurrentYear();
       IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef);
       IEnumerable<TransactionModel> GetAllReceiveableListByBranchAndCompanyId(int branchId, int companyId);
       int ReceiveProduct(List<ScannedProduct> receiveProductList, TransactionModel model);
       int SaveReceiveProductDetails(List<ScannedProduct> receiveProductList, int inventoryId);
       int GetStockQtyByBranchAndProductId(int branchId, int productId);
       int Save(List<ScannedProduct> scannedProducts, Delivery aDelivery, int invoiceStatus, int orderStatus); 
       int SaveDeliveredOrderDetails(List<ScannedProduct> scannedProducts, int inventoryId, int deliveryId);
       ICollection<TransactionModel> GetAllReceiveableProductToBranchByDeliveryId(long id);
       TransactionModel GetTransactionModelById(long id);
       int SaveScannedProductToFactoryInventory(List<ScannedProduct> scannedProducts,int userId);
       ScannedProduct IsThisProductSold(string scannedBarCode);
   }
}
