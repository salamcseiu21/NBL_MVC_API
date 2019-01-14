
using System.Collections.Generic;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.TransferProducts;

namespace NBL.Models.ViewModels.TransferProducts
{
   public  class ViewTransferIssueModel
    {
        public Branch FromBranch { get; set; }
        public Branch ToBranch { get; set; }
        public ICollection<TransferIssue> TransferIssues { get; set; } 
    }
}
