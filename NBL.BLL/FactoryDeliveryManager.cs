using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.Enums;
using NBL.Models.ViewModels.Productions;

namespace NBL.BLL
{
    public class FactoryDeliveryManager:IFactoryDeliveryManager
    {
        private readonly IFactoryDeliveryGateway _iFactoryDeliveryGateway;
        readonly InventoryGateway _inventoryGateway = new InventoryGateway();
        readonly CommonGateway _commonGateway = new CommonGateway();

        public FactoryDeliveryManager(IFactoryDeliveryGateway iFactoryDeliveryGateway)
        {
            _iFactoryDeliveryGateway = iFactoryDeliveryGateway;
        }
        public string SaveDeliveryInformation(Delivery aDelivery, IEnumerable<ScannedProduct> scannedProducts)
        {
            int maxDeliveryNo = _inventoryGateway.GetMaxDeliveryRefNoOfCurrentYear();
            aDelivery.DeliveryRef = GenerateDeliveryReference(maxDeliveryNo);
            int rowAffected = _iFactoryDeliveryGateway.SaveDeliveryInformation(aDelivery, scannedProducts);
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
            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == Convert.ToInt32(ReferenceType.Delivery)).Code;
            string temp = (maxDeliveryNo + 1).ToString();
            string reference = DateTime.Now.Year.ToString().Substring(2, 2) + refCode + temp;
            return reference;
        }

        public bool Add(Delivery model)
        {
            throw new NotImplementedException();
        }

        public bool Update(Delivery model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Delivery model)
        {
            throw new NotImplementedException();
        }

        public Delivery GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Delivery> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}