﻿
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.Orders;
using NBL.Models.ViewModels;

namespace NBL.Areas.Admin.BLL.Contracts
{
    public interface IInvoiceManager
    {
        string Save(IEnumerable<OrderItem> orderItems, Invoice anInvoice);
        IEnumerable<Invoice> GetAllInvoicedOrdersByBranchAndCompanyId(int branchId, int companyId);
        IEnumerable<Invoice> GetAllInvoicedOrdersByBranchCompanyAndUserId(int branchId, int companyId,
            int invoiceByUserId);
        IEnumerable<Invoice> GetAllInvoicedOrdersByUserId(int invoiceByUserId);
        IEnumerable<Invoice> GetInvoicedRefferencesByClientId(int clientId);
        IEnumerable<Invoice> GetAllInvoicedOrdersByCompanyId(int companyId);
        IEnumerable<InvoiceDetails> GetInvoicedOrderDetailsByInvoiceId(int invoiceId);
        IEnumerable<InvoiceDetails> GetInvoicedOrderDetailsByInvoiceRef(string invoiceRef);
        Invoice GetInvoicedOrderByInvoiceId(int invoiceId);
        ICollection<ViewProduct> GetDeliveredProductsByInvoiceRef(string invoiceRef); 
    }
}
