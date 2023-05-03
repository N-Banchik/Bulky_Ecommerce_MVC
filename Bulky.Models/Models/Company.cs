using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }= string.Empty;
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Zipcode { get; set; }
    }
}
