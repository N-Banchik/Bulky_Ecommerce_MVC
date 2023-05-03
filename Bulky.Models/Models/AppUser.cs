using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Zipcode { get; set; }
        public int? CompanyId{ get; set; }
        [ValidateNever]
        public Company? Company { get; set; }
    }
}
