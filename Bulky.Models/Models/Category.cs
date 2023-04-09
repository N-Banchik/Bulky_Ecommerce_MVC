using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models.Models
{
    public class Category
    {
        public int Id { get; set; }
        [DisplayName("Category Name")]
        [MaxLength(40)]
        public string Name { get; set; } = "";
        [DisplayName("Display Order")]
        [Range(1, 150)]
        public int DisplayOrder { get; set; }


    }
}
