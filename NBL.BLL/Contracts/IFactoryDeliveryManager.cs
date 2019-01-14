﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;

namespace NBL.BLL.Contracts
{
    public interface IFactoryDeliveryManager:IManager<Delivery>
    {
        string SaveDeliveryInformation(Delivery aDelivery, IEnumerable<TransferIssueDetails> issueDetails);
    }
}
