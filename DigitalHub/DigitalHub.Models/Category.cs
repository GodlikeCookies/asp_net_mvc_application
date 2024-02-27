using System.ComponentModel.DataAnnotations;

namespace DigitalHub.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }
        [Range(1, 50)]
        public int DisplayOrder { get; set; }
    }
}
