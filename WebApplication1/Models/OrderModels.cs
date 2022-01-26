using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    [Table("orderdetails")]
    public class Order
    {
        [Key]
        [Required]
        public int OrderID { get; set; }
        [Required]
        public Gig Gig { get; set; }

        [Required(ErrorMessage = "Please enter the amount 1-100")]
        [Range(1, 100)]
        public int Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Order start date")]
        public DateTime OrderDate { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public ApplicationUser Buyer { get; set; }

    }

    public class MakeOrderModel {
        public Order Order {get;set;}
        public Gig Gig { get; set; }
    }

}