using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relikt_2_Models
{
    public class InquiryDetail
    {
        
        public int Id { get; set; }

        [Required]
        public int InquiryHeaderId { get; set; }
        [ForeignKey("InquiryHeaderId")]
        public InquiryHeader InquiryHeader { get; set; }

        
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Display(Name = "Тип Категории")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Display(Name = "Тип Товара")]
        public int ApplicationTypeId { get; set; }
        [ForeignKey("ApplicationTypeId")]
        public ApplicationType ApplicationType { get; set; }

        [Display(Name = "Размер Товара")]
        public int SizeTypeId { get; set; }
        [ForeignKey("SizeTypeId")]
        public SizeType SizeType { get; set; }

        [Display(Name = "Модификация Товара")]
        public int? RightLeftId { get; set; }
        [ForeignKey("RightLeftId")]
        public RightLeft RightLeft { get; set; }

        [Display(Name = "Цвет Стекла")]
        public int? ColourOfGlassId { get; set; }
        [ForeignKey("ColourOfGlassId")]
        public ColourOfGlass ColourOfGlass { get; set; }
    }
}
