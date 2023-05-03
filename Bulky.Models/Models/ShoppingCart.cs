using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ValidateNever]
        public AppUser? User { get; set; }
        [ValidateNever]
        public Product? Product { get; set; }
        public int ProductId { get; set; }
        [Range(1,1000,ErrorMessage ="Please Select Amount from 1 And 1000 ")]
        public int Count { get; set; }

        [NotMapped]
        public double PriceTotal { get; set; }
    }
}
