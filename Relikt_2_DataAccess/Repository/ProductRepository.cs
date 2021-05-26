using Microsoft.AspNetCore.Mvc.Rendering;
using Relikt_2_DataAccess.Repository.IRepository;
using Relikt_2_Models;
using Relikt_2_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relikt_2_DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db; 
        public ProductRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            if(obj==WC.CategoryName)
            {
                return _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            if(obj==WC.ApplicationTypeName)
            {
                return _db.ApplicationType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            if(obj==WC.SizeTypeName)
            {
                return _db.SizeType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            if (obj == WC.RightLeftName)
            {
                return _db.RightLeft.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            if (obj == WC.ColourOfGlassName)
            {
                return _db.ColourOfGlass.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            
            if (obj == WC.ssCouponCode)
            {
                return _db.Coupon.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return null;
        }

        public void Update(Product obj)
        {
            _db.Product.Update(obj);
        }
    }
}
