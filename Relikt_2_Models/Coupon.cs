using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Relikt_2_Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Название")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Тип Купона")]
        [Required]
        public string CouponType { get; set; }
        
        public enum ECouponType { Процент = 0, Гривна = 1 }

        [DisplayName("Скидка")]
        [Required]
        public double Discount { get; set; }
        [DisplayName("Минимальное Количество")]
        [Required]
        public double MinimumAmount { get; set; }

        [DisplayName("Картинка")]
        public byte[] Picture { get; set; }

        [DisplayName("Активация")]
        public bool IsActive { get; set; }

    }
}
