using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Invoices;

namespace NBL.Models.ViewModels.Deliveries
{
   public class ViewDeliveryModel
    {
        public ICollection<InvoiceDetails> InvoiceDetailses { get; set; }
        public Client Client { get; set; }
        public Invoice Invoice { get; set; } 
    }
}
