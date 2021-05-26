using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relikt_2_Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        public string CreatedByUserId { get; set; }
        [ForeignKey("CreatedByUserId")]
        public ApplicationUser CreatedBy { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy  HH:mm}")]
        public DateTime OrderDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy  HH:mm}")]
        public DateTime ShippingDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy  HH:mm}")]
        public DateTime CancellDate { get; set; } 
        [Required]
        //[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double FinalOrderTotal { get; set; }
        public string OrderStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Region { get; set; }
        [Required]
        public string PostalCode { get; set; } 
        [Required]
        public string FullName { get; set; }
        public string Email { get; set; }

        [Display(Name = "Код купона")]
        public string CouponCode { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double CouponCodeDiscount { get; set; }

        public string Comments { get; set; }


        public ICollection<OrderDetail> OrderDetail { get; set; }

        //[NotMapped]
        //public string btn { get; set; }

    }
}
