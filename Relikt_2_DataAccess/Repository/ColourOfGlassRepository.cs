﻿using Relikt_2_DataAccess.Repository.IRepository;
using Relikt_2_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relikt_2_DataAccess.Repository
{
    public class ColourOfGlassRepository : Repository<ColourOfGlass>, IColourOfGlassRepository
    {
        private readonly ApplicationDbContext _db;
        public ColourOfGlassRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Update(ColourOfGlass obj)
        {
            var objFromDb = base.FirstOrDefault(u => u.Id == obj.Id);
            if(objFromDb!=null)
            {
                objFromDb.Name = obj.Name;
                
            }
        }
    }
}
