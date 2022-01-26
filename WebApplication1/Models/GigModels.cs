using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    [Table("gigdetails")]
    public class Gig
    {
        [Key]
        public int GigID { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        [Required]
        [StringLength(300, MinimumLength = 5)]
        public string Description { get; set; }
        [Required]
        [Range(5, 1000)]
        public int Price { get; set; }
        [Range(1, 30)]
        [Display(Name = "Delivery days")]
        public int Deliverytime { get; set; }
        public ApplicationUser Seller { get; set; }

    }

}