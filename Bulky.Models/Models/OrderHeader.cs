using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        [ValidateNever]
        public AppUser User { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public double OrderTotal { get; set; }

        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }

        public DateTime PaymentDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        //Id from strip for Credit card
        public string? PaymentIntentId { get; set; }
        public string? SessionId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Zipcode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;




    }
}
