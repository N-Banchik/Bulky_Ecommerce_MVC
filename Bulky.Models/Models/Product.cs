using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.Models
{
    public class Product
    {

        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string ISBN { get; set; } = "";
        public string Author { get; set; } = "";
        [DisplayName("List Price")]
        [Range(1,10000)]
        
        public double ListPrice { get; set; }
        [DisplayName("Price for 1-49")]
        [Range(1, 10000)]
        public double Price { get; set; }
        [DisplayName("Price for 50+")]
        [Range(1, 10000)]
        public double Price50 { get; set; }
        [DisplayName("Price for 100+")]
        [Range(1, 10000)]
        public double Price100 { get; set; }
        [DisplayName("Image")]
        public string? ImageUrl { get; set; } = "";
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
