using System.ComponentModel.DataAnnotations;
namespace NBL.Models.Masters
{
   public class TransactionType
    {
       public int TransactionTypeId { set; get; }
        [Display(Name = "Transaction Type Name")]
       public string TransactionTypeName { set; get; }
    }
}
