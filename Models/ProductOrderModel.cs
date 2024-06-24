using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApplication.Models
{
    [Table("product_order")]
    public class ProductOrderModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [Required]
        public long order_id { get; set; }

        [Required]
        public long product_id { get; set; }

        [Required]
        public int quantity {  get; set; }
        
    }
}
