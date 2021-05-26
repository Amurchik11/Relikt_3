using Relikt_2_Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Relikt_2_DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);
    }
}
