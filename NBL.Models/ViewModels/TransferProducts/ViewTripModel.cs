using System;
namespace NBL.Models.ViewModels.TransferProducts
{
   public class ViewTripModel
    {

        public string Id { get; set; } 
        public long RequisitionId { get; set; }
        public string RequisitionRef { get; set; }
        public int RequisitionQty { get; set; }
        public int DeliveryQuantity { get; set; } 
        public int ToBranchId { get; set; }
        public int ProuctId { get; set; }
        public string ProuctName { get; set; }  
    }
}
