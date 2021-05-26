using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relikt_2_Models.ViewModels
{
    public class HomeVM
    {
        public Page<Product> ProductsPage { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<ApplicationType> ApplicationTypes { get; set; }
        public IEnumerable<SizeType> SizeTypes { get; set; }
        public IEnumerable<RightLeft> RightLefts { get; set; }
        public IEnumerable<ColourOfGlass> ColourOfGlasses { get; set; }
        public IEnumerable<Coupon> Coupons { get; set; }

        //public int PageLinks { get; set; }
        ////public int TotalPages { get; private set; }
        //public int PageInfo { get; set; }
        //public int PageCount { get; set; }
        //public int CurrentPageIndex { get; set; }


    }
}
