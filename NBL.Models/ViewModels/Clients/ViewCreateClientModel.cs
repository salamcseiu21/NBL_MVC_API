﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace NBL.Models.ViewModels.Clients
{
    public class ViewCreateClientModel
    {
       
        [Required(ErrorMessage = "Commercial Name is required")]
        [Display(Name = "Commercial Name")]
        public string CommercialName { get; set; }
        [Required(ErrorMessage = "Client Name is required")]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [Display(Name = "NID")]
        public string NationalIdNo { get; set; }
        [Display(Name = "TIN")]
        public string TinNo { get; set; }

        [Required]

        public string Address { get; set; }
        [Required(ErrorMessage = "Client Phone is required")]
        public string Phone { get; set; }

        [Display(Name = "Alternate Phone")]
        public string AlternatePhone { get; set; }

        [Display(Name = "Contact Person")]
        [Required]
        public string ContactPerson { get; set; }
        [Display(Name = "Contact Person Phone ")]
        [Required]
        public string ContactPersonPhone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
       
        [Required]
        public string Gender { get; set; }
        [Display(Name = "Image")]

        public string ClientImage { get; set; }
        [Display(Name = "Signature")]

        public string ClientSignature { get; set; }

       
        [Required]
        [Display(Name = "Client Type")]
        public int ClientTypeId { get; set; }
        [Required]
        [Display(Name = "Region")]
        public int? RegionId { get; set; }

        [Display(Name = "District")]
        public int? DistrictId { get; set; }

        [Display(Name = "Upazilla")]
        public int? UpazillaId { get; set; }

        [Display(Name = "Post Office")]
        public int? PostOfficeId { get; set; }
        public int? UserId { get; set; }
        [Display(Name = "Territory")]
        [Required]
        public int? TerritoryId { get; set; }
        public bool EmailInUse { get; set; }
      

       

    }
}
