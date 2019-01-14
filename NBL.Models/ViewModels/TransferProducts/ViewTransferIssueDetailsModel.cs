using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.TransferProducts;

namespace NBL.Models.ViewModels.TransferProducts
{
   public class ViewTransferIssueDetailsModel
    {
        public Branch FromBranch { get; set; }
        public Branch ToBranch { get; set; }
        public TransferIssue TransferIssue { get; set; }
        public ICollection<TransferIssueDetails> TransferIssueDetailses { get; set; }   
    }
}
