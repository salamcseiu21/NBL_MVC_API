using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace NBL.Models.ViewModels.Requisitions
{
   public class ViewRequisitionModel
    {

        public string Serial { get; set; } 
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int ToBranchId { get; set; }    
    }
}
