using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Relikt_2_Models
{
    public class Product
    {
        public Product()
        {
            TempCount = 1;
        }

        [Key]
        public int Id { get; set; }
        [DisplayName("Название")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Краткое Описание")]
        public string ShortDesc { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        [Range(1, int.MaxValue)]
        [DisplayName("Цена")]
        public double Price { get; set; }
        public string Image { get; set; }
        [Display(Name = "Тип Категории")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "Тип Товара")]
        public int ApplicationTypeId { get; set; }
        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }

        [Display(Name = "Размер Товара")]
        public int SizeTypeId { get; set; }
        [ForeignKey("SizeTypeId")] 
        public virtual SizeType SizeType { get; set; }

        [Display(Name = "Модификация Товара")]
        public int? RightLeftId { get; set; }
        [ForeignKey("RightLeftId")]
        public virtual RightLeft RightLeft { get; set; }

        [Display(Name = "Цвет Стекла")]
        public int? ColourOfGlassId { get; set; }
        [ForeignKey("ColourOfGlassId")]
        public virtual ColourOfGlass ColourOfGlass { get; set; }

        public ICollection<InquiryDetail> InquiryDetail { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }

        //public Page<Product> ProductsPage { get; set; }

        [NotMapped]
        [Range(1, 10000, ErrorMessage = "Количество должно быть более 0.")]
        public int TempCount { get; set; }
    }
}
