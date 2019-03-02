using System;
namespace NBL.Models.ViewModels.Deliveries
{
    public class ViewDispatchModel
    {
        public long DispatchId { get; set; }
        public long TripId { get; set; }
        public string TripRef { get; set; }
        public string DispatchRef { get; set; }
        public string TransactionRef { get; set; }
        public int DispatchByUserId { get; set; }
        public int Status { get; set; }
        public string IsCancelled { get; set; }
        public DateTime DispatchDate { get; set; }
        public DateTime SystemDateTime { get; set; }
        public int CompanyId { get; set; }
        public int ToBranchId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { set; get; }
        public string ProductBarcode { get; set; }  
        public string Remarks { get; set; }
       
    }
}
