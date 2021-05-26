using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relikt_2_Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader OrderHeader { get; set; }


        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }


        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }


        public int? ApplicationTypeId { get; set; }
        [ForeignKey("ApplicationTypeId")]
        public ApplicationType ApplicationType { get; set; }


        public int? SizeTypeId { get; set; }
        [ForeignKey("SizeTypeId")]
        public SizeType SizeType { get; set; }


        public int? RightLeftId { get; set; }
        [ForeignKey("RightLeftId")]
        public RightLeft RightLeft { get; set; }


        public int? ColourOfGlassId { get; set; }
        [ForeignKey("ColourOfGlassId")]
        public ColourOfGlass ColourOfGlass { get; set; }

        [Range(1, 100000)]
        public int UnitCount { get; set; }   
        public double PriceCount { get; set; }
    }
}
