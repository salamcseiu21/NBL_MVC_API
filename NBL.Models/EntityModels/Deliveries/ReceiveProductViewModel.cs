﻿
using System.Collections.Generic;
using NBL.Models.EntityModels.TransferProducts;

namespace NBL.Models.EntityModels.Deliveries
{
    public class ReceiveProductViewModel 
    {
        public long Id { get; set; }
        public long DeliveryId { get; set; }    
        public TransactionModel TransactionModel { get; set; } 
        public ICollection<TransactionModel> TransactionModels { get; set; }
        

    }
}
