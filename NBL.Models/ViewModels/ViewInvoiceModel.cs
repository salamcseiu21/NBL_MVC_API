using System.Collections.Generic;
using NBL.Models.Invoices;
using NBL.Models.Orders;

namespace NBL.Models.ViewModels
{
   public class ViewInvoiceModel
    {
        public IEnumerable<InvoiceDetails> InvoiceDetailses { get; set; }
        public ViewClient Client { get; set; }
        public Order Order { get; set; }
        public Invoice Invoice { get; set; } 
    }
}
