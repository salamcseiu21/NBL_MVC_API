
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.ViewModels.Productions
{
    public class ScanProductViewModel
    {
        [StringLength(15,MinimumLength = 15,ErrorMessage = "Barcode must be exact 15 character long.")]
        [Display(Name = "Product Barcode")]
        public string ProductCode { get; set; }
        public ICollection<ScannedProduct> BarCodes { get; set; } 
       
    }
}
