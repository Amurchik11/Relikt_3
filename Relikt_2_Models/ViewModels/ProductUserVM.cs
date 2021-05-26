using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relikt_2_Models.ViewModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductList = new List<Product>();
        }
        public ApplicationUser ApplicationUser { get; set; }
        public IList<Product> ProductList { get; set; }
        public Coupon Coupon { get; set; }
        public OrderHeader OrderHeader { get; set; } 
        public OrderDetail OrderDetail { get; set; }

        public double Total => ProductList.Sum(s => s.Price * s.TempCount);
        
        public double CountCoup { get; set; }

        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<ApplicationType> ApplicationTypes { get; set; }
        public IEnumerable<SizeType> SizeTypes { get; set; }
        public IEnumerable<RightLeft> RightLefts { get; set; }
        public IEnumerable<ColourOfGlass> ColourOfGlasses { get; set; }
        public string btn { get; set; }


    }
}
