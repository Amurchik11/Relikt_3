using Microsoft.AspNetCore.Mvc.Rendering;
using Relikt_2_Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Relikt_2_DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);

        IEnumerable<SelectListItem> GetAllDropdownList(string obj);
    }
}
