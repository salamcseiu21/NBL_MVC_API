using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels.Productions;

namespace NBL.DAL
{
    public class FactoryDeliveryGateway : DbGateway,IFactoryDeliveryGateway
    {
        public int SaveDeliveryInformation(Delivery aDelivery, IEnumerable<TransferIssueDetails> issueDetails)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "spSaveDeliveryInformation";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryDate", aDelivery.DeliveryDate);
                CommandObj.Parameters.AddWithValue("@DeliveryRef", aDelivery.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aDelivery.TransactionRef);
                CommandObj.Parameters.AddWithValue("@Transportation", aDelivery.Transportation);
                CommandObj.Parameters.AddWithValue("@DriverName", aDelivery.DriverName);
                CommandObj.Parameters.AddWithValue("@TransportationCost", aDelivery.TransportationCost);
                CommandObj.Parameters.AddWithValue("@VehicleNo", aDelivery.VehicleNo);
                CommandObj.Parameters.AddWithValue("@ToBranchId", aDelivery.ToBranchId);
                CommandObj.Parameters.AddWithValue("@FromBranchId", aDelivery.FromBranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", aDelivery.CompanyId);
                CommandObj.Parameters.AddWithValue("@DeliveredByUserId", aDelivery.DeliveredByUserId);
                CommandObj.Parameters.Add("@DeliveryId", SqlDbType.Int);
                CommandObj.Parameters["@DeliveryId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int deliveryId = Convert.ToInt32(CommandObj.Parameters["@DeliveryId"].Value);
                int rowAffected = SaveDeliveryInformationDetails(issueDetails, deliveryId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;

            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save delivery Info", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        
        public int SaveDeliveryInformationDetails(IEnumerable<TransferIssueDetails> issueDetails, int deliveryId)
        {
            int i;
            int n = 0;
            foreach (TransferIssueDetails tr in issueDetails)
            {
                CommandObj.CommandText = "spSaveDeliveryInformationDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", tr.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", tr.Quantity);
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                CommandObj.Parameters.AddWithValue("@ProductBarCodes", tr.ProductBarCodes);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i=Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                if (i > 0)
                {
                    n += SaveDeliveredProductsBarcode(tr.BarCodes, tr.TransferIssueId);
                }
            }

            return n;
        }

        private int SaveDeliveredProductsBarcode(ICollection<ScannedBarCode> trBarCodes, int transferIssueId)
        {
            int rowAffected = 0;
            foreach (var product in trBarCodes)
            {
                CommandObj.CommandText = "UDSP_SaveProductToFactoryInventory";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductCode", product.ProductCode);
                CommandObj.Parameters.AddWithValue("@IssueId", transferIssueId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.BigInt);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                rowAffected += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return rowAffected;
        }

        public int Add(Delivery model)
        {
            throw new NotImplementedException();
        }

        public int Update(Delivery model)
        {
            throw new NotImplementedException();
        }

        public int Delete(Delivery model)
        {
            throw new NotImplementedException();
        }

        public Delivery GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Delivery> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}