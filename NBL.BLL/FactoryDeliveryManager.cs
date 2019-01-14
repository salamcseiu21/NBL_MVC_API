using System;
using System.Collections.Generic;
using System.Linq;
using NBL.DAL;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;

namespace NBL.BLL
{
    public class FactoryDeliveryManager
    {
        readonly FactoryDeliveryGateway _factoryDeliveryGateway = new FactoryDeliveryGateway();
        readonly InventoryGateway _inventoryGateway = new InventoryGateway();
        readonly CommonGateway _commonGateway = new CommonGateway();
        public string SaveDeliveryInformation(Delivery aDelivery, IEnumerable<TransferIssueDetails> issueDetails)
        {
            int maxDeliveryNo = _inventoryGateway.GetMaxDeliveryRefNoOfCurrentYear();
            aDelivery.DeliveryRef = GenerateDeliveryReference(maxDeliveryNo);
            int rowAffected = _factoryDeliveryGateway.SaveDeliveryInformation(aDelivery, issueDetails);
            if (rowAffected > 0)
                return "Saved Successfully!";
            return "Failed to Save";
        }
        /// <summary>
        /// id=4 stands from factory......
        /// </summary>
        /// <param name="maxDeliveryNo"></param>
        /// <returns></returns>
        private string GenerateDeliveryReference(int maxDeliveryNo)
        {
            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == 4).Code;
            string temp = (maxDeliveryNo + 1).ToString();
            string reference = DateTime.Now.Year.ToString().Substring(2, 2) + refCode + temp;
            return reference;
        }
    }
}