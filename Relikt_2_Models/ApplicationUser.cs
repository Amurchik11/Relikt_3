using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Relikt_2_Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        [DisplayName("Номер отделения Новой Почты, или дома проживания")]
        [NotMapped]
        public string PostalCode { get; set; }
        [DisplayName("Улица проживания, или Новой Почты")]
        [NotMapped]
        public string StreetAddress { get; set; }
        [DisplayName("Город, населенный пункт")]
        [NotMapped]
        public string City { get; set; }
        [DisplayName("Область")]
        [NotMapped]
        public string Region { get; set; }

        

    }
}
