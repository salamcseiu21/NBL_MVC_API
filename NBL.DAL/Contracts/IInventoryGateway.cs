using System.Collections.Generic;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Sales;
using NBL.Models.ViewModels.TransferProducts;

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
       ICollection<ViewProduct> OldestProductByBarcode(string scannedBarCode);
       ScannedProduct IsThisProductDispachedFromFactory(string scannedBarCode);
       ScannedProduct IsThisProductAlreadyInFactoryInventory(string scannedBarCode);
       ICollection<ViewFactoryStockModel> GetStockProductInFactory();
       ICollection<ViewBranchStockModel> GetStockProductInBranchByBranchAndCompanyId(int branchId, int companyId);
       ICollection<ViewProductTransactionModel> GetAllProductTransactionFromFactory();
       ViewProductLifeCycleModel GetProductLifeCycleByBarcode(string productBarCode);
       IEnumerable<ViewProduct> GetAllProductsBarcode();
   }
}
