using System.Collections.Generic;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.TransferProducts;

namespace NBL.DAL.Contracts
{
    public interface IFactoryDeliveryGateway:IGateway<Delivery>
    {
        int SaveDeliveryInformation(Delivery aDelivery, IEnumerable<TransferIssueDetails> issueDetails);
        int SaveDeliveryInformationDetails(IEnumerable<TransferIssueDetails> issueDetails, int deliveryId);
    }
}
