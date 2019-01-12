using System;

namespace NBL.Models.MobileBankings
{
    public class MobileBanking
    {

        public int MobileBankingId { get; set; }
        public string MobileBankingAccountNo { get; set; }
        public string SubSubSubAccountCode { get; set; }
        public int MobileBankingTypeId { get; set; }
        public DateTime SysDateTime { get; set; }

    }
}