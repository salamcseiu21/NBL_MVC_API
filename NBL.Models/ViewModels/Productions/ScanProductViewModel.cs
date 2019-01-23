
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.ViewModels.Productions
{
    public class ScanProductViewModel
    {
        [Required(ErrorMessage = "Please scan barcode or tpye the product code")]
        [StringLength(13,MinimumLength = 13,ErrorMessage = "Barcode must be exact 13 character long.")]
        [Display(Name = "Product Barcode")]
        public string ProductCode { get; set; }
        public ICollection<Product> Products { get; set; }
       
    }
}
