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
    public class InquiryHeaderRepository : Repository<InquiryHeader>, IInquiryHeaderRepository
    {
        private readonly ApplicationDbContext _db; 
        public InquiryHeaderRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        

        public void Update(InquiryHeader obj)
        {
            _db.InquiryHeader.Update(obj);
        }
    }
}
