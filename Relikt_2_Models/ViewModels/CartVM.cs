using System.Collections.Generic;
using System.Linq;

namespace Relikt_2_Models.ViewModels
{
    public class CartVM
    {
        public List<Product> Products { get; set; }
        public Coupon Coupon { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public double Total => Products.Sum(s => s.Price * s.TempCount);
        public string btn { get; set; }
    }
}
