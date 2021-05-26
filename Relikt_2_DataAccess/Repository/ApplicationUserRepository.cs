using Relikt_2_DataAccess.Repository.IRepository;
using Relikt_2_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relikt_2_DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db; 
        public ApplicationUserRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        
    }
}
