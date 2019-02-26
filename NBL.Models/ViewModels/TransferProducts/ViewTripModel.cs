﻿using System;
namespace NBL.Models.ViewModels.TransferProducts
{
   public class ViewTripModel
    {

        public string Id { get; set; }
        public long TripId { get; set; }
        public string TripRef { get; set; }
        public long RequisitionId { get; set; }
        public string RequisitionRef { get; set; }
        public int RequisitionQty { get; set; }
        public int DeliveryQuantity { get; set; } 
        public int ToBranchId { get; set; }
        public int ProuctId { get; set; }
        public string ProuctName { get; set; }
        public string Remarks { get; set; }
        public string Transportation { set; get; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public decimal TransportationCost { get; set; }
        public string VehicleNo { get; set; }
        public int CreatedByUserId { get; set; }
    }
}
