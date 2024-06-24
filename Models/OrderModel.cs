using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApplication.Models
{
    [Table("placed_order")]
    public class OrderModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [Required]
        public long user_id { get; set; }

        public string status { get; set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime placed_at { get; set; }
    }
}
