using System.Collections.Generic;
using NBL.Models.Clients;

namespace NBL.Models.Masters 
{
    public class ClientType
    {
        public int ClientTypeId { get; set; }
        public string ClientTypeName { get; set; }
        public decimal DiscountPercent { get; set; }
        public List<Client> Clients { get; set; } 
    }
}