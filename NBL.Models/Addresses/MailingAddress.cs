
using NBL.Models.Locations;

namespace NBL.Models.Addresses
{
    public class MailingAddress
    {
        public string HouseNo { get; set; }
        public PostOffice PostOffice { get; set; }
        public District District { get; set; }
        public Division Division { get; set; }
        public Upazilla Upazilla { get; set; }
    }
}