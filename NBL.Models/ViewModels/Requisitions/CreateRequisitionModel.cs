
using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.ViewModels.Requisitions
{
    public class CreateRequisitionModel
    {

        public int RequisitionId { get; set; }
        public string RequisitionRef { get; set; }
        public DateTime RequisitionDate { get; set; }
        public int RequisitionByUserId { get; set; }
        public int ToBranchId { get; set; }
        public int Status { get; set; }
        public char IsCancelled { get; set; }
        public char EntryStatus { get; set; }
        public int ApproveByUserId { get; set; }
        public DateTime ApproveDateTime { get; set; }
        public DateTime SystemDateTime { get; set; }
        public List<ViewRequisitionModel> Products { get; set; }
    }
}
