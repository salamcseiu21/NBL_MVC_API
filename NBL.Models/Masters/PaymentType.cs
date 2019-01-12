using System.Collections.Generic;
using NBL.Models.Payments;

namespace NBL.Models.Masters
{
    public class PaymentType
    {
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public string PaymentTypeAccountCode  { get; set; }
        public List<Payment> Payments { get; set; } 

    }
}