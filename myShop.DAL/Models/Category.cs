using myShop.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace myshop.Entities.Models
{
    public class Category : BaseEntity<int>
    {
        

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
