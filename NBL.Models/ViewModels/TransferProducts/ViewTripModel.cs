using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.TransferProducts
{
   public class ViewTripModel
    {
        public long RequisitionId { get; set; }
        public string RequisitionRef { get; set; }
        public int Quantity { get; set; }   
    }
}
