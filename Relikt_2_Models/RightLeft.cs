using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Relikt_2_Models
{
    public class RightLeft
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Название")]
        [Required]
        public string Name { get; set; }

        public ICollection<InquiryDetail> InquiryDetail { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }

    }
}
