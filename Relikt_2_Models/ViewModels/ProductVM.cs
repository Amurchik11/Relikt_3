using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relikt_2_Models.ViewModels
{
    public class ProductVM
    {
        //public Page<Product> ProductsPage { get; set; }
        public Product Product { get; set; }
        public bool ViewOptions { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
        public IEnumerable<SelectListItem> ApplicationTypeSelectList { get; set; }

        public IEnumerable<SelectListItem> SizeTypeSelectList { get; set; }
        public IEnumerable<SelectListItem> RightLeftSelectList { get; set; }
        public IEnumerable<SelectListItem> ColourOfGlassSelectList { get; set; }
        public IEnumerable<SelectListItem> CouponSelectList { get; set; } 
    }
}
