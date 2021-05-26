using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relikt_2_Models.ViewModels
{
    public class OrderVM
    {
        //public List<Product> Products { get; set; }
        //public Coupon Coupon { get; set; }
        public OrderHeader OrderHeader { get; set; }
        
        //public double Total => Products.Sum(s => s.Price * s.TempCount);
        public IEnumerable<OrderDetail> OrderDetail { get; set; }
        
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<ApplicationType> ApplicationTypes { get; set; }
        public IEnumerable<SizeType> SizeTypes { get; set; }
        public IEnumerable<RightLeft> RightLefts { get; set; }
        public IEnumerable<ColourOfGlass> ColourOfGlasses { get; set; }

        public IEnumerable<Coupon> Coupons { get; set; }


    }
}
