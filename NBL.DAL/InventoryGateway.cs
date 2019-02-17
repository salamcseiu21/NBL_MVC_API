using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Sales;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.DAL
{
    public class InventoryGateway:DbGateway,IInventoryGateway
    {
        public IEnumerable<ViewProduct> GetStockProductByBranchAndCompanyId(int branchId, int companyId)
        {

            try
            {
                CommandObj.CommandText = "UDSP_GetStockProductInByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> products = new List<ViewProduct>();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        
                        StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<ViewProduct> GetStockProductByCompanyId(int companyId)
        {

            try
            {
                CommandObj.CommandText = "UDSP_GetStockProductByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> products = new List<ViewProduct>();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                        CostPrice = Convert.ToDecimal(reader["CostPrice"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public int GetMaxDeliveryRefNoOfCurrentYear()
        {
            try
            {
                CommandObj.CommandText = "spGetMaxDeliveryRefOfCurrentYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                int slNo = 0;
                if (reader.Read())
                {
                    slNo = Convert.ToInt32(reader["MaxSlNo"]);
                }
                reader.Close();
                return slNo;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect max delivery ref of current Year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef)
        {
            try
            {
                CommandObj.CommandText = "spGetReceiveableProductToBranchByDeliveryRef";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryRef", deliveryRef);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<TransactionModel> list = new List<TransactionModel>();
                while (reader.Read())
                {
                    list.Add(new TransactionModel
                    {
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        UserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        TransactionDate = Convert.ToDateTime(reader["SysDateTime"]),
                        ProductName = reader["ProductName"].ToString(),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        Transportation = reader["Transportation"].ToString(),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        DriverName = reader["DriverName"].ToString(),
                        VehicleNo = reader["VehicleNo"].ToString(),
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"])
                    });
                }

                reader.Close();
                return list;

            }
            catch (Exception exception)
            {

                throw new Exception("Could not Get receivable product list", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ICollection<TransactionModel> GetAllReceiveableProductToBranchByDeliveryId(long id)
        {
            try
            {
                CommandObj.CommandText = "spGetReceiveableProductToBranchByDeliveryId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryId", id);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<TransactionModel> list = new List<TransactionModel>();
                while (reader.Read())
                {
                    list.Add(new TransactionModel
                    {
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        UserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        // Quantity = Convert.ToInt32(reader["Quantity"]),
                        TransactionDate = Convert.ToDateTime(reader["SysDateTime"]),
                        ProductName = reader["ProductName"].ToString(),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        Transportation = reader["Transportation"].ToString(),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        DriverName = reader["DriverName"].ToString(),
                        VehicleNo = reader["VehicleNo"].ToString(),
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        ProductBarCode = reader["ProductBarCode"].ToString()
                    });
                }

                reader.Close();
                return list;


            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect receivable product to barnch by delivery id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public TransactionModel GetTransactionModelById(long id)
        {
            try
            {
                CommandObj.CommandText = "spGetTransactionModelById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryId", id);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                TransactionModel model =null;
                if(reader.Read())
                {
                   model=new TransactionModel
                    {
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        UserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        TransactionDate = Convert.ToDateTime(reader["SysDateTime"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        Transportation = reader["Transportation"].ToString(),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        DriverName = reader["DriverName"].ToString(),
                        VehicleNo = reader["VehicleNo"].ToString(),
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        FromBranch = new Branch
                        {
                            BranchName = reader["FromBranchName"].ToString(),
                            BranchAddress = reader["FromBranchAddress"].ToString(),
                        },
                        ToBranch = new Branch
                        {
                            BranchName = reader["ToBranchName"].ToString(),
                            BranchAddress = reader["ToBranchAddress"].ToString(),
                        }
                   };
                }

                reader.Close();
                return model;


            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect transaction  to barnch by delivery id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int SaveScannedProductToFactoryInventory(List<ScannedProduct> scannedProducts,int userId)
        {
            try
            {
                int rowAffected = 0;
                foreach (var product in scannedProducts)
                {
                    CommandObj.CommandText = "UDSP_SaveProductToFactoryInventory";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    CommandObj.Parameters.Clear();
                    CommandObj.Parameters.AddWithValue("@ProductCode", product.ProductCode);
                    CommandObj.Parameters.AddWithValue("@UserId", userId);
                    CommandObj.Parameters.Add("@RowAffected", SqlDbType.BigInt);
                    CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                    ConnectionObj.Open();
                    CommandObj.ExecuteNonQuery();
                    rowAffected += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                    ConnectionObj.Close();
                }
                return rowAffected;
            }
            catch (Exception exception)
            {
                
               throw new Exception("Could not saved scanned products",exception);
            }
        }
        public ScannedProduct IsThisProductSold(string scannedBarCode)
        {
            try
            {

                CommandObj.CommandText = "UDSP_IsThisProductSold";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ScannedBarCode", scannedBarCode);
                ConnectionObj.Open();
                ScannedProduct product = null;
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    product = new ScannedProduct
                    {
                        ProductCode = reader["ProductBarCode"].ToString(),
                        ProductName = reader["ProductName"].ToString()
                    };
                }
                reader.Close();
                return product;
            }
            catch (Exception exception)
            {

                throw new Exception("Could not Get  product by barcode", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ICollection<ViewProduct> OldestProductByBarcode(string scannedBarCode)
        {
            try
            {
                List<ViewProduct> products=new List<ViewProduct>();
                CommandObj.CommandText = "UDSP_OldestProductByBarcode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BarCode", scannedBarCode);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while(reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductBarCode = reader["ProductBarCode"].ToString(),
                        ProductionDate =Convert.ToDateTime(reader["Production_Date"])
                        
                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Get  Oldest product qty by barcode", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ScannedProduct IsThisProductDispachedFromFactory(string scannedBarCode)
        {
            try
            {

                CommandObj.CommandText = "UDSP_IsThisProductDispachedFromFactory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ScannedBarCode", scannedBarCode);
                ConnectionObj.Open();
                ScannedProduct product = null;
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    product = new ScannedProduct
                    {
                        ProductCode = reader["ProductBarCode"].ToString(),
                        ProductName = reader["ProductName"].ToString()
                    };
                }
                reader.Close();
                return product;
            }
            catch (Exception exception)
            {

                throw new Exception("Could not Get  product by barcode", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ScannedProduct IsThisProductAlreadyInFactoryInventory(string scannedBarCode)
        {
            try
            {

                CommandObj.CommandText = "UDSP_IsThisProductAlreadyInFactoryInventory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ScannedBarCode", scannedBarCode);
                ConnectionObj.Open();
                ScannedProduct product = null;
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    product = new ScannedProduct
                    {
                        ProductCode = reader["ProductBarCode"].ToString(),
                        ProductName = reader["ProductName"].ToString()
                    };
                }
                reader.Close();
                return product;
            }
            catch (Exception exception)
            {

                throw new Exception("Could not Get  product by barcode", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ICollection<ViewFactoryStockModel> GetStockProductInFactory()
        {
            try
            {
                List<ViewFactoryStockModel> products=new List<ViewFactoryStockModel>();
                CommandObj.CommandText = "UDSP_GetStockProductInFactory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewFactoryStockModel
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductBarCode = reader["ProductBarCode"].ToString(),
                        ProductImage = reader["ProductImagePath"].ToString(),
                        ProductTypeId = Convert.ToInt32(reader["ProductTypeId"]),
                        ProductTypeName = reader["ProductTypeName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductionDate = Convert.ToDateTime(reader["ProductionDate"]),
                        ComeIntoStore = Convert.ToDateTime(reader["ComeIntoStore"]),
                        CategoryName = reader["ProductCategoryName"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        Age = Convert.ToInt32(reader["Age"])
                    });
                }
                reader.Close();
                return products;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect stock product in facotry store", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ICollection<ViewBranchStockModel> GetStockProductInBranchByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                List<ViewBranchStockModel> products = new List<ViewBranchStockModel>();
                CommandObj.CommandText = "UDSP_GetStockProductInBranchByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewBranchStockModel
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductBarCode = reader["ProductBarCode"].ToString(),
                        ProductImage = reader["ProductImagePath"].ToString(),
                        ProductTypeId = Convert.ToInt32(reader["ProductTypeId"]),
                        ProductTypeName = reader["ProductTypeName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductionDate = Convert.ToDateTime(reader["ProductionDate"]),
                        ComeIntoBranch = Convert.ToDateTime(reader["ComeIntoBranch"]),
                        CategoryName = reader["ProductCategoryName"].ToString(),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        Age = Convert.ToInt32(reader["Age"]),
                        AgeAtBranch = Convert.ToInt32(reader["AgeAtBranch"])
                    });
                }
                reader.Close();
                return products;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect stock product in branch store", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public ICollection<ViewProductTransactionModel> GetAllProductTransactionFromFactory()
        {
            try
            {
                List<ViewProductTransactionModel> transactions=new List<ViewProductTransactionModel>();
                CommandObj.CommandText = "UDSP_GetAllProductTransactionFromFactory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    transactions.Add(new ViewProductTransactionModel
                    {
                        TransferIssueId = Convert.ToInt32(reader["TransferIssueId"]),
                        TransactionRef = reader["TransferIssueRef"].ToString(),
                        TransferIssueDate = Convert.ToDateTime(reader["TransferIssueDate"]),
                        IssueStatus = Convert.ToInt32(reader["IssueStatus"]),
                        QuantityIssued = Convert.ToInt32(reader["QuantityIssued"]),
                        DeliveredQuantity = Convert.ToInt32(reader["DeliveredQuantity"]),
                        ReceivedQuantity = Convert.ToInt32(reader["ReceivedQuantity"]),
                        DeliveredStatus = Convert.ToInt32(reader["DeliveryStatus"]),
                        DeliveredAt = Convert.ToDateTime(reader["DeliveredAt"]),
                        ApprovedDateTime = Convert.ToDateTime(reader["ApproveDateTime"]),
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"])
                    });
                }
                reader.Close();
                return transactions;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Production transaction info",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<TransactionModel> GetAllReceiveableListByBranchAndCompanyId(int branchId,int companyId) 
        {
            try
            {
                CommandObj.CommandText = "spGetReceiveableListByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ToBranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId",companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<TransactionModel> list = new List<TransactionModel>();
                while (reader.Read())
                {
                    list.Add(new TransactionModel
                    {
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        UserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Quantity = Convert.ToInt32(reader["TotalQuantity"]),
                        TransactionDate = Convert.ToDateTime(reader["SysDateTime"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        Transportation = reader["Transportation"].ToString(),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        DriverName = reader["DriverName"].ToString(),
                        VehicleNo = reader["VehicleNo"].ToString(),
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        FromBranch = new Branch
                        {
                          BranchName  = reader["FBranchName"].ToString(),
                          BranchAddress = reader["FBranchAddress"].ToString(),
                        },
                        ToBranch = new Branch
                        {
                            BranchName = reader["ToBranchName"].ToString(),
                            BranchAddress = reader["ToBranchAddress"].ToString(),
                        }
                    });
                }

                reader.Close();
                return list;

            }
            catch (Exception exception)
            {

                throw new Exception("Could not Get receivable product list", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int ReceiveProduct(List<ScannedProduct> receiveProductList,TransactionModel model)
        {
            
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "spReceiveProuctToBranch";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransactionDate", model.TransactionDate);
                CommandObj.Parameters.AddWithValue("@TransactionRef", model.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@Quantity", model.Quantity);
                CommandObj.Parameters.AddWithValue("@ToBranchId", model.ToBranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", model.UserId);
                CommandObj.Parameters.Add("@InventoryId", SqlDbType.Int);
                CommandObj.Parameters["@InventoryId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int inventoryId = Convert.ToInt32(CommandObj.Parameters["@InventoryId"].Value);
                int rowAffected = SaveReceiveProductDetails(receiveProductList, inventoryId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;
            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not receive product", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int SaveReceiveProductDetails(List<ScannedProduct> receiveProductList, int inventoryId)
        {
            int i = 0;
            foreach (var item in receiveProductList) 
            {
                CommandObj.CommandText = "spSaveReceiveProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductBarcode", item.ProductCode);
                CommandObj.Parameters.AddWithValue("@ProductId", Convert.ToInt32(item.ProductCode.Substring(0,3)));
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }
        private int SaveReceivedProductBarCodes(string recievedProductBarCodes, int inventoryId) 
        {
            int i = 0;
            if (recievedProductBarCodes.Length != 0)
            {
                var codes = recievedProductBarCodes.TrimEnd(',').Split(',');
                foreach (string code in codes)
                {
                    CommandObj.CommandText = "spSaveReceivedProductBarCodes";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    CommandObj.Parameters.Clear();
                    CommandObj.Parameters.AddWithValue("@ProductBarcode", code);
                    CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                    CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                    CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                    CommandObj.ExecuteNonQuery();
                    i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                }
            }
            return i;
        }
        public int GetStockQtyByBranchAndProductId(int branchId, int productId)
        {
            try
            {
                int stockQty = 0;
                CommandObj.CommandText = "spGetStockQtyByBranchIdAndProductId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@ProductId", productId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    stockQty = Convert.ToInt32(reader["StockQuantity"]);
                }
                reader.Close();
                return stockQty;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect Stock Qty", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public int Save(List<ScannedProduct> scannedProducts, Delivery aDelivery, int invoiceStatus,int orderStatus)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.CommandText = "spSaveDeliveredOrderInformation";
                CommandObj.Parameters.AddWithValue("@TransactionDate", aDelivery.DeliveryDate);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aDelivery.TransactionRef);
                CommandObj.Parameters.AddWithValue("@DeliveryRef", aDelivery.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@InvoiceRef", aDelivery.InvoiceRef);
                CommandObj.Parameters.AddWithValue("@InvoiceId",aDelivery.InvoiceId);
                CommandObj.Parameters.AddWithValue("@InvoiceStatus", invoiceStatus);
                CommandObj.Parameters.AddWithValue("@OrderStatus", orderStatus);
                CommandObj.Parameters.AddWithValue("@Transportation", aDelivery.Transportation);
                CommandObj.Parameters.AddWithValue("@DriverName", aDelivery.DriverName);
                CommandObj.Parameters.AddWithValue("@DriverPhone", aDelivery.DriverPhone);
                CommandObj.Parameters.AddWithValue("@TransportationCost", aDelivery.TransportationCost);
                CommandObj.Parameters.AddWithValue("@VehicleNo", aDelivery.VehicleNo);
                CommandObj.Parameters.AddWithValue("@ToBranchId", aDelivery.ToBranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", aDelivery.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", aDelivery.DeliveredByUserId);
                CommandObj.Parameters.AddWithValue("@Quantity", scannedProducts.Count);
                CommandObj.Parameters.Add("@InventoryId", SqlDbType.Int);
                CommandObj.Parameters["@InventoryId"].Direction = ParameterDirection.Output;
                CommandObj.Parameters.Add("@DeliveryId", SqlDbType.Int);
                CommandObj.Parameters["@DeliveryId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int inventoryId = Convert.ToInt32(CommandObj.Parameters["@InventoryId"].Value);
                int deliveryId = Convert.ToInt32(CommandObj.Parameters["@DeliveryId"].Value);

                int rowAffected = SaveDeliveredOrderDetails(scannedProducts, inventoryId, deliveryId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;

            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save Order", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public int SaveDeliveredOrderDetails(List<ScannedProduct> scannedProducts, int inventoryId,int deliveryId)
        {
            int i = 0;
            foreach (var item in scannedProducts)
            {
               
                CommandObj.CommandText = "spSaveDeliveredOrderDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.AddWithValue("@ProductId", Convert.ToInt32(item.ProductCode.Substring(0,3)));
                CommandObj.Parameters.AddWithValue("@ProductBarcode", item.ProductCode);
                CommandObj.Parameters.AddWithValue("@Status", 1);
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }
        public ViewProductLifeCycleModel GetProductLifeCycleByBarcode(string productBarCode)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetProductLifeCycleByBarcode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BarCode", productBarCode);
                ViewProductLifeCycleModel model = null;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    model=new ViewProductLifeCycleModel
                    {
                        Age = Convert.ToInt32(reader["Age"]),
                        ComeIntoInventory = Convert.ToDateTime(reader["ComeIntoInventory"]),
                        ProductionDate = Convert.ToDateTime(reader["ProductionDate"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                       // Status = Convert.ToInt32(reader["Status"])
                    };
                }
                reader.Close();
                return model;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get product life cycle by barcode",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<ViewProduct> GetAllProductsBarcode()
        {
            try
            {
                List<ViewProduct> products=new List<ViewProduct>();
                CommandObj.CommandText = "UDSP_GetAllProductsBarcode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductBarCode = reader["ProductBarCode"].ToString()
                    });
                }
                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect products barcode",exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }
    }
}